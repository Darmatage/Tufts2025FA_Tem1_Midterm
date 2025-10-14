using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleHandlerScript : MonoBehaviour
{

    public GameObject background;
    private SpriteRenderer backgroundImage;
    public Sprite parkBG;
    public Sprite cafeBG;
    public Sprite vintageBG;

    public Button attackButton;
    public Button dodgeButton;
    public CircleSpawner circleSpawner;
    public EnemyScript enemy;
    public int dmgperhit;
    private int difficulty;
    public bool gamePaused;

    void Start()
    {
        difficulty = StageData.stageDifficulty;
        backgroundImage = background.GetComponent<SpriteRenderer>();
        switch (StageData.selectedStage)
        {
            case "CafeMatcha":
                backgroundImage.sprite = cafeBG;
                break;
            case "Bench":
                backgroundImage.sprite = parkBG;
                break;
            case "VintageStore":
                backgroundImage.sprite = vintageBG;
                break;
            default:
                break;
        }

    }
    public void attackEnemy()
    {
        circleSpawner.StartSpawning(difficulty*10, 0.6f - (difficulty *0.13f), 1.2f - (difficulty*0.22f));
        attackButton.gameObject.SetActive(false);
    }

    public void attackFinished(int circlesHit)
    {
        int damage = circlesHit * dmgperhit;
        enemy.TakeDamage(damage);
        if (enemy.currentHealth > 0)
        {
            dodgeButton.gameObject.SetActive(true);
        }
    }

    public void dodge()
    {
        SceneManager.LoadScene("dodgePhase");
    }

    public void playerWin()
    {
        attackButton.gameObject.SetActive(false);
        SceneManager.LoadScene("SceneWin");

    }
}
