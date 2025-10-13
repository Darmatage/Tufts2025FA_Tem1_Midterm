using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class StageScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public string sceneToLoad;
    public Color highlightColor = Color.yellow;
    public float glowIntensity = 1.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = highlightColor * glowIntensity;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log($"No scene assigned for {gameObject.name}");
        }
    }
}