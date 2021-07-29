using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using Mirror.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


public struct DiscoveryRequest : NetworkMessage
{
    public string displayName;
    // Add properties for whatever information you want sent by clients
    // in their broadcast messages that servers will consume.
}

public struct DiscoveryResponse : NetworkMessage
{
    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }
    // Add properties for whatever information you want the server to return to
    // clients for them to display or consume for establishing a connection.
    public int totalPlayers;
    public string hostPlayerName;
    public Uri uri;
    public IPEndPoint EndPoint { get; set; }
    public long serverId;
    
    
    public void Set()
    {
        if(Room.RoomPlayers.Count == 0) { return; }
        hostPlayerName = Room.RoomPlayers[0].DisplayName;
        totalPlayers = Room.numPlayers;
    }
}
[Serializable]
public class ResponseFoundUnityEvent : UnityEvent<DiscoveryResponse> { };

public class NewNetworkDiscovery : NetworkDiscovery
{
    public ResponseFoundUnityEvent OnResponse;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    [ShowInInspector]
    readonly List<DiscoveryResponse> discoveryResponses = new List<DiscoveryResponse>();
    Vector2 scrollViewPos = Vector2.zero;
    private bool hasDiscovered = false;
    //  private bool hasGottenHost = false;
    
    // public long ServerId { get; private set; }
    Func<bool> hasGot;
    
    public NetworkDiscovery networkDiscovery;
    public ServerResponse currentServer;
    public Texture buttonTexture;

    private RoomPlayerLobby hostPlayer;
    private string ndisplayName = null;
    private int numberOfPlayers;

    private PlayerData playerData;

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
        playerData = Resources.Load<PlayerData>("PlayerData");
    }
    public override void Start()
    {
        base.Start();
        networkDiscovery.displayName = playerData.playerName;
    }
    private void FixedUpdate()
    {
        if(Room.RoomPlayers.Count == 0) { return; }
        hostPlayer = Room.RoomPlayers[0];
        numberOfPlayers = Room.RoomPlayers.Count;
    }
    public void FindServers()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }
   public int discoveredServerCount()
    {
        return discoveredServers.Count;
    }
    public IEnumerator StartHost()
    {
        discoveredServers.Clear();
        Room.StartHost();
        yield return new WaitUntil(hasGottenHost);
        
        
        ndisplayName = hostPlayer.DisplayName;
       // networkDiscovery.displayName = hostPlayer.DisplayName;
       // base.displayName = hostPlayer.DisplayName;
        networkDiscovery.AdvertiseServer();
    }
    public bool hasGottenHost()
    {
        if(hostPlayer == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void StartServer()
    {
        discoveredServers.Clear();
        Room.StartServer();

        networkDiscovery.AdvertiseServer();
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;

        hasDiscovered = true;


    }
    public List<ServerResponse> discoveredServersList() 
        {
        return discoveredServers.Values.ToList();
        } 

    void DrawButton()
    {
        scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);






        foreach (ServerResponse sInfo in discoveredServers.Values)
        {

            GUIContent content = new GUIContent(sInfo.name, buttonTexture);
            if (GUILayout.Button(content))
                Connect(sInfo);

            //  if(displayName == null) { return; }

        }
        GUILayout.EndScrollView();

    }
    private void OnGUI()
    {
        if (Room == null)
            return;

        if (NetworkServer.active || NetworkClient.active)
            return;

        //if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
        //    if (hasDiscovered)
        //        DrawButton();
    }
    public void Connect(ServerResponse info)
    {
        Room.StartClient(info.uri);
    }

    public void UpdateUI(DiscoveryResponse response)
    {
        discoveryResponses.Insert(discoveryResponses.Count, response);
        Debug.Log($"Has called Process Response. Will Invoke on response! Response player name is: {response.hostPlayerName}");
    }

    


    #region Server

    /// <summary>
    /// Reply to the client to inform it of this server
    /// </summary>
    /// <remarks>
    /// Override if you wish to ignore server requests based on
    /// custom criteria such as language, full server game mode or difficulty
    /// </remarks>
    /// <param name="request">Request comming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    protected override void ProcessClientRequest(ServerRequest request, IPEndPoint endpoint)
    {
        
        base.ProcessClientRequest(request, endpoint);
      //  ProcessRequest(request, endpoint);
    }

    /// <summary>
    /// Process the request from a client
    /// </summary>
    /// <remarks>
    /// Override if you wish to provide more information to the clients
    /// such as the name of the host player
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    /// <returns>A message containing information about this server</returns>
    protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {
        base.ProcessRequest(request, endpoint);
        return new ServerResponse()
        {
            
          //  name = networkDiscovery.displayName,
        //    totalPlayers = numberOfPlayers,
         //   uri = networkDiscovery.transport.ServerUri()
        };
    }

    #endregion

    #region Client

    /// <summary>
    /// Create a message that will be broadcasted on the network to discover servers
    /// </summary>
    /// <remarks>
    /// Override if you wish to include additional data in the discovery message
    /// such as desired game mode, language, difficulty, etc... </remarks>
    /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
    protected override ServerRequest GetRequest()
    {
        return new ServerRequest() 
        { 
            
        };
    }

    /// <summary>
    /// Process the answer from a server
    /// </summary>
    /// <remarks>
    /// A client receives a reply from a server, this method processes the
    /// reply and raises an event
    /// </remarks>
    /// <param name="response">Response that came from the server</param>
    /// <param name="endpoint">Address of the server that replied</param>
    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
    {
        base.ProcessResponse(response, endpoint);

      //  OnServerFound.Invoke(response);
        
    }

    #endregion
}

/*
	Discovery Guide: https://mirror-networking.com/docs/Guides/NetworkDiscovery.html
    Documentation: https://mirror-networking.com/docs/Components/NetworkDiscovery.html
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/


