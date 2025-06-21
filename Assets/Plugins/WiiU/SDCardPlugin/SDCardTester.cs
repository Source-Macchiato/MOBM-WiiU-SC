/*using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;

public class SDCardTester : MonoBehaviour
{
    [DllImport("SDCardPlugin")]
    private static extern bool SDCard_Init();

    [DllImport("SDCardPlugin")]
    private static extern void SDCard_Finalize();

    [DllImport("SDCardPlugin")]
    private static extern IntPtr GetListOfDirectories();

    [DllImport("SDCardPlugin")]
    private static extern IntPtr GetLogMessages();

    [DllImport("SDCardPlugin")]
    private static extern bool ReadFileData(IntPtr filePath);

    [DllImport("SDCardPlugin")]
    private static extern bool WriteFile(IntPtr filePath, byte[] data, int dataSize);

    [DllImport("SDCardPlugin")]
    private static extern IntPtr GetFileData();

    [DllImport("SDCardPlugin")]
    private static extern int GetFileDataSize();

    public Text uiText;
    public Text fileContentText;
    public TMP_Text logText; // LogText pour TextMeshPro
    public TMP_Text verificationResultText; // TextMeshPro pour le résultat
    public InputField filePathInput;

    void Start()
    {
        if (SDCard_Init())
        {
            CheckForVerification();
        }
        else
        {
            logText.text += "SD Card initialization failed.\n";
        }
    }

    void OnApplicationQuit()
    {
        SDCard_Finalize();
    }

    private void CheckForVerification()
    {
        string verifiedFilePath = "verifiedMOBM.SrcM";
        bool verifiedExists = FileExists(verifiedFilePath);
        if (verifiedExists)
        {
            logText.text += "verifiedMOBM.SrcM found. Calling SRCMVerification().\n";
            SRCMVerification();
        }
        else
        {
            logText.text += "verifiedMOBM.SrcM not found. Checking for PMKM2.exe...\n";
            string exeFilePath = "PMKM2.exe";
            bool exeExists = FileExists(exeFilePath);
            if (exeExists)
            {
                logText.text += "PMKM2.exe found. Calling CheckEXE().\n";
                CheckEXE();
            }
            else
            {
                logText.text += "PMKM2.exe not found. Calling CheckError().\n";
                CheckError();
            }
        }
    }

    private bool FileExists(string filePath)
    {
        IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);
        bool exists = ReadFileData(pathConverted);
        Marshal.FreeHGlobal(pathConverted);
        return exists;
    }

    private void SRCMVerification()
    {
        logText.text += "SRCM Verification in progress...\n";
        Debug.Log("SRCMVerification() called.");
        // Ajouter la logique de vérification ici
    }

    public void CheckEXE()
    {
        bool readSuccess = ReadFile("PMKM2.exe");
        if (readSuccess)
        {
            string computedHash = fileContentText.text;
            string expectedHash = "d49fa11f525376e4b33526a5ed4195b4";
            if (computedHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                logText.text += "Verification successful.\n";
                verificationResultText.text = "Verification successful";
            }
            else
            {
                logText.text += string.Format("Hash mismatch. Computed: {0}, Expected: {1}\n", computedHash, expectedHash);
                verificationResultText.text = "Verification failed";
                CheckError();
            }
        }
        else
        {
            logText.text += "Failed to read PMKM2.exe.\n";
            CheckError();
        }
    }

    private void CheckError()
    {
        logText.text += "An error occurred during verification.\n";
        Debug.Log("CheckError() called.");
        // Ajouter la gestion d'erreur ici
    }

    // Les autres fonctions existantes (ListDirectories, ReadFile, WriteFile, etc.) restent inchangées
    // ... [Le reste du code fourni est conservé avec les modifications nécessaires] ...

    public bool ReadFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is empty!");
            logText.text += "File path is empty!\n";
            return false;
        }
        IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

        if (ReadFileData(pathConverted))
        {
            IntPtr fileDataPtr = GetFileData();
            int fileSize = GetFileDataSize();
            if (fileSize > 0 && fileDataPtr != IntPtr.Zero)
            {
                byte[] fileBytes = new byte[fileSize];
                Marshal.Copy(fileDataPtr, fileBytes, 0, fileSize);

                string result = string.Empty;
                using (HashAlgorithm algorithm = new MD5CryptoServiceProvider())
                {
                    result = BitConverter.ToString(algorithm.ComputeHash(fileBytes)).ToLower().Replace("-", "");
                }

                fileContentText.text = result;
                Marshal.FreeHGlobal(pathConverted);
                return true;
            }
            else
            {
                Debug.LogError("Failed to read file data or file is empty.");
                logText.text += "Failed to read file data or file is empty.\n";
                Marshal.FreeHGlobal(pathConverted);
                return false;
            }
        }
        else
        {
            Debug.LogError("Failed to read file: " + filePath);
            logText.text += string.Format("Failed to read file: {0}\n", filePath);
            Marshal.FreeHGlobal(pathConverted);
            return false;
        }
    }
    public void ListDirectories()
    {
        Debug.Log("Listing folders in the root of the SD card.");
        errors.text = "Listing folders in the root of the SD card.";
        // Get directory list
        IntPtr intPtr = GetListOfDirectories();
        string directories = Marshal.PtrToStringAnsi(intPtr);
        uiText.text = directories;
    }

    public void ReadFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is empty!");
            errors.text = "File path is empty!";
            return;
        }
        IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

        if (ReadFileData(pathConverted))
        {
            IntPtr fileDataPtr = GetFileData();
            int fileSize = GetFileDataSize();
            if (fileSize > 0 && fileDataPtr != IntPtr.Zero)
            {
                //Text from file example
                //string fileContent = Marshal.PtrToStringAnsi(fileDataPtr, fileSize);
                //fileContentText.text = fileContent;

                //Bytes from file example
                byte[] fileBytes = new byte[fileSize];
                Marshal.Copy(fileDataPtr, fileBytes, 0, fileSize);

                //Checksumming the file just because the original purpose for this plugin is to avoid more patchers and bullshit,
                //just add the files to the SD Card, both the install and the game and you're good to go
                string result = string.Empty;
                using (HashAlgorithm algorithm = new MD5CryptoServiceProvider())
                {
                    result = BitConverter.ToString(algorithm.ComputeHash(fileBytes)).ToLower().Replace("-", "");
                }

                fileContentText.text = result;
            }
            else
            {
                Debug.LogError("Failed to read file data or file is empty.");
                errors.text = "Failed to read file data or file is empty.";
            }
        }
        else
        {
            Debug.LogError("Failed to read file: " + filePath);
            errors.text = "Failed to read file: " + filePath;
        }

        Marshal.FreeHGlobal(pathConverted);
    }

    public void WriteTest()
    {
        WriteFile("test/folders/TestingTesting.txt", "Tester thing");
    }

    public void WriteFile(string filePath, string content)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is empty!");
            errors.text = "File path is empty!";
            return;
        }

        if (string.IsNullOrEmpty(content))
        {
            Debug.LogError("Content is empty!");
            errors.text = "Content is empty!";
            return;
        }

        byte[] fileBytes = Encoding.UTF8.GetBytes(content);
        IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

        if (WriteFile(pathConverted, fileBytes, fileBytes.Length))
        {
            Debug.Log("Successfully wrote file: " + filePath);
            errors.text = "Successfully wrote file: " + filePath;
        }
        else
        {
            Debug.LogError("Failed to write file: " + filePath);
            errors.text = "Failed to write file: " + filePath;
        }

        Marshal.FreeHGlobal(pathConverted);

        // Show any log messages
        //IntPtr logPtr = GetLogMessages();
        //string logMessages = Marshal.PtrToStringAnsi(logPtr);
        //Debug.Log(logMessages);
        //errors.text = logMessages;
    }
}*/