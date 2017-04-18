using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReferenceManager : MonoBehaviour {

    public static GameReferenceManager instance;
    public GameObject leftTopCorner, leftBottomCorner, rightTopCorner, rightButtomCorner;

    void Awake()
    {
        instance = this;
    }

}
