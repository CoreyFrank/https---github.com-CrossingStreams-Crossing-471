using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

//private string typeName = "RotFox-ProjectX";
//private string gameName = "RoomName";
//private HostData[] hostList;
public GameObject playerPrefab;
public GameObject enemyPrefab; 
public GameObject playerCameraPrefab;
public GameObject mapCameraPrefab;
public GameObject LevelGeneratorPrefab;
private bool started = false;


	void OnLevelWasLoaded(int level){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];
		if (players.Length > 0){
			//if(Camera.main != null)
			Instantiate (playerCameraPrefab, new Vector3 (0f,0f,0f),Quaternion.identity);
			Instantiate(mapCameraPrefab, new Vector3 (0f,0f,0f),Quaternion.AngleAxis (-90,Vector3.left));
			//if(GetComponent<NetworkView>().isMine){
			//Instantiate (LevelGeneratorPrefab, new Vector3(0f,0f,0f),Quaternion.identity);
			started = true;
			//}
		}
	}

	void OnAwake(){
		//DontDestroyOnLoad (transform.gameObject);
		
	}

	// Use this for initialization
	void onStart(){
		//MasterServer.ClearHostList ();
	}
	
	private void StartGame(){ 
		//GetComponent<NetworkView>().RPC("SpawnPlayer",RPCMode.AllBuffered,null);
		Instantiate (playerCameraPrefab, new Vector3 (0f,0f,0f),Quaternion.identity);
		Instantiate (mapCameraPrefab, new Vector3 (0f,0f,0f),Quaternion.AngleAxis (-90,Vector3.left));
		Instantiate (LevelGeneratorPrefab, new Vector3(0f,0f,0f),Quaternion.identity);
		started = true;
		//SpawnEnemy();
	}
	
	//private void StartServer () {
		//Network.InitializeServer (4, 25000, !Network.HavePublicAddress ());
		//MasterServer.RegisterHost (typeName,gameName);
		
		//LevelGeneratorScript.instance.loadMap();
	//}
	
	// Update is called once per frame
	//void OnServerInitialized () {
	//	Debug.Log ("Server Initialized");
	//}
	
	//private void RefreshHostList(){
	//	MasterServer.RequestHostList (typeName);
	//}
	
	//void OnMasterServerEvent(MasterServerEvent msEvent){
	//	if(msEvent == MasterServerEvent.HostListReceived )
	//		hostList = MasterServer.PollHostList ();
	//}
	
	//private void JoinServer(HostData hostData){
	//	Network.Connect (hostData);
	//	Instantiate (playerCameraPrefab, new Vector3 (0f,0f,0f),Quaternion.identity);
	//	Instantiate(mapCameraPrefab, new Vector3 (0f,0f,0f),Quaternion.AngleAxis (-90,Vector3.left));
	//}
	
	//void OnConnectedToServer(){
	//	Debug.Log ("Server Joined");
		//SpawnPlayer();
		
	//}
	
	[RPC]
	private void SpawnPlayer(){
		Network.Instantiate(playerPrefab, new Vector3(0f,3f,0f),Quaternion.identity,0);
		//cameraPrefab.AddComponent (Camera) ;
	}
	
	private void SpawnEnemy(){
		for(float x = 0; x < 4; x++){
			for(float z = 0; z < 4; z++){ 
			Network.Instantiate (enemyPrefab, new Vector3(x,5f,z),Quaternion.identity,0);
			}
		}
	}
	
	void OnGUI(){	
		//if (!Network.isClient && !Network.isServer){
			//gameName = GUI.TextField (new Rect(10,0,250,20), gameName, 25);
			if (GUI.Button (new Rect(10,25,250,20), "START Game"))
				StartGame();
			/*if (GUI.Button (new Rect(10,45,250, 20), "Refresh Hosts"))
				RefreshHostList();
			if (hostList != null){
				for(int i = 0; i < hostList.Length; i++){
					if (GUI.Button (new Rect(300,25 + (25 * i), 300, 20), hostList[i].gameName))
						JoinServer (hostList[i]);
				}
			}
		}
		if(Network.isServer && !started)
			if (GUI.Button (new Rect(10,10, 200, 20 ), "Start Game"))
				StartGame();*/

	}
	
	
}