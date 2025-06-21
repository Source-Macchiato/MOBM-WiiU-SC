using UnityEngine;

public class LayoutManager : MonoBehaviour
{
	public GameObject[] screenGame;
    public GameObject[] screenChoice;
	public GameObject[] screenLookAt;

    public I18nTextTranslator lookAtText;

	private int layoutId = 1; // 0 = TV, 1 = TV + GamePad, 2 = GamePad

	void Start()
	{
		layoutId = SaveManager.LoadLayoutId();

		if (layoutId == 0)
		{
            screenGame[0].SetActive(true);
            screenGame[1].SetActive(false);

            if (screenChoice[0] != null && screenChoice[1] != null)
            {
                screenChoice[0].SetActive(true);
                screenChoice[1].SetActive(false);
            }

            lookAtText.textId = "menu.look.tv";
            lookAtText.UpdateText();

            screenLookAt[0].SetActive(false);
            screenLookAt[1].SetActive(true);
        }
		else if (layoutId == 1)
		{
            screenGame[0].SetActive(true);
            screenGame[1].SetActive(false);

            if (screenChoice[0] != null && screenChoice[1] != null)
            {
                screenChoice[0].SetActive(false);
                screenChoice[1].SetActive(true);
            }

            lookAtText.textId = "menu.look.tv";
            lookAtText.UpdateText();

            screenLookAt[0].SetActive(false);
            screenLookAt[1].SetActive(true);
        }
		else if (layoutId == 2)
		{
            screenGame[0].SetActive(false);
            screenGame[1].SetActive(true);

            if (screenChoice[0] != null && screenChoice[1] != null)
            {
                screenChoice[0].SetActive(false);
                screenChoice[1].SetActive(true);
            }

            lookAtText.textId = "menu.look.gamepad";
            lookAtText.UpdateText();

            screenLookAt[0].SetActive(true);
            screenLookAt[1].SetActive(false);
        }
	}

    void Update()
    {
        if (NovEngine.isChoiceActive)
        {
            if (layoutId == 1)
            {
                lookAtText.textId = "menu.look.gamepad";
                lookAtText.UpdateText();

                screenLookAt[0].SetActive(true);
                screenLookAt[1].SetActive(false);
            }
        }
        else
        {
            if (layoutId == 1)
            {
                lookAtText.textId = "menu.look.tv";
                lookAtText.UpdateText();

                screenLookAt[0].SetActive(false);
                screenLookAt[1].SetActive(true);
            }
        }
    }
}
