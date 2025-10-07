using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq.Expressions;

public class EnemyScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBarScript healthBar;

    public BattleHandlerScript battleHandler;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
            battleHandler.playerWin();
        }
        healthBar.SetHealth(currentHealth);

    }

}
