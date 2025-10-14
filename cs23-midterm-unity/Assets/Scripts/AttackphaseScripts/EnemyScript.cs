using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq.Expressions;

public class EnemyScript : MonoBehaviour
{


    public int maxHealth;
    public int currentHealth;
    public HealthBarScript healthBar;

    public BattleHandlerScript battleHandler;

    void Start()
    {
        maxHealth = HealthData.Instance.enemyMaxHP;
        currentHealth = HealthData.Instance.enemyHP;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthData.Instance.enemyHP -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
            battleHandler.playerWin();
        }
        healthBar.SetHealth(currentHealth);

    }

}
