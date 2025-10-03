using UnityEngine;
using System.Collections;
using TMPro;
public class CircleScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public Sprite wrongImage;

    private Vector3 originalScale;
    private Vector3 originalPos;

    public CircleSpawner spawner;

    private bool alreadyHit = false;
    [SerializeField] private TMP_Text label;

    void Start()
    {
        spriteRenderer.sprite = defaultImage;
        originalScale = transform.localScale;
        originalPos = transform.localPosition;
        label = GetComponentInChildren<TMP_Text>();
    }

    public void CircleMissed()
    {
        spriteRenderer.sprite = wrongImage;
        Destroy(gameObject, 0.2f);
    }

    void OnMouseOver()
    {
        if (alreadyHit) return;

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
            // Ask spawner if this is the next circle
            if (spawner.TryHit(this))
            {
                spriteRenderer.sprite = pressedImage;
                transform.localScale = originalScale * 1.2f;
                alreadyHit = true;

                Destroy(gameObject, 0.2f);
            }
            else
            {
                spriteRenderer.sprite = wrongImage;
                StartCoroutine(Shake(0.2f, 0.15f));
            }
        }
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        spriteRenderer.sprite = defaultImage;

    }

    public void SetLabel(string text)
    {
        if (label != null)
            label.text = text;
    }

}
