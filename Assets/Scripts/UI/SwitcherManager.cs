using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitcherManager : MonoBehaviour
{
	public int currentOptionId;
	public string[] optionsName;
	public TextMeshProUGUI text;

	public Button centerButton;

	void Start()
	{
		UpdateText();
    }

	public void ChangeOption()
	{
		if (currentOptionId < optionsName.Length - 1)
		{
            currentOptionId++;
        }
		else
		{
			currentOptionId = 0;
		}

		UpdateText();
	}

	public void UpdateText()
	{
        text.text = optionsName[currentOptionId];
    }

	public void LeftOption()
	{
		centerButton.Select();
        centerButton.GetComponent<ButtonManager>().UpdateTextColor();

        if (currentOptionId > 0)
		{
			currentOptionId--;

            UpdateText();
        }
	}

	public void RightOption()
	{
		centerButton.Select();
        centerButton.GetComponent<ButtonManager>().UpdateTextColor();

        if (currentOptionId < optionsName.Length -1)
		{
			currentOptionId++;

			UpdateText();
		}
	}
}
