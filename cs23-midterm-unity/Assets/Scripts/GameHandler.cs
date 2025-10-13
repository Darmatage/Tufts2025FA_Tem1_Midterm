using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using Unity.VisualScripting;

public class GameHandler : MonoBehaviour{
    public static int playerStat1;

    public GameObject pauseMenu;
    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }


    public void StartGame()
    {
        SceneManager.LoadScene("StageSelect");
    }
    
    public void StartBattle() {
        SceneManager.LoadScene("attackphase");
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenCredits(){
        SceneManager.LoadScene("Credits");
    }

    public void RestartGame(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}