using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using WiiU = UnityEngine.WiiU;

public class GameSelector : MonoBehaviour
{
	public Animator separatorAnimator;
	public Animator milk1Animator;
	public Animator milk2Animator;

	public int currentSelected = 0;

	private bool canNavigate = true;

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
                if (lastNavigationTime > stickNavigationCooldown)
                {
                    if (leftStickGamepad.y > stickDeadzone)
                    {
                        if (canNavigate)
                        {
                            if (currentSelected == 0)
                            {
                                lastNavigationTime = 0f;

                                StartCoroutine(Milk1());
                            }
                            else if (currentSelected == 2)
                            {
                                lastNavigationTime = 0f;

                                StartCoroutine(FromMilk2ToMilk1());
                            }
                        }
                    }
                    else if (leftStickGamepad.y < -stickDeadzone)
                    {
                        if (canNavigate)
                        {
                            if (currentSelected == 0)
                            {
                                lastNavigationTime = 0f;

                                StartCoroutine(Milk2());
                            }
                            else if (currentSelected == 1)
                            {
                                lastNavigationTime = 0f;

                                StartCoroutine(FromMilk1ToMilk2());
                            }
                        }
                    }
                }
            }

            // Is Triggered
            if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
            {
                if (canNavigate)
                {
                    if (currentSelected == 0)
                    {
                        StartCoroutine(Milk1());
                    }
                    else if (currentSelected == 2)
                    {
                        StartCoroutine(FromMilk2ToMilk1());
                    }
                }
            }
            else if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
            {
                if (canNavigate)
                {
                    if (currentSelected == 0)
                    {
                        StartCoroutine(Milk2());
                    }
                    else if (currentSelected == 1)
                    {
                        StartCoroutine(FromMilk1ToMilk2());
                    }
                }
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
            {
                if (currentSelected == 1)
                {
                    SceneManager.LoadSceneAsync("Milk1MainMenu");
                }
                else if (currentSelected == 2)
                {
                    SceneManager.LoadSceneAsync("Milk2MainMenu");
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
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        if (leftStickProController.y > stickDeadzone)
                        {
                            if (canNavigate)
                            {
                                if (currentSelected == 0)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(Milk1());
                                }
                                else if (currentSelected == 2)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(FromMilk2ToMilk1());
                                }
                            }
                        }
                        else if (leftStickProController.y < -stickDeadzone)
                        {
                            if (canNavigate)
                            {
                                if (currentSelected == 0)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(Milk2());
                                }
                                else if (currentSelected == 1)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(FromMilk1ToMilk2());
                                }
                            }
                        }
                    }
                }

                // Is Triggered
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                {
                    if (canNavigate)
                    {
                        if (currentSelected == 0)
                        {
                            StartCoroutine(Milk1());
                        }
                        else if (currentSelected == 2)
                        {
                            StartCoroutine(FromMilk2ToMilk1());
                        }
                    }
                }
                else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                {
                    if (canNavigate)
                    {
                        if (currentSelected == 0)
                        {
                            StartCoroutine(Milk2());
                        }
                        else if (currentSelected == 1)
                        {
                            StartCoroutine(FromMilk1ToMilk2());
                        }
                    }
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
                {
                    if (currentSelected == 1)
                    {
                        SceneManager.LoadSceneAsync("Milk1MainMenu");
                    }
                    else if (currentSelected == 2)
                    {
                        SceneManager.LoadSceneAsync("Milk2MainMenu");
                    }
                }
                break;
            case WiiU.RemoteDevType.Classic:
                // Stick
                Vector2 leftStickClassicController = remoteState.classic.leftStick;

                if (Mathf.Abs(leftStickClassicController.y) > stickDeadzone)
                {
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        if (leftStickClassicController.y > stickDeadzone)
                        {
                            if (canNavigate)
                            {
                                if (currentSelected == 0)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(Milk1());
                                }
                                else if (currentSelected == 2)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(FromMilk2ToMilk1());
                                }
                            }
                        }
                        else if (leftStickClassicController.y < -stickDeadzone)
                        {
                            if (canNavigate)
                            {
                                if (currentSelected == 0)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(Milk2());
                                }
                                else if (currentSelected == 1)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(FromMilk1ToMilk2());
                                }
                            }
                        }
                    }
                }

                // Is Triggered
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                {
                    if (canNavigate)
                    {
                        if (currentSelected == 0)
                        {
                            StartCoroutine(Milk1());
                        }
                        else if (currentSelected == 2)
                        {
                            StartCoroutine(FromMilk2ToMilk1());
                        }
                    }
                }
                else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                {
                    if (canNavigate)
                    {
                        if (currentSelected == 0)
                        {
                            StartCoroutine(Milk2());
                        }
                        else if (currentSelected == 1)
                        {
                            StartCoroutine(FromMilk1ToMilk2());
                        }
                    }
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
                {
                    if (currentSelected == 1)
                    {
                        SceneManager.LoadSceneAsync("Milk1MainMenu");
                    }
                    else if (currentSelected == 2)
                    {
                        SceneManager.LoadSceneAsync("Milk2MainMenu");
                    }
                }
                break;
            default:
                // Stick
                Vector2 stickNunchuk = remoteState.nunchuk.stick;

                if (Mathf.Abs(stickNunchuk.y) > stickDeadzone)
                {
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        if (stickNunchuk.y > stickDeadzone)
                        {
                            if (canNavigate)
                            {
                                if (currentSelected == 0)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(Milk1());
                                }
                                else if (currentSelected == 2)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(FromMilk2ToMilk1());
                                }
                            }
                        }
                        else if (stickNunchuk.y < -stickDeadzone)
                        {
                            if (canNavigate)
                            {
                                if (currentSelected == 0)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(Milk2());
                                }
                                else if (currentSelected == 1)
                                {
                                    lastNavigationTime = 0f;

                                    StartCoroutine(FromMilk1ToMilk2());
                                }
                            }
                        }
                    }
                }

                // Is Triggered
                if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                {
                    if (canNavigate)
                    {
                        if (currentSelected == 0)
                        {
                            StartCoroutine(Milk1());
                        }
                        else if (currentSelected == 2)
                        {
                            StartCoroutine(FromMilk2ToMilk1());
                        }
                    }
                }
                else if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                {
                    if (canNavigate)
                    {
                        if (currentSelected == 0)
                        {
                            StartCoroutine(Milk2());
                        }
                        else if (currentSelected == 1)
                        {
                            StartCoroutine(FromMilk1ToMilk2());
                        }
                    }
                }

                if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                {
                    if (currentSelected == 1)
                    {
                        SceneManager.LoadSceneAsync("Milk1MainMenu");
                    }
                    else if (currentSelected == 2)
                    {
                        SceneManager.LoadSceneAsync("Milk2MainMenu");
                    }
                }
                break;
        }

        if (Application.isEditor)
		{
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (canNavigate)
                {
                    if (currentSelected == 0)
                    {
                        StartCoroutine(Milk1());
                    }
                    else if (currentSelected == 2)
                    {
                        StartCoroutine(FromMilk2ToMilk1());
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (canNavigate)
                {
                    if (currentSelected == 0)
                    {
                        StartCoroutine(Milk2());
                    }
                    else if (currentSelected == 1)
                    {
                        StartCoroutine(FromMilk1ToMilk2());
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentSelected == 1)
                {
                    SceneManager.LoadSceneAsync("Milk1MainMenu");
                }
                else if (currentSelected == 2)
                {
                    SceneManager.LoadSceneAsync("Milk2MainMenu");
                }
            }
        }

        // Calculate stick last navigation time
        lastNavigationTime += Time.deltaTime;
    }

	private IEnumerator Milk1()
	{
		canNavigate = false;

		separatorAnimator.Play("StartDown");

		milk1Animator.Play("StartSelect");
		milk2Animator.Play("StartUnselect");

		currentSelected = 1;

		yield return new WaitForSeconds(0.26f);

		canNavigate = true;
	}

	private IEnumerator Milk2()
	{
		canNavigate = false;

		separatorAnimator.Play("StartUp");

		milk1Animator.Play("StartUnselect");
		milk2Animator.Play("StartSelect");

		currentSelected = 2;

		yield return new WaitForSeconds(0.26f);

		canNavigate = true;
	}

	private IEnumerator FromMilk1ToMilk2()
	{
		canNavigate = false;

        separatorAnimator.Play("Up");

        milk1Animator.Play("Unselect");
        milk2Animator.Play("Select");

		currentSelected = 2;

		yield return new WaitForSeconds(0.26f);

		canNavigate = true;
    }

	private IEnumerator FromMilk2ToMilk1()
	{
		canNavigate = false;

        separatorAnimator.Play("Down");

        milk1Animator.Play("Select");
        milk2Animator.Play("Unselect");

		currentSelected = 1;

		yield return new WaitForSeconds(0.26f);

		canNavigate = true;
    }
}
