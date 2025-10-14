using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DodgePhaseManager : MonoBehaviour
{

    // Warnings
    public GameObject leftWarning;
    public GameObject rightWarning;
    public GameObject topWarning;
    
    // Enemy Attacks
    public GameObject enemyLeftPunch;
    public GameObject enemyRightPunch;
    public GameObject enemyTopPunch;

    // Screen object references
    public GameObject player;  
    public PlayerScript playerScript;
    public GameObject enemy;
    public GameObject redFlash;
    
    // Timing settings
    public float warningDuration = 0.5f;
    public float timeBetweenAttacks = 1.5f;

    // How fast player moves
    public float dodgeSpeed = 10f;  

    // Speed for enemy movement
    public float enemyMoveSpeed = 8f; 
    private int numAttacks;
    
    // Positions found by trial and error
    private Vector3 defaultPosition = new Vector3(1.1f, -0.4f, 0.001f);
    private Vector3 leftDodgePosition = new Vector3(-2.08f, -0.72f, 0.001f);
    private Vector3 rightDodgePosition = new Vector3(4.02f, -0.72f, 0.001f);
    private Vector3 downDodgePosition = new Vector3(1.1f, -1.5f, 0.001f);

    // Enemy positions
    private Vector3 enemyDefaultPosition = new Vector3(0f, -1.3f, 0f);
    private Vector3 enemyRightPunchPosition = new Vector3(3.61f, -1.3f, 0f);
    private Vector3 enemyLeftPunchPosition = new Vector3(-3.47f, -1.3f, 0f);
    
    private enum AttackDirection { Left, Right, Top }
    private AttackDirection currentAttack;
    private bool waitingForInput = false;

    private AudioSource audioSource;

    void Start()
    {
        int difficulty = StageData.stageDifficulty;
        player.transform.position = defaultPosition;
        enemy.transform.position = enemyDefaultPosition; 
        audioSource = GetComponent<AudioSource>();

        numAttacks = difficulty * 5;

        StartCoroutine(AttackCycle());

    }
    
    void Update()
    {
        if (waitingForInput)
        {
            CheckPlayerInput();
        }
    }
    
    IEnumerator AttackCycle()
    {
        while (numAttacks > 0)
        {
            // Wait between attacks
            yield return new WaitForSeconds(timeBetweenAttacks);

            // Pick random attack
            currentAttack = (AttackDirection)Random.Range(0, 3);

            // Show warning and START checking for input
            ShowWarning(currentAttack);
            waitingForInput = true;

            float timer = 0;
            while (waitingForInput && timer < warningDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Hide warning
            HideWarning(currentAttack);

            // Show attack regardless of success/failure
            ShowAttack(currentAttack);

            // Check if player dodged in time
            if (waitingForInput)
            {
                // Time ran out - they failed
                Failed();
            }
            // If waitingForInput is false, Success() was already called

            yield return new WaitForSeconds(0.3f);
            HideAttack(currentAttack);

            waitingForInput = false;
        }

        SceneManager.LoadScene("attackphase");
    }
    
    void ShowWarning(AttackDirection direction)
    {
        if (direction == AttackDirection.Left) leftWarning.SetActive(true);
        else if (direction == AttackDirection.Right) rightWarning.SetActive(true);
        else if (direction == AttackDirection.Top) topWarning.SetActive(true);
    }
    
    void HideWarning(AttackDirection direction)
    {
        if (direction == AttackDirection.Left) leftWarning.SetActive(false);
        else if (direction == AttackDirection.Right) rightWarning.SetActive(false);
        else if (direction == AttackDirection.Top) topWarning.SetActive(false);
    }

    IEnumerator PunchAnimation(GameObject punchObject)
    {
        float duration = 0.1f; 
        float startScale = 0.2f; 
        float endScale = 0.5f;  
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(startScale, endScale, elapsed / duration);
            punchObject.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        
        // Make sure it ends at full scale
        punchObject.transform.localScale = new Vector3(endScale, endScale, 1f);
    }

    IEnumerator EnemyMovement(Vector3 targetPosition)
    {
        // Move to attack position
        while (Vector3.Distance(enemy.transform.position, targetPosition) > 0.01f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, enemyMoveSpeed * Time.deltaTime);
            yield return null;
        }
        
        // Waiting briefly
        yield return new WaitForSeconds(0.2f);
        
        // Return to default position
        while (Vector3.Distance(enemy.transform.position, enemyDefaultPosition) > 0.01f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemyDefaultPosition, enemyMoveSpeed * Time.deltaTime);
            yield return null;
        }
        
        enemy.transform.position = enemyDefaultPosition;  
    }
    
    void ShowAttack(AttackDirection direction)
    {
        GameObject attackObject = null;
        
        if (direction == AttackDirection.Left)
        {
            attackObject = enemyLeftPunch;
            StartCoroutine(EnemyMovement(enemyLeftPunchPosition));  
        }
        else if (direction == AttackDirection.Right)
        {
            attackObject = enemyRightPunch;
            StartCoroutine(EnemyMovement(enemyRightPunchPosition));  
        }
        else if (direction == AttackDirection.Top)
        {
            attackObject = enemyTopPunch;
        }
        
        if (attackObject != null)
        {
            attackObject.SetActive(true);
            StartCoroutine(PunchAnimation(attackObject));
        }
    }
    
    void HideAttack(AttackDirection direction)
    {
        GameObject attackObject = null;
        
        if (direction == AttackDirection.Left) attackObject = enemyLeftPunch;
        else if (direction == AttackDirection.Right) attackObject = enemyRightPunch;
        else if (direction == AttackDirection.Top) attackObject = enemyTopPunch;
        
        if (attackObject != null)
        {
            attackObject.SetActive(false);

            // Reset scale for next time
            attackObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    
    void CheckPlayerInput()
    {
        // Attack from right meaning dodge left
        if (currentAttack == AttackDirection.Right && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Success(leftDodgePosition);
        }
        // Attack from left meaning dodge right
        else if (currentAttack == AttackDirection.Left && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Success(rightDodgePosition);
        }
        // Attack from top meaning dodge down
        else if (currentAttack == AttackDirection.Top && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Success(downDodgePosition);
        }

        // Incorrect input
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Failed();
        }
    }
    
    void Success(Vector3 dodgePosition)
    {
        waitingForInput = false;
        Debug.Log("Dodged successfully!");
        numAttacks--;
        StartCoroutine(DodgeMovement(dodgePosition));
    }
    
    void Failed()
    {
        waitingForInput = false;
        Debug.Log("Hit! Take damage");

        int damage = StageData.stageDifficulty * 3;
        playerScript.TakeDamage(damage);
        numAttacks--;
        audioSource.time = 0.8f;
        audioSource.Play();
        StartCoroutine(RedFlashEffect());
    }

    IEnumerator RedFlashEffect()
    {
        redFlash.SetActive(true);
        yield return new WaitForSeconds(0.3f); 
        redFlash.SetActive(false);
    }
    
    IEnumerator DodgeMovement(Vector3 targetPosition)
    {
        
        // Move to dodge position
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, dodgeSpeed * Time.deltaTime);
            yield return null;
        }
        
        // Waiting briefly
        yield return new WaitForSeconds(0.2f);
        
        // Return to default position
        while (Vector3.Distance(player.transform.position, defaultPosition) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, defaultPosition, dodgeSpeed * Time.deltaTime);
            yield return null;
        }
        
        player.transform.position = defaultPosition; 
    }
}
