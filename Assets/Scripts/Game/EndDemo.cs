using System.Collections;
using UnityEngine;

public class EndDemo : MonoBehaviour
{
	public GameObject[] endTexts;
	public AudioSource fxAudio;

	void Start()
	{
        foreach (GameObject endText in endTexts)
		{
			endText.SetActive(false);
		}

		StartCoroutine(Sequence());
    }

	private IEnumerator Sequence()
	{
		endTexts[0].SetActive(true);
		fxAudio.Play();

		yield return new WaitForSeconds(3f);

		endTexts[0].SetActive(false);
		endTexts[1].SetActive(true);
		fxAudio.Play();

		yield return new WaitForSeconds(3f);

		endTexts[1].SetActive(false);
		endTexts[2].SetActive(true);
		fxAudio.Play();
	}
}
