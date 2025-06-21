using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared : MonoBehaviour 
{

    // ------ Audio shared code ------
    public static IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }
    public static IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0;
        audioSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, targetVolume, t / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }



    // ------ Dissolve shared code ------
    public static IEnumerator DissolveFade(GameObject FadeGO, float startValue, float endValue, float duration)
    {
        // Image component for UI
        UnityEngine.UI.Image img = FadeGO.GetComponent<UnityEngine.UI.Image>();
        if (img == null)
        {
            Debug.LogError("Image component not found on the GameObject: " + FadeGO.name);
            yield break;
        }

        Material dissolveMaterial = img.material;
        float time = 0f;
        FadeGO.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            float cutoff = Mathf.Lerp(startValue, endValue, time / duration);
            dissolveMaterial.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

        dissolveMaterial.SetFloat("_Cutoff", endValue);

        if (endValue == 1f)
        {
            FadeGO.SetActive(false);
        }
    }

    public IEnumerator Transition(GameObject SceneObj, GameObject DissolveTransition, float duration1, float duration2, float duration3)
    {
        yield return new WaitForSeconds(duration1);
        StartCoroutine(DissolveFade(DissolveTransition, 1f, 0f, 1f));
        yield return new WaitForSeconds(duration2);
        SceneObj.SetActive(false);
        yield return new WaitForSeconds(duration3);
        StartCoroutine(DissolveFade(DissolveTransition, 0f, 1f, 1f));
    }

    public static void StartTransition(GameObject SceneObj, GameObject DissolveTransition, float duration1, float duration2, float duration3)
    {
        Shared instance = FindObjectOfType<Shared>();
        if (instance != null)
        {
            instance.StartCoroutine(instance.Transition(SceneObj, DissolveTransition, duration1, duration2, duration3));
        }
    }

    public static IEnumerator SetFade(GameObject FadeGO, float SetValue)
    {
        // Image component for UI
        UnityEngine.UI.Image img = FadeGO.GetComponent<UnityEngine.UI.Image>();
        if (img == null)
        {
            Debug.LogError("Image component not found on the GameObject: " + FadeGO.name);
            yield break;
        }

        Material dissolveMaterial = img.material;
        FadeGO.SetActive(true);
        dissolveMaterial.SetFloat("_Cutoff", SetValue);
    }


}

