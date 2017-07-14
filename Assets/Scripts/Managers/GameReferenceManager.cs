using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReferenceManager : MonoBehaviour {

    public static GameReferenceManager instance;
    public GameObject leftTopCorner, leftBottomCorner, rightTopCorner, rightButtomCorner,player,pausePanel,losePanel,winPanel;
    public Map map;
    public LineRenderer lr;
    public Movement movement;
    public Text loseLevel, loseScore, gameScreenLevelText, coveredAreaText, winLevel, winScore, gameScore;
    public Image star1, star2, star3;

    void Awake()
    {
        instance = this;
    }

}
