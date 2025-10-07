using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleHandlerScript : MonoBehaviour
{
    public Button attackButton;
    public CircleSpawner circleSpawner;
    public EnemyScript enemy;
    public int dmgperhit;
    public bool gamePaused;

    void Start()
    {

    }
    public void attackEnemy()
    {
        circleSpawner.StartSpawning(10, 0.6f, 1.2f);
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
