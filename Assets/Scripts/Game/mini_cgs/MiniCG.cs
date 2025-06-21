using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniCG : MonoBehaviour
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
    public AudioSource trans1;

    [Header("Voices")]
    public AudioSource narr;
    public AudioSource mum1;
    public AudioSource mum1_ext1;
    public AudioSource mum1_ext2;

    [Header("Dialog")]
    public TextMeshProUGUI MainDialog;
	public TextMeshProUGUI MiddleDialog;

    [Header("Scripts")]
     public NovEngine novEngine;
     public Shared shared;

    [Header("Dissolve effect")]
    public GameObject DissolveIntro;
    public GameObject DissolveTrans;

	[Header("Shake effect")]
	public float Intensity = 3f;
	public float Speed = 1.4f;

    [Header("Canvas")]
    public GameObject CanvasGamePad;

    private int currentScene = 0;
    private Animator animator;
    
    // Flags
    private bool miniCG1Started = false;
    private bool sceneTransitionInProgress = false;

    void Start()
    {   
        StartCoroutine(DissolveEffectIntro());
        CanvasGamePad.SetActive(true);
        if (SceneMiniCG1 != null)
        {
            animator = SceneMiniCG1.GetComponent<Animator>();
        }
    }

    void Update()
    {
        Intro();
        if (NovEngine.Ended && !sceneTransitionInProgress)
        {
            sceneTransitionInProgress = true; 
            MainDialog.text = "";
            switch(currentScene)
            {
                case 1:
                    SceneMiniCG2.SetActive(true);
                    StartCoroutine(MiniCG2());
                    break;
                case 2:
                    SceneMiniCG3.SetActive(true);
                    StartCoroutine(MiniCG3());
                    break;
                case 3:
                    SceneMiniCG4.SetActive(true);
                    StartCoroutine(MiniCG4());
                    break;
                case 4:
                    SceneMiniCG4.SetActive(false);
                    SceneMiniCG5_1.SetActive(true);
                    StartCoroutine(MiniCG5_1());
                    break;
                case 5:
                    StartCoroutine(MiniCG5_2());
                    break;
                case 6:
                    SceneMiniCG5_3.SetActive(true);
                    StartCoroutine(MiniCG5_3());
                    break;
                case 7:
                    StartCoroutine(MiniCG5_4());
                    break;
                case 8:
                    StartCoroutine(MiniCG5_5());
                    break;
				case 9:
					SceneMiniCG5_1.SetActive(true);
					SceneMiniCG5_3.SetActive(false);
					StartCoroutine(MiniCG5_6());
					break;
                case 10:
                    StartCoroutine(outro());
                    break;
                default:
                    // Nothing to do
                    break;
            }
        }
    }

    private void Intro()
    {
        
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("mini_cg_1_shake"))
        {
            if (!milk13.isPlaying)
            {
                milk13.Play();
            }
            // Start MiniCG1 only once
            if (!miniCG1Started)
            {
                miniCG1Started = true;
                StartCoroutine(MiniCG1());
            }
        }
        
    }
     private IEnumerator DissolveEffectIntro()
     {
        yield return new WaitForSeconds(3f);
        StartCoroutine(Shared.DissolveFade(DissolveIntro, 0f, 1f, 1f));
     }
    private IEnumerator MiniCG1()
    {
        yield return new WaitForSeconds(4f);
        currentScene = 1;
        NovEngine.ClearQueue();
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.1"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.2"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.3"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.4"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.5"));
        NovEngine.End(); // fin de la partie 1

        // Reset the transition flag to allow moving to the next part
        sceneTransitionInProgress = false;
    }

    private IEnumerator MiniCG2()
    {
        Shared.StartTransition(SceneMiniCG1, DissolveTrans, 1f, 2f, 1.5f);
        yield return new WaitForSeconds(7f);
        currentScene = 2;
        NewDialog();
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.6"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.7"));
        NovEngine.End(); // end of part 2

        sceneTransitionInProgress = false;
    }

    private IEnumerator MiniCG3()
    {
        Shared.StartTransition(SceneMiniCG2, DissolveTrans, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(5f);
        currentScene = 3;
        NewDialog();
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.8"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.9"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.10"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.11"));
        NovEngine.End();

        sceneTransitionInProgress = false;
    }

    private IEnumerator MiniCG4()
    {
        Shared.StartTransition(SceneMiniCG3, DissolveTrans, 1f, 1f, 0f);
        yield return new WaitForSeconds(4f);
        currentScene = 4;
        NewDialog();

        // stop milk13 if it's playing
        if (milk13.isPlaying)
        {
            milk13.Stop();
        }
        // Play milk10 if it's not playing
        if (!milk10.isPlaying)
        {
            milk10.Play();
        }
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.12"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.13"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.14"));
        NovEngine.End();

        sceneTransitionInProgress = false;
    }

	// ------------------------Mom part------------------------
    private IEnumerator MiniCG5_1()
    {
        StartCoroutine(Shared.FadeOut(milk10, 1f));
        yield return new WaitForSeconds(1.5f);
        NewDialog();
        currentScene = 5;
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.15"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.16"));
        NovEngine.End(); // end of part 5

        sceneTransitionInProgress = false;
    }

    private IEnumerator MiniCG5_2()
    {
        yield return new WaitForSeconds(3f);
        SceneMiniCG5_1.SetActive(false);
        SceneMiniCG5_2.SetActive(true);
        yield return new WaitForSeconds(3f);
        milk16.SetActive(true); // Play the music
        currentScene = 6;
        NewDialog();
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.17"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.18"));
        NovEngine.End(); // end of part 6

        sceneTransitionInProgress = false;
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator MiniCG5_3()
    {
        Shared.StartTransition(SceneMiniCG5_2, DissolveTrans, 1f, 2f, 0.5f);
        yield return new WaitForSeconds(4f);
        currentScene = 7;
        NewDialog();
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.19"));
        NovEngine.TOGV(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.20"), narr);
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.21"));
        NovEngine.TOGVShake(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.22"), mum1, Intensity, Speed);
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.23"));
        NovEngine.TOGV(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.24"), narr);
        NovEngine.TOGVShake(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.25"), mum1, Intensity, Speed);
        NovEngine.End(); // end of part 7

        sceneTransitionInProgress = false;
    }

    private IEnumerator MiniCG5_4()
    {
        currentScene = 8;
        Animator anim = SceneMiniCG5_3.GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("minicg5_3_3");
        }
        NewDialog();
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.26"));
        NovEngine.End(); // end of part 8

        sceneTransitionInProgress = false;
        yield break;
    }

    private IEnumerator MiniCG5_5()
    {
        currentScene = 9;
        Animator anim = SceneMiniCG5_3.GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("minicg5_3_4");
        }
        NewDialog();
        NovEngine.TOGV(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.27"), narr);
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.28"));
        NovEngine.TOGV(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.29"), narr);
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.30"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.31"));
        NovEngine.TOGV(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.32"), narr);
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.33"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.34"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.35"));
        NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.36"));
        NovEngine.End(); // end of part 9
		yield return new WaitForSeconds(3f);

        sceneTransitionInProgress = false;
        yield break;
    }


	private IEnumerator MiniCG5_6()
	{
		yield return new WaitForSeconds(3f);
		currentScene = 10;
		NewDialog();
		NovEngine.TOGVShake(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.37"), mum1, 4.3f, 1.45f, 0.04f);
		NovEngine.TOGV(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.38"), narr);
		NovEngine.TOGVShake(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.39"), mum1, 7f, 1.45f, 0.07f);
		NovEngine.TOGV(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.40"), narr);
		NovEngine.TOGVShake(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.41"), mum1, 8f, 1.45f, 0.09f);
		NovEngine.TOGV(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.42"), narr);
		NovEngine.TOGV(MiddleDialog, I18nTextTranslator.GetTranslatedText("dialog.minicg.43"),narr, 70f);
		NovEngine.End();
		sceneTransitionInProgress = false;
	}

    private IEnumerator outro()
    {
        MiddleDialog.text = "";
        StartCoroutine(Shared.FadeOut(milk16_audio, 5f));
        trans1.Play();

        yield return new WaitForSeconds(8f);

        StartCoroutine(Shared.FadeOut(trans1, 3f));

        yield return new WaitForSeconds(4f);

        LoadingManager.LoadScene("cg_mirror");
    }



    // clear dialog queue at each new dialog
    private void NewDialog()
    {
        NovEngine.ClearQueue();
        NovEngine.Ended = false;
    }

	/*

	private IEnumerator ModifyThis()
	{
		yield return new WaitForSeconds(2f);
		currentScene = 0;
		NewDialog();

		NovEngine.End();
		sceneTransitionInProgress = false;
	}

	*/
}
