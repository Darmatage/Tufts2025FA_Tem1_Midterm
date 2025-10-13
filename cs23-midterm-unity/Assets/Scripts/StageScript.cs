using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class StageScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public string stageToLoad;
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
        StageData.selectedStage = stageToLoad;
        SceneManager.LoadScene("attackphase");
    }
}