using UnityEngine;
using UnityEngine.InputSystem;
public class CircleScript : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite defaultImage;
    public Sprite pressedImage;
    private Vector3 originalScale;


    public KeyCode keyToPress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = defaultImage;
        originalScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseOver()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            spriteRenderer.sprite = pressedImage;
            GetComponent<Collider2D>().enabled = false;
            transform.localScale = originalScale * 1.2f;
            Destroy(gameObject, 0.2f);
            Debug.Log("Circle hit via OnMouseOver!");
        }
    }

}
