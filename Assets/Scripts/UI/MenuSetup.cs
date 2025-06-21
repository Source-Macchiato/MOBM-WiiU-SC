using UnityEngine;

public class MenuSetup : MonoBehaviour
{
	MenuManager menuManager;
	MenuData menuData;

	void Start()
	{
		menuManager = FindObjectOfType<MenuManager>();
		menuData = FindObjectOfType<MenuData>();

		menuManager.SetBackCallback(2, OnBackFromLayout);
		menuManager.SetBackCallback(3, OnBackFromLanguage);
		menuManager.SetBackCallback(4, OnBackFromCredits);
	}

	public void NewGame()
	{
		if (menuManager.canNavigate)
		{
            StartCoroutine(menuData.CreateNewGame());
        }
	}

	public void Layout()
	{
		if (menuManager.canNavigate)
		{
			menuManager.ChangeMenu(2);

			menuData.LoadLayoutId();
		}
	}

	public void Language()
	{
		if (menuManager.canNavigate)
		{
			menuManager.ChangeMenu(3);

			menuData.LoadLanguage();
		}
	}

	public void Credits()
	{
		if (menuManager.canNavigate)
		{
			menuManager.ChangeMenu(4);

			menuData.ToggleTopInfos(false);
		}
	}

	public void Quit()
	{
		if (menuManager.canNavigate)
		{
			menuData.BackToGameSelector();
		}
	}

	// Back callbacks
	private void OnBackFromLayout()
	{
		menuData.SaveLayoutId();
	}

	private void OnBackFromLanguage()
	{
		menuData.SaveLanguage();
	}

	private void OnBackFromCredits()
	{
		menuData.ToggleTopInfos(true);
	}
}