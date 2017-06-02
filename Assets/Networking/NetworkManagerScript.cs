using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {

	public enum GameState
	{
		menu,		//menu
		lobby,		//lobby as server or client
		ingame,		//ingame as server or client
	}

	GameState gamestate;

	float buttonX;
	float buttonY;
	float buttonW;
	float buttonH;
	bool refreshing = false;
	bool searched = false;
	HostData[] hostData; //this is the data stored from MasterServer.PollHostList()
	//http://docs.unity3d.com/ScriptReference/HostData.html


	Rect servers;
	Rect scroll;
	Vector2 scrollPosition = Vector2.zero;
	GUIStyle formatTitle = new GUIStyle();
	GUIStyle formatButton = new GUIStyle();

	public string gameType = "D2AFtest";

	public Transform pf;

	string gameName = "D2AF Test Server";
	string txtPort = "30000";
	int intPort = 30000;
	
	public static NetworkManagerScript nms;

	void Awake () {
		if (nms == null) {
			DontDestroyOnLoad (this);
			nms = this;
		} else if (nms != this) {
			Destroy (gameObject);
		}
	}



	// Use this for initialization
	void Start () {
		gamestate = GameState.menu;

		buttonX = Screen.width * 0.1f;
		buttonY = Screen.height * 0.4f;
		buttonW = Screen.width * 0.16f;
		buttonH = Screen.height * 0.055f;

		formatTitle.fontSize = (int)(Screen.height*0.08f);
		formatTitle.normal.textColor = Color.white;

		formatButton.fontSize = (int)(Screen.height*0.03f);
		formatButton.normal.textColor = Color.white;

		servers = new Rect(Screen.width * 0.55f,Screen.height * 0.4f,205,Screen.height * 0.3f);
		scroll = new Rect (0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (refreshing) {
			//MasterServer.RequestHostList(gameType);
			if (MasterServer.PollHostList ().Length > 0){
				Debug.Log (MasterServer.PollHostList().Length);
				//hostData = MasterServer.PollHostList ();
				searched = true;
				refreshing = false;
				scroll = new Rect (0, 0, 0, 20*MasterServer.PollHostList().Length);
			}
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log ("Disconnected from server: " + info);
		Destroy (this.gameObject);
		Application.LoadLevel (0);
		gamestate = GameState.menu;
	}

	//message
	void OnServerInitialized(){
		Debug.Log ("Server created");
		gamestate = GameState.lobby;
		spawnPlayer ();
	}

	void OnConnectedToServer(){
		Debug.Log ("Connected to server");
		gamestate = GameState.lobby;
		spawnPlayer ();
	}

	void OnMasterServerEvent(MasterServerEvent mse){
		if(mse == MasterServerEvent.RegistrationSucceeded){
			Debug.Log ("Registered Server");
		}
	}


	void createSinglePlayer(){
		gamestate = GameState.ingame;
		Application.LoadLevel(1);
	}
	void createServer(){
		//initialize
		intPort = int.Parse(txtPort);
		Debug.Log(Network.InitializeServer(1,intPort,!Network.HavePublicAddress()));
		MasterServer.RegisterHost(gameType,gameName,"test");
	}

	void findServers(){
		MasterServer.ClearHostList ();
		refreshing = true;
		MasterServer.RequestHostList(gameType);
		Debug.Log (MasterServer.PollHostList().Length);
	}

	void spawnPlayer(){
		Network.Instantiate (pf,Vector3.zero,pf.rotation,0);
	}

	//GUI
	void OnGUI(){
		if (gamestate==GameState.menu) {

			//Title
			GUI.Label (new Rect (((Screen.width / 2)) - Screen.width*0.079f, Screen.width * 0.1f, 50, 50), "D2AF",formatTitle);

			//click on single player
			if (GUI.Button (new Rect (buttonX, buttonY, buttonW, buttonH), "Single Player",formatButton)) {
				Debug.Log ("Creating Singleplayer");
				createSinglePlayer();
			}

			//click on find servers
//			if (GUI.Button (new Rect (buttonX, buttonY + Screen.height * 0.06f, buttonW, buttonH), "Find Servers",formatButton)) {
//				Debug.Log ("Finding Servers");
//				findServers ();
//			}
			/*if (GUI.Button (new Rect (buttonX + 110, buttonY + 40, buttonW, buttonH), "Stop search",formatButton)) {
				Debug.Log ("Stop search");
				refreshing = false;
			}*/
			//click on direct connect
//			if (GUI.Button (new Rect (buttonX, buttonY + Screen.height * 0.12f, buttonW, buttonH), "Direct Connect",formatButton)) {
//				Debug.Log ("Direct Connecting");
//
//			}
//			//click on create server
//			if (GUI.Button (new Rect (buttonX, buttonY + Screen.height * 0.18f, buttonW, buttonH), "Create Server",formatButton)) {
//				Debug.Log ("Creating Server");
//				createServer ();
//			}
//
//			gameName = GUI.TextField(new Rect(buttonX,buttonY + Screen.height * 0.24f,150,20),gameName,20);
//			txtPort = GUI.TextField(new Rect(buttonX+150,buttonY + Screen.height * 0.24f,50,20),txtPort,5);

			scrollPosition = GUI.BeginScrollView(servers,scrollPosition,scroll);
			hostData = MasterServer.PollHostList ();
			//display servers
			if (searched) {


				for (int i = 0; i< hostData.Length; i++) {
					if (GUI.Button (new Rect (0, 0+i*20, 190, 20), hostData [i].gameName + " [" + hostData [i].connectedPlayers + "/" + hostData [i].playerLimit+ "]" )) {
						Network.Connect (hostData [i]);
					}
				}

			}
			GUI.EndScrollView();
		}
	}
}
