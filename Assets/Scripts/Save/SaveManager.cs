using UnityEngine;

public class SaveManager : MonoBehaviour
{
	// Save
	public void SaveLayoutId(int layoutId)
	{
		PlayerPrefs.SetInt("Layout", layoutId);
		PlayerPrefs.Save();
	}

	public void SaveLanguage(string language)
	{
		PlayerPrefs.SetString("Language", language.ToLower());
		PlayerPrefs.Save();
	}

	// Load
	public static int LoadLayoutId()
	{
		if (PlayerPrefs.HasKey("Layout"))
		{
			int layoutId = PlayerPrefs.GetInt("Layout");

			return layoutId;
		}
		else
		{
			return 1;
		}
	}

	public static string LoadLanguage()
	{
		if (PlayerPrefs.HasKey("Language"))
		{
			string language = PlayerPrefs.GetString("Language");

			return language;
		}
		else
		{
			return null;
		}
	}
}