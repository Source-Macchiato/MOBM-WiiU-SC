using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuData : MonoBehaviour
{
	[Header("Create New Game")]	
	public GameObject fadeOut;
	public AudioSource music;

	[Header("Scripts")]
	public Shared shared;

	[Header("Switchers")]
	public SwitcherManager layoutSwitcher;
	public SwitcherManager languageSwitcher;

	[Header("Top infos")]
	public GameObject gameTitle;
	public GameObject gameVersion;
	public GameObject gameCredits;

	public I18nTextTranslator languageText;

    private MenuManager menuManager;
	private SaveGameState saveGameState;
	private SaveManager saveManager;

	void Start()
	{
		menuManager = FindObjectOfType<MenuManager>();
		saveGameState = FindObjectOfType<SaveGameState>();
		saveManager = FindObjectOfType<SaveManager>();
	}

	public void ToggleTopInfos(bool visibility)
	{
		gameTitle.SetActive(visibility);
		gameVersion.SetActive(visibility);
		gameCredits.SetActive(visibility);
	}

	public IEnumerator CreateNewGame()
	{
        fadeOut.SetActive(true);
        StartCoroutine(Shared.FadeOut(music, 0.5f));

        yield return new WaitForSeconds(8f);

		LoadingManager.LoadScene("mini_cg");
    }

	public void CanNavigate(bool canNavigate)
	{
		menuManager.canNavigate = canNavigate;
	}

	public void LoadLayoutId()
	{
		layoutSwitcher.currentOptionId = SaveManager.LoadLayoutId();
		layoutSwitcher.UpdateText();
	}

	public void SaveLayoutId()
	{
		saveManager.SaveLayoutId(layoutSwitcher.currentOptionId);
		bool saveResult = saveGameState.DoSave();
	}

	public void LoadLanguage()
	{
        string language = I18n.GetLanguage();

        // Find language index
        int languageIndex = System.Array.IndexOf(languageSwitcher.optionsName, language);

        if (languageIndex >= 0 && languageIndex < languageSwitcher.optionsName.Length)
        {
            languageSwitcher.currentOptionId = languageIndex;
        }

		ChangeDisplayedLanguage();
    }

	public void SaveLanguage()
	{
		saveManager.SaveLanguage(languageSwitcher.optionsName[languageSwitcher.currentOptionId]);
		bool saveResult = saveGameState.DoSave();

		// Refresh translations
		I18n.LoadLanguage();
	}

	public void BackToGameSelector()
	{
		SceneManager.LoadSceneAsync("GameSelector");
	}

	public void ChangeDisplayedLanguage()
	{
        switch (languageSwitcher.optionsName[languageSwitcher.currentOptionId])
        {
			case "de":
				languageText.textId = "menu.language.german";
				break;
            case "es":
                languageText.textId = "menu.language.spanish";
                break;
            case "fr":
                languageText.textId = "menu.language.french";
                break;
            case "it":
                languageText.textId = "menu.language.italian";
                break;
            case "pl":
                languageText.textId = "menu.language.polish";
                break;
            case "ru":
                languageText.textId = "menu.language.russian";
                break;
			default:
                languageText.textId = "menu.language.english";
                break;
        }

        languageText.UpdateText();
    }
}