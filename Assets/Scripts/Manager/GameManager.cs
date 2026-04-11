using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action onKeyZone; 
    public Action onDoorZone;
    public Action<bool> onGameEnd;

    public bool playerIsOnDeathZone;
    public bool playerHasKey;
    public Canvas canvas;
    public TextMeshProUGUI canvasText;
    public Image image;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void Init()
    {

        canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
        //image = canvas.GetComponentInChildren<Image>();
        canvasText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        if (canvas == null)
        {
            return;
        }
        //canvasText = GameObject.FindWithTag("Canvas").GetComponentInChildren<TextMeshProUGUI>();
        InitializeUIElements();
        playerIsOnDeathZone = false;
        playerHasKey = false;
    }

    private void Start()
    {
        Init();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    private void InitializeUIElements()
    {
        image.gameObject.SetActive(false);



        onKeyZone += KeyVisibility; //Igualo porque es la unica función que se suscribe
        onDoorZone = DoorVisibility; //Igualo porque es la unica función que se suscribe
        onGameEnd += OnGameEnding;
    }
    public void KeyVisibility()
    {
        Debug.Log("kasjdas");
        string keyTextToUse = "Press \"E\" to pick up the key";
        ChangeUIVisibility(true, keyTextToUse);
    }
    public void OnGameEnding(bool value)
    {
        WinLoseScreenManager.SetText(value);
        SceneManager.LoadScene("WinScreen");
    }

    public void ChangeUIVisibility(bool value, string text = null)
    {
        if (value)
        {
            canvasText.text = text;
            image.gameObject.SetActive(true);
        } else
        {
            canvasText.text = "";
            image.gameObject.SetActive(false);
        }
    }
    public void DoorVisibility()
    {

        // Actualizar texto según estado
        string doorMessage = playerHasKey ?
            "\"E\" to open the door" :
            "You need a key to open this door";

        ChangeUIVisibility(true, doorMessage);

    }

    public void SetPlayerHasKey()
    {
        playerHasKey = true;
        canvasText.gameObject.SetActive(false);
    }

}
