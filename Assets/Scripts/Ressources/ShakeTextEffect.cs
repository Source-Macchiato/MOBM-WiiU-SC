using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ShakeTextEffect : MonoBehaviour
{
    public float intensity = 1f;
    public float speed = 1f;

    private TMP_Text tmpText;
    private float time;
    private Vector3[][] originalVertices;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        CacheOriginalVertices(true);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void Update()
    {
        if (!Application.isPlaying) return;

        time += Time.deltaTime * speed;
        ApplyShakeEffect();
    }

    void OnTextChanged(Object obj)
    {
        if (obj == tmpText)
        {
            CacheOriginalVertices(false);
        }
    }

    void CacheOriginalVertices(bool forceUpdate)
    {
        if (forceUpdate)
            tmpText.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpText.textInfo;
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = (Vector3[])textInfo.meshInfo[i].vertices.Clone();
        }
    }

    void ApplyShakeEffect()
    {
        if (originalVertices == null) return;

        TMP_TextInfo textInfo = tmpText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            // character center position
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3 center = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2;

            // Perlin noise for shake effect
            float seed = i * 100f;
            float offsetX = (Mathf.PerlinNoise(seed, time * 10f) * 2 - 1) * intensity;
            float offsetY = (Mathf.PerlinNoise(seed + 50f, time * 10f) * 2 - 1) * intensity;
            Vector3 offset = new Vector3(offsetX, offsetY, 0);

            // Apply the shake effect to each vertex of the character
            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j] = originalVertices[materialIndex][vertexIndex + j] - center + offset + center;
            }
        }

        // Update mesh vertices
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            tmpText.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
