using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour {

    public static GameMenuManager instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        GameReferenceManager.instance.gameScreenLevelText.text = "Level " + MenuManager.selectedLevel;
    }

    public void MenuAction(int option)
    {
        switch (option)
        {
            case 0://Puase game
                Time.timeScale = 0;
                GameReferenceManager.instance.pausePanel.SetActive(true);
                break;
            case 1://Contuniue game
                Time.timeScale = 1;
                GameReferenceManager.instance.pausePanel.SetActive(false);
                break;
            case 2://Return to menu
                SceneManager.LoadScene("MainMenu");
                break;
            case 3://Retry
                SceneManager.LoadScene("Game");
                break;
            case 4://Next Level
                MenuManager.selectedLevel += 1;
                SceneManager.LoadScene("Game");
                break;
        }
    }
}
