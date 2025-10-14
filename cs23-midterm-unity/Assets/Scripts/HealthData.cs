using UnityEngine;

public class HealthData : MonoBehaviour
{
    public static HealthData Instance;
    public int playerHP;
    public int enemyHP;
    public int playerMaxHP = 100;
    public int enemyMaxHP;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
