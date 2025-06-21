using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderMovie : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public MovieTexture srcMacchiatoVideo;
    public MovieTexture introVideo;

    public AudioSource srcMacchiatoAudio;
    public AudioSource introAudio;

    private bool skipIntro = false;

    void Start()
    {
        StartCoroutine(Sequence());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            skipIntro = true;
        }
    }

    private IEnumerator Sequence()
    {
        meshRenderer.material.mainTexture = srcMacchiatoVideo;

        srcMacchiatoVideo.Play();
        srcMacchiatoAudio.Play();

        yield return new WaitUntil(() => !srcMacchiatoAudio.isPlaying);

        srcMacchiatoVideo.Stop();
        srcMacchiatoAudio.Stop();

        meshRenderer.material.mainTexture = introVideo;

        introVideo.Play();
        introAudio.Play();

        skipIntro = false;

        yield return new WaitUntil(() => !introAudio.isPlaying || skipIntro);

        SceneManager.LoadSceneAsync("GameSelector");
    }
}