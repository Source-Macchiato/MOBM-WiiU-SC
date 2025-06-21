// This is the very first version of the NovEngine (1.0).
// It is a simple dialogue engine for Unity, designed to handle text and voice dialogues with choices (even though choices are not implemented yet in this version).

// The most recent version of NovEngine is the 3.4.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WiiU = UnityEngine.WiiU;

public abstract class DialogueTask
{
    public abstract IEnumerator Execute();
}

public class TextTask : DialogueTask
{
    protected TextMeshProUGUI textUI;
    private string message;
    private float characterDelay;

    public TextTask(TextMeshProUGUI textUI, string message, float characterDelay)
    {
        this.textUI = textUI;
        this.message = message;
        this.characterDelay = characterDelay;
    }

    public override IEnumerator Execute()
    {
        NovEngine.Instance.skipDialogs = false;
        textUI.text = "";
        textUI.ForceMeshUpdate();

        foreach (char c in message)
        {
            textUI.text += c;
            float elapsed = 0f;
            
            while (elapsed < characterDelay && !NovEngine.Instance.skipDialogs)
            {
                elapsed += Time.deltaTime;

                yield return null;
            }
        }

        NovEngine.Instance.skipDialogs = false;

        textUI.text = message;

        while (!NovEngine.Instance.skipDialogs)
        {
            yield return null;
        }

        NovEngine.Instance.skipDialogs = false;

        yield break;
    }
}

public class VoiceTextTask : DialogueTask
{
    protected TextMeshProUGUI textUI;
    private string message;
    private float characterDelay;
    private AudioSource audioSource;
    private float? customFontSize;

    public VoiceTextTask(TextMeshProUGUI textUI, string message, float characterDelay, AudioSource audioSource, float? customFontSize = null)
    {
        this.textUI = textUI;
        this.message = message;
        this.characterDelay = characterDelay;
        this.audioSource = audioSource;
        this.customFontSize = customFontSize;
    }

    public override IEnumerator Execute()
    {
        // Save original size
        float originalFontSize = textUI.fontSize;

        // Apply custom size if provided
        if (customFontSize.HasValue)
            textUI.fontSize = customFontSize.Value;

        NovEngine.Instance.skipDialogs = false;
        textUI.text = "";
        textUI.ForceMeshUpdate();

        // Start looping audio playback
        audioSource.loop = true;
        audioSource.Play();

        foreach (char c in message)
        {
            textUI.text += c;
            float elapsed = 0f;
            while (elapsed < characterDelay && !NovEngine.Instance.skipDialogs)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        // Stop audio when text is displayed
        audioSource.Stop();

        NovEngine.Instance.skipDialogs = false;
        textUI.text = message;
        while (!NovEngine.Instance.skipDialogs)
        {
            yield return null;
        }
        NovEngine.Instance.skipDialogs = false;

        // Restore original size
        textUI.fontSize = originalFontSize;
    }
}



public class ShakyTextTask : TextTask
{
    private float intensity;
    private float speed;

    public ShakyTextTask(TextMeshProUGUI textUI, string message, float characterDelay, float intensity, float speed) 
        : base(textUI, message, characterDelay)
    {
        this.intensity = intensity;
        this.speed = speed;
    }

    public override IEnumerator Execute()
    {
        var shakeEffect = textUI.GetComponent<ShakeTextEffect>();
        if (shakeEffect == null)
            shakeEffect = textUI.gameObject.AddComponent<ShakeTextEffect>();

        shakeEffect.enabled = true;
        shakeEffect.intensity = intensity;
        shakeEffect.speed = speed;

        yield return base.Execute();

        shakeEffect.enabled = false;
        textUI.SetVerticesDirty();
    }
}


public class ShakyVoiceTextTask : VoiceTextTask
{
    private float intensity;
    private float speed;

    public ShakyVoiceTextTask(TextMeshProUGUI textUI, string message, float characterDelay, 
                                AudioSource audioSource, float intensity, float speed) 
        : base(textUI, message, characterDelay, audioSource)
    {
        this.intensity = intensity;
        this.speed = speed;
    }

    public override IEnumerator Execute()
    {
        var shakeEffect = textUI.GetComponent<ShakeTextEffect>();
        if (shakeEffect == null)
            shakeEffect = textUI.gameObject.AddComponent<ShakeTextEffect>();

        shakeEffect.enabled = true;
        shakeEffect.intensity = intensity;
        shakeEffect.speed = speed;

        yield return base.Execute();

        shakeEffect.enabled = false;
        textUI.SetVerticesDirty();
    }
}



public class ChoiceTask : DialogueTask
{
    protected TextMeshProUGUI textUI;
    private string message;
    private List<string> choices;
    private float characterDelay;

    public ChoiceTask(TextMeshProUGUI textUI, string message, List<string> choices, float characterDelay)
    {
        this.textUI = textUI;
        this.message = message;
        this.choices = choices;
        this.characterDelay = characterDelay;
    }

    public override IEnumerator Execute()
    {
        yield return NovEngine.Instance.StartCoroutine(DisplayText());
        yield return NovEngine.Instance.StartCoroutine(ShowChoices());
    }

