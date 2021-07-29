using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using DapperDino.Mirror.Tutorials.Lobby;

public class ColorButtonNetwork : MonoBehaviour
{
  

    public ColorStateCode currentColor;
    public int colorCode;

    public Button button { get;private set; }

    
    public bool IsPicked = false;

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }


    private void Awake()
    {
        
    }
    private void Start()
    {
        button = GetComponent<Button>();
        currentColor = new ColorStateCode(colorCode, Color.red, false);
    }
    private void Update()
    {
        if (!IsPicked)
        {
            button.interactable = true;
        }

        else
        {
            button.interactable = false;
        }
    }
}
