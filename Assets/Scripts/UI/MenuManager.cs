using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WiiU = UnityEngine.WiiU;

public class PopupData
{
    public GameObject popupObject;
    public int actionType;
    public string popupId;
    public int optionId;

    public PopupData(GameObject popupObject, int actionType)
    {
        this.popupObject = popupObject;
        this.actionType = actionType;
    }
}

public class MenuManager : MonoBehaviour
{
    public ScrollRect currentScrollRect;
    public PopupData currentPopup;
    private Selectable lastSelected;
    private Coroutine autoScrollCoroutine;

    public GameObject[] menus;
    public Button[] defaultButtons;

    private Stack<int> menuHistory = new Stack<int>();

    // List to keep track of generated callbacks
    private Dictionary<int, UnityEngine.Events.UnityAction> backCallbacks = new Dictionary<int, UnityEngine.Events.UnityAction>();

    private bool isNavigatingBack = false;

    [HideInInspector]
    public bool canNavigate = true;

    [HideInInspector]
    public int currentMenuId = 0;

    // Stick navigation
    private float stickNavigationCooldown = 0.2f;
    private float lastNavigationTime;
    private float stickDeadzone = 0.19f;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        ChangeMenu(0);
    }
	
	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            // Stick
            Vector2 leftStickGamepad = gamePadState.lStick;

            if (Mathf.Abs(leftStickGamepad.y) > stickDeadzone)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.y > stickDeadzone)
                            {
                                MenuNavigation(Vector2.up);
                            }
                            else if (leftStickGamepad.y < -stickDeadzone)
                            {
                                MenuNavigation(Vector2.down);
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                }
                else
                {
                    if (currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, leftStickGamepad.y));
                    }
                }
            }

            // Is Triggered
            if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
            {
                MenuNavigation(Vector2.up);
            }
            else if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
            {
                MenuNavigation(Vector2.down);
            }
            else if (gamePadState.IsTriggered(WiiU.GamePadButton.Left))
            {
                MenuNavigation(Vector2.left);
            }
            else if (gamePadState.IsTriggered(WiiU.GamePadButton.Right))
            {
                MenuNavigation(Vector2.right);
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
            {
                ClickSelectedButton();
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.B))
            {
                GoBack();
            }

            // Is pressed
            if (gamePadState.IsPressed(WiiU.GamePadButton.Up))
            {
                if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                {
                    ScrollNavigation(new Vector2(0, 1));
                }
            }
            else if (gamePadState.IsPressed(WiiU.GamePadButton.Down))
            {
                if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                {
                    ScrollNavigation(new Vector2(0, -1));
                }
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                // Stick
                Vector2 leftStickProController = remoteState.pro.leftStick;

                if (Mathf.Abs(leftStickProController.y) > stickDeadzone)
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.y > stickDeadzone)
                                {
                                    MenuNavigation(Vector2.up);
                                }
                                else if (leftStickProController.y < -stickDeadzone)
                                {
                                    MenuNavigation(Vector2.down);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                    }
                    else
                    {
                        if (currentScrollRect != null)
                        {
                            ScrollNavigation(new Vector2(0, leftStickProController.y));
                        }
                    }
                }

                // Is triggered
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                {
                    MenuNavigation(Vector2.up);
                }
                else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                {
                    MenuNavigation(Vector2.down);
                }
                else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Left))
                {
                    MenuNavigation(Vector2.left);
                }
                else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Right))
                {
                    MenuNavigation(Vector2.right);
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
                {
                    ClickSelectedButton();
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.B))
                {
                    GoBack();
                }

                // Is pressed
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Up))
                {
                    if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, 1));
                    }
                }
                else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Down))
                {
                    if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, -1));
                    }
                }

                break;
            case WiiU.RemoteDevType.Classic:
                // Stick
                Vector2 leftStickClassicController = remoteState.classic.leftStick;

                if (Mathf.Abs(leftStickClassicController.y) > stickDeadzone)
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.y > stickDeadzone)
                                {
                                    MenuNavigation(Vector2.up);
                                }
                                else if (leftStickClassicController.y < -stickDeadzone)
                                {
                                    MenuNavigation(Vector2.down);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                    }
                    else
                    {
                        if (currentScrollRect != null)
                        {
                            ScrollNavigation(new Vector2(0, leftStickClassicController.y));
                        }
                    }
                }

                // Is triggered
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                {
                    MenuNavigation(Vector2.up);
                }
                else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                {
                    MenuNavigation(Vector2.down);
                }
                else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Left))
                {
                    MenuNavigation(Vector2.left);
                }
                else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Right))
                {
                    MenuNavigation(Vector2.right);
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
                {
                    ClickSelectedButton();
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.B))
                {
                    GoBack();
                }

                // Is pressed
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Up))
                {
                    if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, 1));
                    }
                }
                else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Down))
                {
                    if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, -1));
                    }
                }

                break;
            default:
                // Stick
                Vector2 stickNunchuk = remoteState.nunchuk.stick;

                if (Mathf.Abs(stickNunchuk.y) > stickDeadzone)
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.y > stickDeadzone)
                                {
                                    MenuNavigation(Vector2.up);
                                }
                                else if (stickNunchuk.y < -stickDeadzone)
                                {
                                    MenuNavigation(Vector2.down);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                    }
                    else
                    {
                        if (currentScrollRect != null)
                        {
                            ScrollNavigation(new Vector2(0, stickNunchuk.y));
                        }
                    }
                }

                // Is triggered
                if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                {
                    MenuNavigation(Vector2.up);
                }
                else if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                {
                    MenuNavigation(Vector2.down);
                }
                else if (remoteState.IsTriggered(WiiU.RemoteButton.Left))
                {
                    MenuNavigation(Vector2.left);
                }
                else if (remoteState.IsTriggered(WiiU.RemoteButton.Right))
                {
                    MenuNavigation(Vector2.right);
                }

                if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                {
                    ClickSelectedButton();
                }

                if (remoteState.IsTriggered(WiiU.RemoteButton.B))
                {
                    GoBack();
                }

                // Is pressed
                if (remoteState.IsPressed(WiiU.RemoteButton.Up))
                {
                    if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, 1));
                    }
                }
                else if (remoteState.IsPressed(WiiU.RemoteButton.Down))
                {
                    if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                    {
                        ScrollNavigation(new Vector2(0, -1));
                    }
                }

                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MenuNavigation(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MenuNavigation(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MenuNavigation(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MenuNavigation(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ClickSelectedButton();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                GoBack();
            }

            // Key
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                {
                    ScrollNavigation(new Vector2(0, 1));
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (EventSystem.current.currentSelectedGameObject == null && currentScrollRect != null)
                {
                    ScrollNavigation(new Vector2(0, -1));
                }
            }
        }

        // Calculate stick last navigation time
        lastNavigationTime += Time.deltaTime;
    }

    private void MenuNavigation(Vector2 direction)
    {
        if (canNavigate)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (direction == Vector2.up)
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    {
                        Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                    }
                }
                else if (direction == Vector2.left)
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    {
                        Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                    }
                }
                else if (direction == Vector2.down)
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    {
                        Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                    }
                }
                else if (direction == Vector2.right)
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    {
                        Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                    }
                }
            }
            else
            {
                if (lastSelected != null)
                {
                    Select(lastSelected);
                }
            }

            AutoScroll();
        }
    }

    public void ScrollNavigation(Vector2 direction)
    {
        if (canNavigate)
        {
            RectTransform content = currentScrollRect.content;
            RectTransform viewport = currentScrollRect.viewport;

            if (content != null && viewport != null)
            {
                // Total size of content and visible view
                float contentHeight = content.rect.height;
                float viewportHeight = viewport.rect.height;

                if (contentHeight > viewportHeight)
                {
                    // Calculating proportional scrolling
                    float scrollAmount = (600 / (contentHeight - viewportHeight)) * direction.y * Time.deltaTime;

                    Vector2 newPosition = currentScrollRect.normalizedPosition + new Vector2(0f, scrollAmount);
                    newPosition.y = Mathf.Clamp01(newPosition.y);
                    currentScrollRect.normalizedPosition = newPosition;
                }
            }
        }
    }

    public void Select(Selectable selectable)
    {
        if (canNavigate)
        {
            if (selectable != null)
            {
                if (selectable.GetComponent<Button>() != null)
                {
                    Button newSelectable = selectable.GetComponent<Button>();
                    newSelectable.Select();
                }

                if (selectable.GetComponent<ButtonManager>() != null)
                {
                    selectable.GetComponent<ButtonManager>().UpdateTextColor();
                }                

                lastSelected = selectable;
            }
        }
    }

    private void ClickSelectedButton()
    {
        if (canNavigate)
        {
            if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void ChangeMenu(int menuId)
    {
        if (currentMenuId != menuId && !isNavigatingBack)
        {
            menuHistory.Push(currentMenuId);
        }

        // Menu
        foreach (GameObject menu in menus)
        {
            menu.SetActive(menu == menus[menuId]);
        }

        // Button
        for (int i = 0; i < defaultButtons.Length; i++)
        {
            if (i == menuId)
            {
                if (currentPopup == null)
                {
                    if (defaultButtons[i] != null)
                    {
                        Select(defaultButtons[i]);
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        lastSelected = null;
                    }
                }
            }
        }

        currentMenuId = menuId;

        // Set scroll rect if component exists
        currentScrollRect = menus[menuId].transform.Find("MainPanel").GetChild(0).GetComponent<ScrollRect>();

        isNavigatingBack = false;
    }

    public void GoBack()
    {
        if (currentPopup == null && canNavigate)
        {
            if (menuHistory.Count > 0)
            {
                // Set the navigation back flag to true
                isNavigatingBack = true;

                // Execute the callback for the current menu, if it exists
                if (backCallbacks.ContainsKey(currentMenuId) && backCallbacks[currentMenuId] != null)
                {
                    backCallbacks[currentMenuId].Invoke();
                }

                // Retrieve the previous menu ID from the history stack
                int previousMenuId = menuHistory.Pop();

                // Change to the previous menu
                ChangeMenu(previousMenuId);
            }
        }
    }

    public void SetBackCallback(int menuId, UnityEngine.Events.UnityAction callback)
    {
        if (backCallbacks.ContainsKey(menuId))
        {
            backCallbacks[menuId] = callback;
        }
        else
        {
            backCallbacks.Add(menuId, callback);
        }
    }

    private void AutoScroll()
    {
        if (currentScrollRect != null && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
        {
            int index = 0;
            float targetPosition;

            Button[] buttons = menus[currentMenuId].GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                if (button == EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                {
                    break;
                }
                index++;
            }

            targetPosition = 1f - ((float)index / (buttons.Length - 1));

            // Stop any running coroutine to avoid conflicts
            if (autoScrollCoroutine != null)
            {
                StopCoroutine(autoScrollCoroutine);
            }

            // Start a new coroutine
            autoScrollCoroutine = StartCoroutine(ScrollCoroutine(targetPosition));
        }
    }

    private IEnumerator ScrollCoroutine(float targetPosition)
    {
        float duration = 0.1f;
        float elapsedTime = 0f;
        float startPosition = currentScrollRect.verticalNormalizedPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentScrollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, elapsedTime / duration);

            yield return null; // Wait for next frame
        }

        // Ensure that the final position is reached
        currentScrollRect.verticalNormalizedPosition = targetPosition;
    }
}