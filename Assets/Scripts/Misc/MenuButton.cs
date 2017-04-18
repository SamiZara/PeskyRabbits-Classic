using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuButton : MonoBehaviour
{

    public int menuOption;

    public void MenuAction()
    {
        if(menuOption == 2)
        {
            MenuManager.selectedLevel = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
        }
        MenuManager.instance.MenuAction(menuOption);
    }
}
