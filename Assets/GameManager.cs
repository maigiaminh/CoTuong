using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //References
    [Header("Manager")]
    [SerializeField] private BoardManager boardManager;

    [Header("Timer")]
    [SerializeField] private TMP_Text p1TimerText;
    [SerializeField] private TMP_Text p2TimerText;


    [Header("End Game")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text[] endGameText;
    [SerializeField] private Image[] endGameImage;

    [Header("Dialog")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;

    [Header("Button")]
    [SerializeField] private Button p1Backward;
    [SerializeField] private Button p2Backward;
    [SerializeField] private Button p1Draw;
    [SerializeField] private Button p2Draw;
    [SerializeField] private Button p1Surrender;
    [SerializeField] private Button p2Surrender;

    private SettingManager settingManager;
    private float p1Time;
    private float p2Time;
    private int playerPressed;
    private int actionPressed;

    private void OnEnable() {
        p1Draw.onClick.AddListener(() => ButtonPress(1, 0));
        p2Draw.onClick.AddListener(() => ButtonPress(2, 0));
        p1Surrender.onClick.AddListener(() => ButtonPress(1, 1));
        p2Surrender.onClick.AddListener(() => ButtonPress(2, 1));
        p1Backward.onClick.AddListener(() => ButtonPress(1, 2));
        p2Backward.onClick.AddListener(() => ButtonPress(2, 2));
    }

    private void OnDisable() {
        p1Draw.onClick.RemoveAllListeners();
        p2Draw.onClick.RemoveAllListeners();
        p1Surrender.onClick.RemoveAllListeners();
        p2Surrender.onClick.RemoveAllListeners();
        p1Backward.onClick.RemoveAllListeners();
        p2Backward.onClick.RemoveAllListeners();
    }

    void Start()
    {
        settingManager = FindAnyObjectByType<SettingManager>();
        if(settingManager != null){
            if(settingManager.timer){
                p1Time = settingManager.p1Time;
                p2Time = settingManager.p2Time;
            }

            if(!settingManager.backward){
                p1Backward.gameObject.SetActive(false);
                p2Backward.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(settingManager != null){
            if(settingManager.timer){
                if(!boardManager.isEndGame){
                    if(boardManager.isRedTurn){
                        p1Time -= Time.deltaTime;
                        if (p1Time < 0)
                        {
                            p1Time = 0;
                            boardManager.isEndGame = true;
                            EndGame(2);
                        }
                    }
                    else{
                        p2Time -= Time.deltaTime;
                        if (p2Time < 0)
                        {
                            p2Time = 0;
                            boardManager.isEndGame = true;
                            EndGame(1);
                        }
                    }
                }
                

                UpdateTimeUI();
            }
        }
    }

    private void UpdateTimeUI()
    {
        p1TimerText.text = FormatTime(p1Time);
        p2TimerText.text = FormatTime(p2Time);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void EndGame(int pWin){
        StartCoroutine(EndGameAnimation());
        if(pWin == 0){
            endGameText[1].text = "Draw";
        }
        else if(pWin == 1){
            endGameText[1].text = "Player 1 win";
        }
        else if (pWin == 2){
            endGameText[1].text = "Player 2 win";
        }
    }

    private IEnumerator EndGameAnimation(){
        yield return new WaitForSeconds(1f);
        endGamePanel.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < 2)
        {
            elapsedTime += Time.deltaTime;
            foreach(Image image in endGameImage){
                Color color = image.color;
                color.a = Mathf.Clamp01(elapsedTime / 2);
                image.color = color;
            }
            
            foreach(TMP_Text text in endGameText){
                text.alpha = Mathf.Clamp01(elapsedTime / 2);
            }

            yield return null; 
        }
    }
    
    public void Rematch(){
        endGamePanel.SetActive(false);
        boardManager.isEndGame = false;
        if(settingManager != null){
            if(settingManager.timer){
                p1Time = settingManager.p1Time;
                p2Time = settingManager.p2Time;
            }

            if(!settingManager.backward){
                p1Backward.gameObject.SetActive(false);
                p2Backward.gameObject.SetActive(false);
            }
        }

        boardManager.DestroyAllPieces();
        boardManager.SpawnAllPieces();
        boardManager.PositionAllPieces();

    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("Menu Scene");
    }

    //0: Draw
    //1: Surrender
    //2: Move Backward
    public void ButtonPress(int player, int action){
        string pText = player == 1 ? "Player 1" : "Player 2";

        dialogPanel.SetActive(true);
        playerPressed = player;
        actionPressed = action;

        switch(action){
            case 0:
                dialogText.text = pText + " wants to draw";
                break;
            
            case 1:
                dialogText.text = "Are you sure you want to surrender?";
                break;

            case 2:
                dialogText.text = pText + " wants to go back to the previous move";
                break;
        }
    }

    public void Accept(){
        dialogPanel.SetActive(false);

        switch(actionPressed){
            case 0:
                boardManager.isEndGame = true;
                EndGame(0);
                break;
            
            case 1:
                boardManager.isEndGame = true;

                if(playerPressed == 1){
                    EndGame(2);
                }
                else{
                    EndGame(1);
                }

                break;
            case 2:
                boardManager.BackToPreviousMove();
                break;
        }

        playerPressed = -1;
        actionPressed = -1;
    }

    public void Decline(){
        dialogPanel.SetActive(false);

        playerPressed = -1;
        actionPressed = -1;
    }
}
