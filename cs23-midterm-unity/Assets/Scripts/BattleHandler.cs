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
    public CircleSpawner circleSpawner;
    public EnemyScript enemy;
    public int dmgperhit;
    public int difficulty;
    public bool gamePaused;

    void Start()
    {
        backgroundImage = background.GetComponent<SpriteRenderer>();
        switch (StageData.selectedStage)
        {
            case "CafeMatcha":
                backgroundImage.sprite = cafeBG;
                difficulty = 1;
                break;
            case "Bench":
                backgroundImage.sprite = parkBG;
                difficulty = 2;
                break;
            case "VintageStore":
                backgroundImage.sprite = vintageBG;
                difficulty = 3;
                break;
            default:
                break;
        }

    }
    public void attackEnemy()
    {
        circleSpawner.StartSpawning(difficulty*10, 0.6f - (difficulty *0.1f), 1.2f - (difficulty*0.2f));
        attackButton.gameObject.SetActive(false);
    }

    public void attackFinished(int circlesHit)
    {
        int damage = circlesHit * dmgperhit;
        enemy.TakeDamage(damage);
        if (enemy.currentHealth > 0)
        {
            attackButton.gameObject.SetActive(true);
        }

    }

    public void playerWin()
    {
        attackButton.gameObject.SetActive(false);
        SceneManager.LoadScene("SceneWin");

    }
}
