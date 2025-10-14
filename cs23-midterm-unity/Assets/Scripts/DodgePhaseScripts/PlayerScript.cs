using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq.Expressions;

public class PlayerScript : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public HealthBarScript healthBar;
    public GameHandler gameHandler;


    void Start()
    {
        maxHealth = HealthData.Instance.playerMaxHP;
        currentHealth = HealthData.Instance.playerHP;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthData.Instance.playerHP -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameHandler.PlayerDie();
        }
        healthBar.SetHealth(currentHealth);

    }

}