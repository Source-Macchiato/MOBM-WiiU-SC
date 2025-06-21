using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
	public static string sceneToLoad;

	void Start()
	{
		if (sceneToLoad != null)
		{
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
	}
	
	public static void LoadScene(string scene)
	{
		sceneToLoad = scene;

		SceneManager.LoadSceneAsync("Loading");
	}
}
