// hey it's ShiroSATA
// Yeah  i know this is a mess, it was the very early version of NovEngine

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WiiU = UnityEngine.WiiU;
using TMPro;

public class CGMirror : MonoBehaviour {

	[Header("Scenes")]
	public GameObject cg_mirror;
	public GameObject cg_pills;

	[Header("Audio")]
	public AudioSource milk6;

	[Header("Dialog")]

	public GameObject TransDialogBox;
	public GameObject DialogBox;
	public TextMeshProUGUI MainDialog;

	[Header("Buttons")]
	public GameObject heyButton;

	[Header("Scripts")]
	public NovEngine novEngine;
	public Shared shared;

	[Header("Dissolve effect")]
    public GameObject DissolveTrans;

	private int currentScene = 0;
	private bool sceneTransitionInProgress = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Preset fade
        StartCoroutine(Shared.SetFade(DissolveTrans, 0f));

		//start CGMirror intro
		StartCoroutine(Intro());

		heyButton.SetActive(false);
	}

	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
			if (NovEngine.isChoiceActive)
			{
                if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
                {
                    heyButton.GetComponent<ButtonManager>().UpdateTextColor();
                }
				else if (gamePadState.IsReleased(WiiU.GamePadButton.A))
				{
                    heyButton.GetComponent<Button>().onClick.Invoke();
                }
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (NovEngine.isChoiceActive)
				{
                    if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
                    {
                        heyButton.GetComponent<ButtonManager>().UpdateTextColor();

                    }
					else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.A))
					{
                        heyButton.GetComponent<Button>().onClick.Invoke();
                    }
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (NovEngine.isChoiceActive)
				{
                    if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
                    {
                        heyButton.GetComponent<ButtonManager>().UpdateTextColor();

                    }
					else if (remoteState.classic.IsReleased(WiiU.ClassicButton.A))
					{
                        heyButton.GetComponent<Button>().onClick.Invoke();
                    }
                }
                break;
            default:
                if (NovEngine.isChoiceActive)
				{
                    if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                    {
                        heyButton.GetComponent<ButtonManager>().UpdateTextColor();

                    }
					else if (remoteState.IsReleased(WiiU.RemoteButton.A))
					{
                        heyButton.GetComponent<Button>().onClick.Invoke();
                    }
                }
                break;
        }

        if (Application.isEditor)
        {
			if (NovEngine.isChoiceActive)
			{
                if (Input.GetKeyDown(KeyCode.Space))
                {
					heyButton.GetComponent<ButtonManager>().UpdateTextColor();
                }
				else if (Input.GetKeyUp(KeyCode.Space))
				{
                    heyButton.GetComponent<Button>().onClick.Invoke();
                }
            }
        }

        if (NovEngine.Ended && !sceneTransitionInProgress)
        {
            sceneTransitionInProgress = true;
            MainDialog.text = "";
            switch(currentScene)
            {
				case 1:
				cg_pills.SetActive(true);
				StartCoroutine(SceneCGPills());
				break;
				case 2:
					StartCoroutine(EndScene());
					break;
                
                default:
                    // Rien à foutre de ta depression
                    break;
            }
        }
	}

	private IEnumerator Intro()
	{
		yield return new WaitForSeconds(2f);
		StartCoroutine(Shared.DissolveFade(DissolveTrans, 0f, 1f, 1f));
		yield return new WaitForSeconds(2f);
		StartCoroutine(SceneCGMirror());

	}

	private IEnumerator SceneCGMirror()
	{
		yield return new WaitForSeconds(2f);
		StartCoroutine(Shared.DissolveFade(TransDialogBox, 1f, 0f, 1f));
		yield return new WaitForSeconds(1f);
		DialogBox.SetActive(true);
		yield return new WaitForSeconds(1f);
		currentScene = 1;
		NewDialog();

		// This also can be optimized with a for loop tbh, ig i was just dumb back then
		NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.cgmirror.1"));
		NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.cgmirror.2"));
		NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.cgmirror.3"));
		NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.cgmirror.4"));
		NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.cgmirror.5"));
		NovEngine.TOG(MainDialog, I18nTextTranslator.GetTranslatedText("dialog.cgmirror.6"));


		NovEngine.End();
		sceneTransitionInProgress = false;
		yield break;
	} 

	private IEnumerator SceneCGPills()
	{
		Shared.StartTransition(cg_mirror, DissolveTrans, 1f, 1f, 1.5f);
		yield return new WaitForSeconds(2f);
		DialogBox.SetActive(false);
		yield return new WaitForSeconds(1.5f);
		DialogBox.SetActive(true);
		yield return new WaitForSeconds(1f);
		currentScene = 2;
		NewDialog();
		// this also can be optimized with a for loop
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.1"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.2"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.3"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.4"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.5"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.6"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.7"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.8"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.9"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.10"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.11"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.12"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.13"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.14"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.15"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.16"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.17"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.18"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.19"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.20"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.21"));
		NovEngine.TOG(MainDialog,I18nTextTranslator.GetTranslatedText("dialog.cgpills.22"));
		NovEngine.End();
		sceneTransitionInProgress = false;
	} 

	private IEnumerator EndScene()
	{
		StartCoroutine(Shared.DissolveFade(DissolveTrans, 1f, 0f, 1f));
		yield return new WaitForSeconds(2f);

		DialogBox.SetActive(false);

		StartCoroutine(Shared.FadeOut(milk6, 2f));

		yield return new WaitForSeconds(3f);

		NovEngine.isChoiceActive = true;
		heyButton.SetActive(true);
	}

	public void OnHeyButtonClick()
	{
		StartCoroutine(StartEndDemo());
	}

	public IEnumerator StartEndDemo()
	{
		heyButton.SetActive(false);

		yield return new WaitForSeconds(2f);

		NovEngine.isChoiceActive = false;

		LoadingManager.LoadScene("EndDemo");
	}
	private void NewDialog()
    {
        NovEngine.ClearQueue();
        NovEngine.Ended = false;
    }
}
