using System.Collections;
using UnityEngine;

public class DodgePhaseManager : MonoBehaviour
{
    public GameObject leftWarning;
    public GameObject rightWarning;
    public GameObject topWarning;
    
    public GameObject enemyLeftPunch;
    public GameObject enemyRightPunch;
    public GameObject enemyTopPunch;
    
    // How long warning shows
    public float warningDuration = 0.5f; 

    // How long player has to react
    public float reactionWindow = 1f;    
    public float timeBetweenAttacks = 1.5f;
    
    private enum AttackDirection { Left, Right, Top }
    private AttackDirection currentAttack;
    private bool waitingForInput = false;
    
    void Start()
    {
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
        while (true)
        {
            // Wait between attacks
            yield return new WaitForSeconds(timeBetweenAttacks);
            
            // Pick random attack
            currentAttack = (AttackDirection)Random.Range(0, 3);
            
            // Show warning
            ShowWarning(currentAttack);
            yield return new WaitForSeconds(warningDuration);
            HideWarning(currentAttack);
            
            // Show attack and wait for input
            ShowAttack(currentAttack);
            waitingForInput = true;
            
            float timer = 0;
            while (waitingForInput && timer < reactionWindow)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            // If time ran out
            if (waitingForInput)
            {
                Failed();
            }
            
            HideAttack(currentAttack);
            waitingForInput = false;
        }
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
    
    void ShowAttack(AttackDirection direction)
    {
        if (direction == AttackDirection.Left) enemyLeftPunch.SetActive(true);
        else if (direction == AttackDirection.Right) enemyRightPunch.SetActive(true);
        else if (direction == AttackDirection.Top) enemyTopPunch.SetActive(true);
    }
    
    void HideAttack(AttackDirection direction)
    {
        if (direction == AttackDirection.Left) enemyLeftPunch.SetActive(false);
        else if (direction == AttackDirection.Right) enemyRightPunch.SetActive(false);
        else if (direction == AttackDirection.Top) enemyTopPunch.SetActive(false);
    }
    
    void CheckPlayerInput()
    {
        // Attack from RIGHT → dodge LEFT
        if (currentAttack == AttackDirection.Right && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Success();
        }
        // Attack from LEFT → dodge RIGHT
        else if (currentAttack == AttackDirection.Left && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Success();
        }
        // Attack from TOP → dodge DOWN
        else if (currentAttack == AttackDirection.Top && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Success();
        }
        // Wrong input
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Failed();
        }
    }
    
    void Success()
    {
        waitingForInput = false;
        Debug.Log("Dodged successfully!");
        // Add score, visual feedback, etc.
    }
    
    void Failed()
    {
        waitingForInput = false;
        Debug.Log("Hit! Take damage");
        // Reduce health, show damage effect, etc.
    }
}