using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject canvasKey;
    public GameObject canvasDoor;
    public Action onKeyZone; 
    public Action onDoorZone;
    [SerializeField] public TextMeshProUGUI doorText;

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


    }

    private void Start()
    {
        canvasKey = GameObject.FindWithTag("KeyText");
        canvasDoor = GameObject.FindWithTag("DoorText");
        canvasDoor.SetActive(false);
        canvasKey.SetActive(false);
        onKeyZone += KeyVisibility;
        onDoorZone += DoorVisibility;
    }

    public void KeyVisibility()
    {
        CanvasVisibility(canvasKey);
    }


    public void DoorVisibility()
    {
        CanvasVisibility(canvasDoor);
        if (playerHasKey)
        {
            doorText.text = "\"E\" to open the door";
        } else
        {
            doorText.text = "You need a key to open this door";
        }
    }

    public void CanvasVisibility(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }

    public void SetPlayerHasKey()
    {
        playerHasKey = true;
        KeyVisibility();
    }

}
