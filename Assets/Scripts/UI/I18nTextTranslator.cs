using UnityEngine;
using TMPro;

public class I18nTextTranslator : MonoBehaviour
{
    public string textId;
    private TextMeshProUGUI text;
    private string currentLanguage;

	void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

    void Update()
    {
        if (textId != string.Empty)
        {
            if (currentLanguage != I18n.GetLanguage())
            {
                UpdateText();

                currentLanguage = I18n.GetLanguage();
            }
        }
    }

    public void UpdateText()
    {
        if (text != null)
        {
            text.text = GetTranslatedText(textId);
        }
    }

    public static string GetTranslatedText(string id)
    {
        string translatedText;

        if (I18n.Texts.TryGetValue(id, out translatedText))
        {
            return translatedText;
        }
        else
        {
            return id;
        }
    }
}