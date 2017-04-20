using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    private Image currentWindow;
    public static MenuManager instance;
    public static int selectedLevel,currentPage;
    int maxLevelAchieved = 25;
    int pageCount;
    void Start () {
        //int maxLevelAchieved = PlayerPrefs.GetInt("MaxLevel");
        int pageCount = maxLevelAchieved / 15;
        if(pageCount > 0)
        {
            ReferenceManager.instance.levelSelectionLeftButton.interactable = true;
            currentPage = pageCount;
        }
        GameObject currentRow = null;
        for(int i = 0; i < 15; i++)
        {
            int rowNumber = (i / 3) + 1;
            if(rowNumber == 1)
            {
                currentRow = ReferenceManager.instance.levelSelectionRow1;
            }
            else if(rowNumber == 2)
            {
                currentRow = ReferenceManager.instance.levelSelectionRow2;
            }
            else if (rowNumber == 3)
            {
                currentRow = ReferenceManager.instance.levelSelectionRow3;
            }
            else if (rowNumber == 4)
            {
                currentRow = ReferenceManager.instance.levelSelectionRow4;
            }
            else if (rowNumber == 5)
            {
                currentRow = ReferenceManager.instance.levelSelectionRow5;
            }
            GameObject currentItem = currentRow.transform.GetChild(i % 3).gameObject;
            if(i + pageCount * 15 <= maxLevelAchieved)
            {
                currentItem.transform.GetChild(0).GetComponent<Text>().text = (i + 1 + pageCount * 15).ToString();
            }
            else
            {
                currentItem.transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,0);
                currentItem.GetComponent<Image>().sprite = ReferenceManager.instance.lockedImage;
                currentItem.GetComponent<Button>().interactable = false;
            }
        }
	}

    void Awake()
    {
        instance = this;
    }
	
	public void MenuAction(int option)
    {
        switch (option)
        {
            case 0:
                ReferenceManager.instance.windowLevelSelection.gameObject.SetActive(true);
                break;
            case 1:
                ReferenceManager.instance.windowLevelSelection.gameObject.SetActive(false);
                break;
            case 2:
                Debug.Log(selectedLevel);
                SceneManager.LoadScene("Game");
                break;
            case 3:
                currentPage--;
                ReferenceManager.instance.levelSelectionRightButton.interactable = true;
                if (currentPage < 0)
                    ReferenceManager.instance.levelSelectionLeftButton.interactable = false;
                GameObject currentRow = null;
                for (int i = 0; i < 15; i++)
                {
                    int rowNumber = (i / 3) + 1;
                    if (rowNumber == 1)
                    {
                        currentRow = ReferenceManager.instance.levelSelectionRow1;
                    }
                    else if (rowNumber == 2)
                    {
                        currentRow = ReferenceManager.instance.levelSelectionRow2;
                    }
                    else if (rowNumber == 3)
                    {
                        currentRow = ReferenceManager.instance.levelSelectionRow3;
                    }
                    else if (rowNumber == 4)
                    {
                        currentRow = ReferenceManager.instance.levelSelectionRow4;
                    }
                    else if (rowNumber == 5)
                    {
                        currentRow = ReferenceManager.instance.levelSelectionRow5;
                    }
                    GameObject currentItem = currentRow.transform.GetChild(i % 3).gameObject;
                    if (i + currentPage * 15 <= maxLevelAchieved)
                    {
                        currentItem.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 1);
                        currentItem.GetComponent<Image>().sprite = ReferenceManager.instance.unlockImage;
                        currentItem.GetComponent<Button>().interactable = true;
                        currentItem.transform.GetChild(0).GetComponent<Text>().text = (i + 1 + pageCount * 15).ToString();
                    }
                }
                break;
            case 4:
                
                currentPage++;
                Debug.Log(currentPage);
                ReferenceManager.instance.levelSelectionLeftButton.interactable = true;
                if (currentPage >= pageCount)
                    ReferenceManager.instance.levelSelectionRightButton.interactable = false;
                GameObject currentRow1 = null;
                for (int i = 0; i < 15; i++)
                {
                    int rowNumber = (i / 3) + 1;
                    if (rowNumber == 1)
                    {
                        currentRow1 = ReferenceManager.instance.levelSelectionRow1;
                    }
                    else if (rowNumber == 2)
                    {
                        currentRow1 = ReferenceManager.instance.levelSelectionRow2;
                    }
                    else if (rowNumber == 3)
                    {
                        currentRow1 = ReferenceManager.instance.levelSelectionRow3;
                    }
                    else if (rowNumber == 4)
                    {
                        currentRow1 = ReferenceManager.instance.levelSelectionRow4;
                    }
                    else if (rowNumber == 5)
                    {
                        currentRow1 = ReferenceManager.instance.levelSelectionRow5;
                    }
                    GameObject currentItem = currentRow1.transform.GetChild(i % 3).gameObject;
                    if (i + currentPage * 15 > maxLevelAchieved)
                    {
                        currentItem.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 0);
                        currentItem.GetComponent<Image>().sprite = ReferenceManager.instance.lockedImage;
                        currentItem.GetComponent<Button>().interactable = false;
                    }
                    currentItem.transform.GetChild(0).GetComponent<Text>().text = (i + 1 + currentPage * 15).ToString();
                }
                break;
        }
    }
}
