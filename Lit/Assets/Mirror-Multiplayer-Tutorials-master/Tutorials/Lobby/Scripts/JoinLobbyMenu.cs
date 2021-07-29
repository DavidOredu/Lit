using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class JoinLobbyMenu : MonoBehaviour
    {
       private NetworkManagerLobby networkManager = null;
       private NewNetworkDiscovery newNetworkDiscovery = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private TMP_InputField ipAddressInputField = null;
        [SerializeField] private TextMeshProUGUI numberOfServersText = null;
        [SerializeField] private TextMeshProUGUI roomNameText = null;
        [SerializeField] private Button joinButton = null;
        [SerializeField] private Button leftButton = null;
        [SerializeField] private Button rightButton = null;

        private int index;
        private void Start()
        {

            
        }

        private void OnEnable()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
        }
        public void HostLobby()
        {
            StartCoroutine(newNetworkDiscovery.StartHost());
            landingPagePanel.SetActive(false);
        }
        private void Update()
        {
            if(newNetworkDiscovery == null)
                newNetworkDiscovery = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkDiscovery>();

            if (index == 0)
            {
                leftButton.interactable = false;
            }
            else
            {
                leftButton.interactable = true;
            }

            if(index == newNetworkDiscovery.discoveredServerCount() - 1 || newNetworkDiscovery.discoveredServerCount() == 0)
            {
                rightButton.interactable = false;
            }
            else
            {
                rightButton.interactable = true;
            }

            if(newNetworkDiscovery.discoveredServerCount() != 0)
            {
                newNetworkDiscovery.currentServer = newNetworkDiscovery.discoveredServersList()[index];
                joinButton.interactable = true;
            }
            

            if(newNetworkDiscovery.discoveredServerCount() == 1)
            {
                numberOfServersText.text = $"{newNetworkDiscovery.discoveredServerCount()} Room Found";
            }
            else
            {
                numberOfServersText.text = $"{newNetworkDiscovery.discoveredServerCount()} Rooms Found";
            }
            if(newNetworkDiscovery.discoveredServerCount() != 0)
            {
                roomNameText.gameObject.SetActive(true);
                roomNameText.text = $"{newNetworkDiscovery.currentServer.name}'s Room";
            }
            Debug.Log($"{newNetworkDiscovery.discoveredServerCount()}");
        }
        public void FindLobby()
        {
            //   string ipAddress = ipAddressInputField.text;
            newNetworkDiscovery.FindServers();
            
            
        }
        public void JoinLobby()
        {
            newNetworkDiscovery.Connect(newNetworkDiscovery.currentServer);
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }

        public void LeftButton()
        {
            index--;
        }
        public void RightButton()
        {
            index++;
        }
    }
}
