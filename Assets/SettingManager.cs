using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }
    public bool timer;
    public float p1Time;
    public float p2Time;
    public bool redGoFirst;
    public bool backward;
    private void Awake() 
    {

        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this);

        } 
    }

    void Start()
    {
        FindAnyObjectByType<CanvasScaler>().GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
    }
}
