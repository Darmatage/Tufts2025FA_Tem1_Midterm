using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class StageScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public string stageToLoad;
    public Color highlightColor = Color.yellow;
    public float glowIntensity = 1.0f;

    public TMP_Text infoText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        infoText.text = "Hover over special locations of the map to see which jobs you can take on!";
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = highlightColor * glowIntensity;
        
        switch (stageToLoad)
        {
            case "CafeMatcha":
                infoText.text = "A quaint little shop. You've been hired to take on an unpopular worker who is always hitting on customers. All he does is drink matcha all day, so he's not very in shape. (Difficulty - Easy)";
                break;
            case "Bench":
                infoText.text = "You've been hired to take on an infamous thug at the park. Since they know how to fight, it might be a little bit of a challenge. (Difficulty - Medium)";
                break;
            case "VintageStore":
                infoText.text = "The legendary Jason Gizer is said to hang around this vintage store. You know this will be a hellish battle, but the pay is too good to pass up... (Difficulty - Hard)";
                break;
            default:
                break;
        }
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
        infoText.text = "Hover over special locations of the map to see which jobs you can take on!";
    }

    void OnMouseDown()
    {
        StageData.selectedStage = stageToLoad;
        SceneManager.LoadScene("attackphase");
    }
}