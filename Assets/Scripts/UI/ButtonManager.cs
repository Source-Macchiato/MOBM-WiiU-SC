using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonManager : MonoBehaviour, IPointerDownHandler
{
    public bool disabled;
    public int game;

	public TextMeshProUGUI text;

	void Start()
	{
        if (disabled)
        {
            GetComponent<Button>().interactable = false;

            SetDisabledColor();
        }
        else
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                SetSelectedColor();
            }
            else
            {
                SetDefaultColor();
            }
        }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateTextColor();
    }

    public void UpdateTextColor()
    {
        if (!disabled)
        {
            ButtonManager[] buttonManagers = FindObjectsOfType<ButtonManager>();

            foreach (ButtonManager buttonManager in buttonManagers)
            {
                if (!buttonManager.disabled)
                {
                    buttonManager.SetDefaultColor();
                }
            }

            SetSelectedColor();
        }
    }

    private void SetDefaultColor()
	{
        if (text != null)
        {
            if (game == 1)
            {
                text.color = new Color(93 / 255f, 1 / 255f, 32 / 255f);
            }
            else if (game == 2)
            {
                text.color = new Color(82 / 255f, 38 / 255f, 62 / 255f);
            }
        }
    }

	private void SetSelectedColor()
	{
        if (text != null)
        {
            if (game == 1)
            {
                text.color = new Color(125 / 255f, 18 / 255f, 141 / 255f);
            }
            else if (game == 2)
            {
                text.color = new Color(173 / 255f, 50 / 255f, 49 / 255f);
            }
        }
    }

    private void SetDisabledColor()
    {
        if (text != null)
        {
            if (game == 1 || game == 2)
            {
                text.color = new Color(45 / 255f, 39 / 255f, 43 / 255f);
            }
        }
    }
}