    private IEnumerator DisplayText()
    {
        textUI.text = "";
        textUI.ForceMeshUpdate();

        foreach (char c in message)
        {
            textUI.text += c;
            float elapsed = 0f;
            while (elapsed < characterDelay && !NovEngine.Instance.skipDialogs)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        NovEngine.Instance.skipDialogs = false;
        textUI.text = message;
        while (!NovEngine.Instance.skipDialogs)
        {
            yield return null;
        }
        NovEngine.Instance.skipDialogs = false;
    }

    private IEnumerator ShowChoices()
    {
        // Reset the selected choice to -1 (no choice selected)
        NovEngine.SelectedChoice = -1;

        // Get the list of choice buttons from the NovEngine instance
        List<GameObject> buttons = NovEngine.Instance.choiceButtons;

        // Loop through the available choices
        for (int i = 0; i < choices.Count; i++)
        {
            // Ensure there are enough buttons to display the choices
            if (i < buttons.Count)
            {
                // Activate the button and set its text to the corresponding choice
                buttons[i].SetActive(true);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i];

                // Capture the current index for the button's onClick listener
                int index = i;

                // Add a listener to the button to handle the choice selection
                buttons[i].GetComponent<Button>().onClick.AddListener(() => Choose(index));
            }
        }

        // Wait until the player selects a choice
        while (NovEngine.SelectedChoice == -1)
            yield return null;

        // Deactivate all buttons after a choice is made
        foreach (var btn in buttons)
            btn.SetActive(false);
    }

    private void Choose(int index)
    {
        NovEngine.SelectedChoice = index;
    }
}


public class EndTask : DialogueTask
{
    public override IEnumerator Execute()
    {
        NovEngine.Ended = true;
        yield break;
    }
}

public class NovEngine : MonoBehaviour
{
    // Singleton Instance
    public static NovEngine Instance { get; private set; }

    // Configuration
    [Header("Configuration")]
    public float characterDelay = 0.05f; // Default delay between characters
    public List<GameObject> choiceButtons; // List of UI buttons for choices

    // Internal State
    private Queue<DialogueTask> taskQueue = new Queue<DialogueTask>(); // Queue to hold dialogue tasks
    private Coroutine processingCoroutine; // Reference to the coroutine processing tasks

    // Static Flags
    public static bool Ended { get; set; } // Flag to indicate if the dialogue has ended
    public static int SelectedChoice { get; set; } // Index of the selected choice

    public bool skipDialogs = false;
    private bool touchscreenPressed = false;
    public static bool isChoiceActive = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Initialization
    void Awake()
    {
        // Ensure there is only one instance of NovEngine (Singleton pattern)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this; // Set the static instance to this object
        processingCoroutine = StartCoroutine(ProcessTasks()); // Start processing dialogue tasks

        Ended = false; // Initialize the dialogue ended flag
        SelectedChoice = -1; // Initialize the selected choice index

        // Deactivate all choice buttons at the start
        foreach (var btn in choiceButtons)
            btn.SetActive(false);
    }

    void Start()
    {
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }

    void Update()
    {
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        skipDialogs = false;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
            {
                skipDialogs = true;
            }
            else if (gamePadState.touch.touch == 1)
            {
                if (!touchscreenPressed)
                {
                    skipDialogs = true;
                    touchscreenPressed = true;
                }
            }
            else if (gamePadState.touch.touch == 0)
            {
                if (touchscreenPressed)
                {
                    touchscreenPressed = false;
                }
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
                {
                    skipDialogs = true;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
                {
                    skipDialogs = true;
                }
                break;
            default:
                if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                {
                    skipDialogs = true;
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                skipDialogs = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                skipDialogs = true;
            }
        }
    }

    // Task Processing
    private IEnumerator ProcessTasks()
    {
        while (true)
        {
            // Check if there are tasks in the queue and the dialogue hasn't ended
            if (taskQueue.Count > 0 && !Ended)
            {
                // Dequeue the next task and execute it
                var task = taskQueue.Dequeue();
                yield return StartCoroutine(task.Execute());
            }
            else
            {
                // Wait for the next frame if no tasks are available
                yield return null;
            }
        }
    }

    // Public API for Enqueuing Tasks
    public static void TOG(TextMeshProUGUI textUI, string message)
    {
        Instance.taskQueue.Enqueue(new TextTask(textUI, message, Instance.characterDelay));
    }

    public static void TOGWithChoices(TextMeshProUGUI textUI, string message, List<string> choices)
    {
        Instance.taskQueue.Enqueue(new ChoiceTask(textUI, message, choices, Instance.characterDelay));
    }

    public static void TOGV(TextMeshProUGUI textUI, string message, AudioSource audioSource, float fontSize = -1)
    {
        if (fontSize > 0)
            Instance.taskQueue.Enqueue(new VoiceTextTask(textUI, message, Instance.characterDelay, audioSource, fontSize));
        else
            Instance.taskQueue.Enqueue(new VoiceTextTask(textUI, message, Instance.characterDelay, audioSource));
    }



    public static void TOGVShake(TextMeshProUGUI textUI, string message, AudioSource audioSource, float intensity, float speed, float? customDelay = null)
    {
        float delay = customDelay.HasValue ? customDelay.Value : Instance.characterDelay;
        Instance.taskQueue.Enqueue(new ShakyVoiceTextTask(textUI, message, delay, audioSource, intensity, speed));
    }


    public static void TOGVShake(TextMeshProUGUI textUI, string message, AudioSource audioSource, float intensity, float speed)
    {
        Instance.taskQueue.Enqueue(new ShakyVoiceTextTask(textUI, message, Instance.characterDelay, audioSource, intensity, speed));
    }

    public static void End()
    {
        Instance.taskQueue.Enqueue(new EndTask());
    }

    // Utility Methods
    public static void ClearQueue()
    {
        if (Ended)
        {
            Ended = false; // Reset the dialogue ended flag
            Instance.taskQueue.Clear(); // Clear all tasks in the queue
            Instance.StopAllCoroutines(); // Stop all running coroutines
            Instance.processingCoroutine = Instance.StartCoroutine(Instance.ProcessTasks()); // Restart the processing coroutine
        }
    }
}