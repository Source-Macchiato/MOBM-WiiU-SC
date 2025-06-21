using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugDialog : MonoBehaviour
{
    [Header("Scenes")]
    public GameObject SceneMiniCG1;
    public GameObject SceneMiniCG2;
    public GameObject SceneMiniCG3;
    public GameObject SceneMiniCG4;
    public GameObject SceneMiniCG5_1; // blank
    public GameObject SceneMiniCG5_2;
    public GameObject SceneMiniCG5_3;

    [Header("Audio")]
    public AudioSource milk13;
    public AudioSource milk10;
    public GameObject milk16;
    public AudioSource milk16_audio;
    public AudioSource milk6;

    [Header("Voices")]
    public AudioSource narr;
    public AudioSource mum1;
    public AudioSource mum1_ext1;
    public AudioSource mum1_ext2;

    [Header("Dialog")]
    public TextMeshProUGUI MainDialog;
	public TextMeshProUGUI MiddleDialog;
    public NovEngine novEngine;

    [Header("Demo End Text")]
    public GameObject DemoEndText1;
    public GameObject DemoEndText2;
    public GameObject DemoEndText3;

	[Header("Shake effect")]
	public float Intensity = 3f;
	public float Speed = 1.4f;

    private int currentScene = 0;
    private Animator animator;

    // Flags to avoid multiple calls
    private bool miniCG1Started = false;
    private bool sceneTransitionInProgress = false;

    void Start()
    {
        StartCoroutine(ChoiceTest());
    }

    void Update()
    {
        if (NovEngine.Ended && !sceneTransitionInProgress)
        {
            sceneTransitionInProgress = true; // avoid multiple calls
            MainDialog.text = "";
            switch(currentScene)
            {
                case 1:
                    Debug.Log("end debug");
                    break;
            }
        }
        Debug.Log(NovEngine.SelectedChoice);
    }

    private IEnumerator MiniCG1()
    {
        currentScene = 1;
        NovEngine.ClearQueue();
        /*NovEngine.TOGVShake(MiddleDialog, "- SAY IT: I'LL NEVER DRINK MILK EVER AGAIN.", mum1, 4.3f, 1.45f, 0.04f);
		NovEngine.TOGV(MiddleDialog, "- I...", narr);
		NovEngine.TOGVShake(MiddleDialog, "- SAY IT.", mum1, 7f, 1.45f, 0.07f);
		NovEngine.TOGV(MiddleDialog, "- I'll never... drink milk... ever again...", narr);
		NovEngine.TOGVShake(MiddleDialog, "- SAY IT AGAIN.", mum1, 8f, 1.45f, 0.09f);
		NovEngine.TOGV(MiddleDialog, "- I'll never drink milk ever again!", narr);
		NovEngine.TOGV(MiddleDialog, "- I'LL NEVER DRINK MILK EVER AGAIN!",narr, 70f); */


        NovEngine.TOG(MainDialog, "Text 1 test");
        NovEngine.TOG(MainDialog, "This is a pretty long text, even though it's not that long, but it's still long enough to be considered long. I hope you're ready for this, because it's going to be a long ride.");
        NovEngine.TOGVShake(MainDialog, "This is a shaky text with custon intensity and speed, and a voice", narr, Intensity, Speed);
		NovEngine.TOGV(MainDialog, "this is a simple text with a voice ", narr);
        NovEngine.TOGV(MiddleDialog, "This text is at the center of the screen and has a voice", narr);
		NovEngine.TOGVShake(MiddleDialog, "This text is at the center of the screen and it's shaking", mum1, 4.3f, 1.45f);
        NovEngine.TOGVShake(MiddleDialog, "Same, but the text on it is with a custom speed", mum1, 4.3f, 1.45f, 0.1f);
        NovEngine.End(); // end of part 1

        // Reset the transition flag to allow moving to the next part
        sceneTransitionInProgress = false;
		yield break;
    }

    private IEnumerator ChoiceTest()
    {
        currentScene = 2;
        NovEngine.ClearQueue();
        NovEngine.TOG(MainDialog, "This is a choice test");
        NovEngine.TOG(MainDialog, "This is a choice test with a second text, next will be a choice");
        NovEngine.TOGWithChoices(MainDialog, "Skibidi toilet !!!!", new List<string> { "Choice1", "Choice2", "Choice3" });
        NovEngine.End();
        yield break;
    }



    // Function called at each new dialog: clear the queue and reset the Ended flag
    private void NewDialog()
    {
        NovEngine.ClearQueue();
        NovEngine.Ended = false;
    }
}
