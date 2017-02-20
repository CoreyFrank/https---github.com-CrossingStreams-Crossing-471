using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class LevelGeneratorScript : MonoBehaviour{

	public static LevelGeneratorScript instance;
	private const int step = 10;
	public GameObject[] tileSet;
	public GameObject level;
	public List<GameObject> rooms;
	private MazeMap map;
	public RoomTrigger roomTrigger; 
	public bool created = false; 
	
	void Awake(){
		//DontDestroyOnLoad (transform.gameObject);
	}
	
	void Start () {
		bool works = false;
		Camera.main.enabled = true;
		//if(GetComponent<NetworkView>().isMine){
			while(!works){

				map = ScriptableObject.CreateInstance ("MazeMap") as MazeMap;
				map.initializeMap (step, tileSet);
                map.onEnable();
				//created = true;
				works = defineRooms ();
			}
			created = true;
			int i = 0;
			foreach(LinkedRoom r in map.rooms){
				if(r.getRoom ().getState () != "null"){
					//GetComponent<NetworkView>().RPC ("loadMap", RPCMode.AllBuffered, r.getRoom ().toString(),i);
					i++;  
				}
			}
		//} 
//		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];
//		foreach(GameObject p in players){
//			playerController control = p.GetComponent<playerController>() as playerController;
//			control.enabled = true;
//		}
	}
	
	void OnLevelWasLoaded(){
		//if(GetComponent<NetworkView>().isMine){
			bool works = false;
			int i = 0;
			while(!works){
				map = ScriptableObject.CreateInstance ("MazeMap") as MazeMap;
				map.initializeMap (step,tileSet);
				created = true;
				works = defineRooms();
			}
			foreach(LinkedRoom r in map.rooms){
				//map.initializeNeighborRooms (r);
				if(r.getRoom ().getState () != "null"){
					GetComponent<NetworkView>().RPC ("loadMap", RPCMode.AllBuffered, r.getRoom ().toString(),i);
					i++;  
				}
			}
		//}
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];
		foreach(GameObject p in players){
			playerController control = p.GetComponent<playerController>() as playerController;
			control.enabled = true;
		}
	}
	
	bool defineRooms(){ 
		int rng;
		//bool treasure = false;
		//bool boss = false;
		List<LinkedRoom> OneDoorRooms= new List<LinkedRoom>();
		foreach(LinkedRoom r in map.rooms){
			map.initializeNeighborRooms (r);
			if(r.getX () == 0 && r.getY() == 0)
				r.setState("start");
			if(r.getRoom ().getState () != "null"){
				List<string> doors = new List<string>();
				if(r.getLeft() != "null"){
					doors.Add ("left");
				}
				if(r.getRight() != "null"){
					doors.Add ("right");
				}
				if(r.getUp() != "null"){
					doors.Add ("up");
				}
				if(r.getDown() != "null"){
					doors.Add ("down");
				}
				
				if(doors.Count == 1){
					OneDoorRooms.Add (r);
				}
			}
			
		}
		if(OneDoorRooms.Count <= 2)
			return false;
		rng = Random.Range (1, OneDoorRooms.Count);
		OneDoorRooms[rng].setState ("treasure");
		OneDoorRooms.RemoveAt (rng);
		OneDoorRooms.TrimExcess();
		rng = Random.Range (1, OneDoorRooms.Count);
		OneDoorRooms[rng].setState ("boss");
		return true;
	}
	
	[RPC]
	private void loadMap(string s, int i){
		int x = i;
		Room room = ScriptableObject.CreateInstance ("Room") as Room;
		room.fromString (s);
		if(room.getState() == "active" || room.getState() == "treasure" || room.getState() == "boss" || room.getState() == "start"){
			GameObject temp = null;
			rooms.Add (tileSet[10]);
			rooms[x].name = "Room: " + x.ToString ();
			if(room.getX () == 0 && room.getY () == 0){
				room.setBeaten (true);
				//GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera") as GameObject;
				MouseOrbit orbit = Camera.main.GetComponent<MouseOrbit>();
				orbit.setTarget (rooms[x].gameObject.transform);
			}
			else
				room.setBeaten (false);
			rooms[x] = Instantiate (rooms[x], new Vector3(0,0,0),Quaternion.identity) as GameObject;
			temp = Instantiate(tileSet[0], new Vector3(room.getX ()*50, -15f, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = rooms[x].transform;
			temp = Instantiate(tileSet[7], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = rooms[x].transform;
			temp = tileSet[5];
			roomTrigger = temp.GetComponent("RoomTrigger") as RoomTrigger;
			roomTrigger.setRoom (room);
			
			temp = Instantiate(temp, new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = rooms[x].transform;
			
			//if(room.ge != null){	
				if((room.getLeft () == "null")){
					
					temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,270f,0f)) as GameObject;
					
					temp.transform.parent = rooms[x].transform;
					temp = Instantiate(tileSet[8], new Vector3(room.getX ()*50-50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
				else{
					
					temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,270f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp =Instantiate(tileSet[9], new Vector3(room.getX ()*50-50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
					
				}
			//}
			
			//if(room.getRoom ("right") != null){
				if((room.getRight () == "null")){
					
					temp =Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp = Instantiate(tileSet[8], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
				else{
					
					temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp = Instantiate(tileSet[9], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
			//}
			//if(room.getRoom ("up") != null){
				if((room.getUp () == "null")){
					
				temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,180f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp =Instantiate(tileSet[8], new Vector3(room.getX ()*50, -940, room.getY ()*50-50),Quaternion.identity) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
				else{
					
				temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,180f,0f)) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp = Instantiate(tileSet[9], new Vector3(room.getX ()*50, -940, room.getY ()*50-50),Quaternion.identity) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
				
			//}
			//if(room.getRoom ("down") != null){
				if((room.getDown () == "null")){
					
					temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp = Instantiate(tileSet[8], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.identity) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
				else{
					
					temp =Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
					temp.transform.parent = rooms[x].transform;
					temp = Instantiate(tileSet[9], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.identity) as GameObject;
					temp.transform.parent = rooms[x].transform;
				}
			//}
			//rooms[x].transform.parent = level.transform;
			if(!room.getBeaten ()){
				foreach(MeshRenderer r in rooms[x].GetComponentsInChildren<MeshRenderer>())
					if(r.gameObject.name != "MapRoom")
						r.enabled = false;
			}
			x++;
		}
		
}	
//	private void loadMap(byte[] arrBytes){
//	int x = 0;
//	//	MazeMap m = ScriptableObject.CreateInstance ("MazeMap") as MazeMap;
//	//	m =(MazeMap) m.Deserialize(arrBytes);
//	//	foreach(LinkedRoom room in map.rooms){
//	//		map.initializeNeighborRooms(room);
//		Room room = ScriptableObject.CreateInstance ("Room") as Room;
//		room = room.Deserialize(arrBytes);
//			if(room.getState() == "active"){
//				GameObject temp = null;
//				rooms.Add (tileSet[10]);
//				//rooms[x] = tileSet[10];
//				rooms[x].name = "room: " + x.ToString ();
//				//rooms[x].AddComponent<NetworkView>();
//				if(room.getRoom ().getX () == 0 && room.getRoom ().getY () == 0){
//					room.getRoom ().setBeaten (true);
//					MouseOrbit orbit = Camera.main.GetComponent<MouseOrbit>();
//					orbit.setTarget (rooms[x].gameObject.transform);
//				}
//				else
//					room.getRoom ().setBeaten (false);
//					
//				
//				
////				temp = tileSet[0];
////				temp.transform.parent = rooms[x].transform;
////				temp = tileSet[7];
////				temp.transform.position = new Vector3(0,10,0);
////				temp.transform.parent = rooms[x].transform;
////				temp = tileSet[5];
////				roomTrigger = temp.GetComponent("RoomTrigger") as RoomTrigger;
////				roomTrigger.setRoom (room);
////				temp.transform.parent = rooms[x].transform;
////				
////				rooms[x] = Network.Instantiate (temp, new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity,0) as GameObject;
//
//				rooms[x] = Instantiate (rooms[x], new Vector3(0,0,0),Quaternion.identity) as GameObject;
//				temp = Instantiate(tileSet[0], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
//				temp.transform.parent = rooms[x].transform;
//				temp = Instantiate(tileSet[7], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.identity) as GameObject;
//				temp.transform.parent = rooms[x].transform;
//				temp = tileSet[5];
//				roomTrigger = temp.GetComponent("RoomTrigger") as RoomTrigger;
//				roomTrigger.setRoom (room);
//				temp = Instantiate(temp, new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
//				temp.transform.parent = rooms[x].transform;
//			
//				if(room.getRoom ("left") != null){	
//					if((room.getRoom ("left").getState () == "null")){
//					
//						temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50-50, 0, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp = Instantiate(tileSet[8], new Vector3(room.getX ()*50-50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//					else{
//
//						temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50-50, 0, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp =Instantiate(tileSet[9], new Vector3(room.getX ()*50-50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//
//					}
//				}
//				
//				if(room.getRoom ("right") != null){
//					if((room.getRoom ("right").getState () == "null")){
//
//						temp =Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp = Instantiate(tileSet[8], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//					else{
//
//						temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp = Instantiate(tileSet[9], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.Euler (0f,90f,0f)) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//				}
//				if(room.getRoom ("up") != null){
//					if((room.getRoom ("up").getState () == "null")){
//
//						temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50-50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp =Instantiate(tileSet[8], new Vector3(room.getX ()*50, -940, room.getY ()*50-50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//					else{
//					
//						temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50-50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp = Instantiate(tileSet[9], new Vector3(room.getX ()*50, -940, room.getY ()*50-50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//
//				}
//				if(room.getRoom ("down") != null){
//					if((room.getRoom ("down").getState () == "null")){
//
//						temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp = Instantiate(tileSet[8], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//					else{
//
//						temp =Instantiate(tileSet[3], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//						temp = Instantiate(tileSet[9], new Vector3(room.getX ()*50, -940, room.getY ()*50),Quaternion.identity) as GameObject;
//						temp.transform.parent = rooms[x].transform;
//					}
//				}
//				//rooms[x].transform.parent = level.transform;
//				if(!room.getRoom ().getBeaten ()){
//					foreach(MeshRenderer r in rooms[x].GetComponentsInChildren<MeshRenderer>())
//						if(r.gameObject.name != "MapRoom")
//							r.enabled = false;
//				}
//				x++;
//			}
//		}
//	}	
	
	void Update () {
		
	}
}
