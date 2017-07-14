using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public float totalArea,currentCoveredArea;
    public int stepCount;
    public int score;
    public Sprite filledStar;

	void Awake () {
        instance = this;
	}

    void Start()
    {
        Time.timeScale = 1;
        List<Vector2> edges = new List<Vector2>();
        //Debug.Log(MenuManager.selectedLevel);
    }

    public void FinishGame(bool isWin)
    {
        if (!isWin)
        {
            GameReferenceManager.instance.losePanel.SetActive(true);
            GameReferenceManager.instance.loseLevel.text = "Level " + MenuManager.selectedLevel;
            GameReferenceManager.instance.loseScore.text = "Score \n " + score;
            GameReferenceManager.instance.lr.startColor = new Color(1,0,0);
            GameReferenceManager.instance.lr.endColor = new Color(1,0,0);
            Time.timeScale = 0;
            SoundManager.instance.LoseGame();
        }
        else
        {
            GameReferenceManager.instance.winPanel.SetActive(true);
            GameReferenceManager.instance.winLevel.text = "Level " + MenuManager.selectedLevel;
            GameReferenceManager.instance.winScore.text = "Score \n " + score;
            Time.timeScale = 0;
            int starCount = PlayerPrefs.GetInt("StarLevel" + MenuManager.selectedLevel, 0);
            if (stepCount < 6)
            {
                GameReferenceManager.instance.star1.sprite = filledStar;
                GameReferenceManager.instance.star2.sprite = filledStar;
                GameReferenceManager.instance.star3.sprite = filledStar;
                if (starCount < 3)
                {
                    int totalStar = PlayerPrefs.GetInt("TotalStar",0);
                    PlayerPrefs.SetInt("StarLevel" + MenuManager.selectedLevel, 3);
                    PlayerPrefs.SetInt("TotalStar", totalStar + (3 - starCount));
                }
            }
            else if(stepCount < 10)
            {
                GameReferenceManager.instance.star1.sprite = filledStar;
                GameReferenceManager.instance.star2.sprite = filledStar;
                if (starCount < 2)
                {
                    int totalStar = PlayerPrefs.GetInt("TotalStar", 0);
                    PlayerPrefs.SetInt("StarLevel" + MenuManager.selectedLevel, 3);
                    PlayerPrefs.SetInt("TotalStar", totalStar + (2 - starCount));
                }
            }
            else
            {
                GameReferenceManager.instance.star1.sprite = filledStar;
                if (starCount < 1)
                {
                    int totalStar = PlayerPrefs.GetInt("TotalStar", 0);
                    PlayerPrefs.SetInt("StarLevel" + MenuManager.selectedLevel, 3);
                    PlayerPrefs.SetInt("TotalStar", totalStar + (1 - starCount));
                }
            }
            if (MenuManager.selectedLevel > PlayerPrefs.GetInt("MaxLevel", 0))
            {
                PlayerPrefs.SetInt("MaxLevel",MenuManager.selectedLevel);
            }
            AdManager.instance.LevelEnded();
            int highScore = PlayerPrefs.GetInt("HighScore",0);
            if(score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", score);

            }
        }
    }

    public void CoverArea(float area)
    {
        
        currentCoveredArea += area;
        int coveredAreaPercent = (int)(currentCoveredArea / (totalArea * 0.8f) * 100);
        if (coveredAreaPercent > 100)
            coveredAreaPercent = 100;
        GameReferenceManager.instance.coveredAreaText.text = "%" + coveredAreaPercent;
        //Debug.Log(area+","+ Mathf.Pow(MenuManager.selectedLevel, 0.5f)+","+ Mathf.Pow(stepCount, 0.25f));
        score += (int)(area * Mathf.Pow(MenuManager.selectedLevel, 0.5f) / (Mathf.Pow(stepCount,0.25f)) / 10);
        GameReferenceManager.instance.gameScore.text = score.ToString();
        if (coveredAreaPercent == 100)
            FinishGame(true);
        stepCount++;
    }
}
