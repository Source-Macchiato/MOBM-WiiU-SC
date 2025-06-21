using UnityEngine;
using UnityEngine.UI;

public class RandomBg : MonoBehaviour
{
    // target Image component where the background will be displayed
    public Image targetImage;

    // folder contenant les sprites de fond
    public string backgroundsFolder = "cg_mirror_gg";

    // Tableau pour stocker les sprites chargés
    private Sprite[] backgroundSprites;

	public void Start()
	{
		ChangeBackground();
	}

    // load all sprites from Ressources
    void Awake()
    {
        backgroundSprites = Resources.LoadAll<Sprite>(backgroundsFolder);
        if (backgroundSprites == null || backgroundSprites.Length == 0)
        {
            Debug.LogError("No sprites found in the specified folder: " + backgroundsFolder);
        }
    }

    // Randomly change the background sprite
    public void ChangeBackground()
    {
        if (backgroundSprites == null || backgroundSprites.Length == 0)
        {
            Debug.LogError("No sprites available to change the background.");
            return;
        }

        int randomIndex = Random.Range(0, backgroundSprites.Length);
        targetImage.sprite = backgroundSprites[randomIndex];
    }
}
