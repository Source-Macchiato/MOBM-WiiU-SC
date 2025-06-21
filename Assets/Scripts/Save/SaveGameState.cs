﻿using UnityEngine;
using System.IO;
using WiiU = UnityEngine.WiiU;
using System.Threading;

public class SaveGameState : MonoBehaviour
{
    public bool DoSave()
    {
        bool saved = false;
        Thread t = new Thread(new ThreadStart(
            delegate
            {
                saved = Save();
            })
        );

        t.Start();

        return saved;
    }

    bool Save()
    {
        WiiU.SaveCommand cmd = WiiU.Save.SaveCommand(WiiU.Save.accountNo);

        long freespace = 0;
        WiiU.Save.FSStatus status = cmd.GetFreeSpaceSize(out freespace, WiiU.Save.FSRetFlag.None);
        if (status != WiiU.Save.FSStatus.OK)
            return false;

        long needspace = Mathf.Max(1024 * 1024, WiiU.PlayerPrefsHelper.rawData.Length);

        if (freespace < needspace)
        {
            // not enough free space
            return false;
        }
        else
        {
            var path = Application.persistentDataPath + "/save.bin";
            var fileStream = new FileStream(path, FileMode.Create);
            byte[] prefsData = WiiU.PlayerPrefsHelper.rawData;
            fileStream.Write(prefsData, 0, prefsData.Length);
            fileStream.Close();

            // It is very important to flush quota, otherwise filesystem changes will be discarded upon reboot
            status = cmd.FlushQuota(WiiU.Save.FSRetFlag.None);
            if (status != WiiU.Save.FSStatus.OK)
                return false;
        }

        return true;
    }
}