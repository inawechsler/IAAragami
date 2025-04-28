using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // IDs para canvas (en lugar de referencias directas)
    private const string KEY_CANVAS_ID = "KeyCanvas";
    private const string DOOR_CANVAS_ID = "DoorCanvas";
    public Action onKeyZone; 
    public Action onDoorZone;
    public Action<bool> onGameEnd;

    public bool playerHasKey;
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

    private void Start()
    {
        InitializeUIElements();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.RegisterAllUIElements();
        InitializeUIElements();
    }

    private void InitializeUIElements()
    {
        // Ocultar elementos UI al inicio
        UIManager.Instance.ToggleUI(KEY_CANVAS_ID);
        UIManager.Instance.HideUI(DOOR_CANVAS_ID);

        // Conectar eventos
        onKeyZone += KeyVisibility;
        onDoorZone += DoorVisibility;
        onGameEnd += OnGameEnding;
    }

    public void KeyVisibility()
    {
        UIManager.Instance.ToggleUI(KEY_CANVAS_ID);
    }
    public void OnGameEnding(bool value)
    {
        WinLoseScreenManager.SetText(value);
        SceneManager.LoadScene("WinScreen");
    }

    public void DoorVisibility()
    {
        UIManager.Instance.ToggleUI(DOOR_CANVAS_ID);

        // Actualizar texto según estado
        string doorMessage = playerHasKey ?
            "\"E\" to open the door" :
            "You need a key to open this door";

        UIManager.Instance.UpdateUIText(DOOR_CANVAS_ID, doorMessage);
    }

    public void SetPlayerHasKey()
    {
        playerHasKey = true;
        KeyVisibility();
    }

}
