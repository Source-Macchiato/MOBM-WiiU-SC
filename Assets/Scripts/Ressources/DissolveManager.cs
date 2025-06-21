using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DissolveManager : MonoBehaviour {


    // GameOject list to check
    public List<GameObject> gameObjectsToCheck;

    // Start the continuous check
    private void Start()
    {
        StartCoroutine(CheckGameObjects());
    }

    // Coroutine that continuously checks each GameObject in the list
    private IEnumerator CheckGameObjects()
    {
        while (true)
        {
            // For each GameObject in the list
            foreach (GameObject go in gameObjectsToCheck)
            {
                // Get the Image component
                Image img = go.GetComponent<Image>();
                if (img == null)
                {
                    Debug.LogWarning("GameObject " + go.name + " does not have an Image component.");
                    continue;
                }

                // Get the material and the current value of "_Cutoff"
                Material mat = img.material;
                float currentValue = mat.GetFloat("_Cutoff");

                // If the value is equal to 1, disable the GameObject
                if (Mathf.Approximately(currentValue, 1f))
                {
                    if (go.activeSelf) // avoid calling SetActive if already disabled
                    {
                        go.SetActive(false);
                    }
                }

                // Otherwise, check if the value is less than 1
                else if (currentValue == 0f)
                {
                    // Disable the GameObject if it is active
                    if (go.activeSelf) // avoid calling SetActive if already disabled
                    {
                        go.SetActive(false);
                    }
                }
                // Otherwise, activate the GameObject
                else
                {
                    if (!go.activeSelf) // avoid calling SetActive if already enabled
                    {
                        go.SetActive(true);
                    }
                }
            }
            // Wait for the next frame
            yield return null;
        }
    }
}