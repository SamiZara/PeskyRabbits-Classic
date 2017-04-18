using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour {

    public static ReferenceManager instance;
    public GameObject windowLevelSelection, levelSelectionRow1, levelSelectionRow2, levelSelectionRow3, levelSelectionRow4, levelSelectionRow5;
    public Button levelSelectionLeftButton,levelSelectionRightButton;
    public Sprite lockedImage,unlockImage;

	void Awake()
    {
        instance = this;
    }
}
