using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuButton : MonoBehaviour {

    public int menuOption;

    public void MenuAction()
    {
        GameMenuManager.instance.MenuAction(menuOption);
    }
}
