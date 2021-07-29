using UnityEngine;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class MainMenu : MonoBehaviour
    {
        private NetworkManagerLobby networkManager = null;
        private NewNetworkDiscovery networkDiscovery = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        private void Start()
        {
            
        }
        private void Update()
        {
            if(networkDiscovery == null)
                networkDiscovery = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkDiscovery>();
        }
        public void HostLobby()
        {
            StartCoroutine(networkDiscovery.StartHost());
            landingPagePanel.SetActive(false);
        }
    }
}
