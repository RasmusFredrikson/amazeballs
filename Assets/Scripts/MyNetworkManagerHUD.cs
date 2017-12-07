#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class MyNetworkManagerHUD : MonoBehaviour
    {
        public NetworkManager manager;
        [SerializeField] public bool showGUI = true;
        private int offsetX = 0;
        private int offsetY;

        private int scale;
        private GameObject debugUI;

        void Awake()
        {
            setBig();
            manager = GetComponent<NetworkManager>();
        }

        void setBig()
        {
            scale = 3;
            offsetY = 0;
        }

        void setSmall()
        {
            scale = 1;
            offsetY = 100;
        }

        void Update()
        {
            if (Input.GetKeyDown("z"))
            {
                showGUI = !showGUI;
                if (debugUI == null)
                    debugUI = GameObject.Find("Debug UI");
                debugUI.SetActive(showGUI);
            }

            if (!showGUI)
                return;

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    setSmall();
                    manager.StartServer();
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    setSmall();
                    manager.StartHost();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    setSmall();
                    manager.StartClient();
                }
            }
            if (NetworkServer.active && NetworkClient.active)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    setBig();
                    manager.StopHost();
                }
            }
        }

        void OnGUI()
        {
            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            int spacing = 24*scale*2;

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                /*if (GUI.Button(new Rect(xpos, ypos, 400, 40), "LAN Host(H)"))
                {
                    manager.StartHost();
                }
                ypos += spacing;
                */

                if (GUI.Button(new Rect(xpos, ypos, 200*scale, 40*scale), "LAN Client(C)"))
                {
                    setSmall();
                    manager.StartClient();
                }
                ypos += spacing;
                manager.networkAddress = GUI.TextField(new Rect(xpos, ypos, 200 * scale, 40 * scale), manager.networkAddress);
                ypos += spacing;

                

                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 40 * scale), "LAN Server Only(S)"))
                {
                    setSmall();
                    manager.StartServer();
                    
                }
                ypos += spacing;
            }
            else
            {
                if (NetworkServer.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 200 * scale, 40 * scale), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }
                if (NetworkClient.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 200 * scale, 40 * scale), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }

            if (NetworkClient.active && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 40 * scale), "Client Ready"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || NetworkClient.active)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 40 * scale), "Stop (X)"))
                {
                    setBig();
                    manager.StopHost();
                }
                ypos += spacing;
            }
        }
    }
};
#endif //ENABLE_UNET
