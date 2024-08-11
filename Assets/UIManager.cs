using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    //Panels
    [Header("Panels")]
    [SerializeField] private GameObject pvpSettingPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text goFirstText;
    [Header("Button")]
    [SerializeField] private Image blackImage;
    [SerializeField] private Image redImage;
    [Header("Image")]
    [SerializeField] private Sprite[] sides;
    //Time
    public int timeIndex = 0;
    private string[] time = { "OFF", "5", "10", "15", "20", "25", "30" };

    //Side
    public int currentSide = 0;
    //Go First
    private string[] go = { "Black", "Red" };
    public int currentGoFirst;
    private void Awake(){
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
        blackImage.sprite = sides[1];
        redImage.sprite = sides[2];
        currentGoFirst = 0;
    }

    public void OpenPanel(int panels){
        switch(panels){
            case 0:

                break;

            case 1:
                pvpSettingPanel.SetActive(true);
                break;

            case 2:

                break;

            default:
                break;
        }
    }

    public void ClosePanel(){
        pvpSettingPanel.SetActive(false);
    }

    public void ChangeTime(int index){
        timeIndex += index;

        if(timeIndex >= time.Length){
            timeIndex = 0;
        }
        else if(timeIndex < 0){
            timeIndex = time.Length - 1;
        }

        string text = time[timeIndex];
        if (timeIndex != 0){
            text += "   mins";
        }

        timeText.text = text;
    }

    public void ChangeSide(int side){
        switch (side){
            case 0: 
                currentSide = 0;
                blackImage.sprite = sides[1];
                redImage.sprite = sides[2];
                break;
            case 1:
                currentSide = 1;
                blackImage.sprite = sides[0];
                redImage.sprite = sides[3];
                break;
            default:
                break;
        }
    }

    public void ChangGoFirst(int goFirst){
        currentGoFirst += goFirst;
        if(currentGoFirst < 0){
            currentGoFirst = 1;
        }
        if(currentGoFirst > 1){
            currentGoFirst = 0;
        }
        
        goFirstText.text = go[currentGoFirst];
    }

    public void StartPvP(){
        if(timeIndex != 0){
            gameManager.timer = true;
        }
        else{
            gameManager.timer = false;
        }
        SceneManager.LoadScene("Main Scene");
    }
}
