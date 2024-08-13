using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] SettingManager settingManager;
    //Panels
    [Header("Panels")]
    [SerializeField] private GameObject pvpSettingPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text backwardText;
    [Header("Button")]
    [SerializeField] private Image blackImage;
    [SerializeField] private Image redImage;
    [Header("Image")]
    [SerializeField] private Sprite[] sideSprites;
    //Time
    public int timeIndex = 0;
    private string[] time = { "OFF", "5", "10", "15", "20", "25", "30" };

    //Move First
    public bool redGoFirst;
    
    //Backward
    public bool backward = false;
    private void Awake(){
        redImage.sprite = sideSprites[1];
        blackImage.sprite = sideSprites[2];
        redGoFirst = true;
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

    public void MoveFirst(int selected){
        if(selected == 0){
            redGoFirst = false;
            redImage.sprite = sideSprites[0];
            blackImage.sprite = sideSprites[3];
        }
        else if (selected == 1){
            redGoFirst = true;
            redImage.sprite = sideSprites[1];
            blackImage.sprite = sideSprites[2];
        }
    }

    public void Backward(){
        backward = !backward;
        backwardText.text = backward ? "ON" : "OFF";
    }

    public void StartPvP(){

        //Time
        if(timeIndex != 0){
            settingManager.timer = true;
            settingManager.p1Time = float.Parse(time[timeIndex]) * 60;
            settingManager.p2Time = float.Parse(time[timeIndex]) * 60;
        }
        else{
            settingManager.timer = false;
            settingManager.p1Time = 0;
            settingManager.p2Time = 0;
        }

        //Move First
        settingManager.redGoFirst = redGoFirst;

        //Enable Backward
        settingManager.backward = backward;

        SceneManager.LoadScene("Main Scene");
    }
}
