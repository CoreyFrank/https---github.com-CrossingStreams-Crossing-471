using UnityEngine;
using System.Collections;
using System.Reflection;


public class RoomTrigger : MonoBehaviour {

	public Room room;
	public GameObject temp;
	public GameObject[] tileSet;
	//public bool active = false; 
	public bool built = false;
	public int PlayerCount;
	
	// Update is called once per frame
	void Update () {
		 if(room.PlayerCount == 0 && room.active){
			Invoke ("hideRoom", 0.5f);
		 	room.active = false;
		 	room.PlayerCount = 0;
		 }
		if(room.PlayerCount < 0)
			room.PlayerCount = 0;
		 PlayerCount = room.PlayerCount;
	}
	
	public void setRoom(Room r){
		room = r;
	}
	
	public Room getRoom(){
		return room;
	}
	
	[RPC]
	void updateNetworkRoom(string r, string n, float rnd){
		GameObject obj = GameObject.Find (n);
		RoomTrigger tag = obj.GetComponentInChildren <RoomTrigger>() as RoomTrigger;
		tag.room.fromString (r);
		tag.fillRoom(rnd);
		tag.instantiateRoom (tag.room.toString());
		tag.built = true;
	}  
	[RPC]
	void fillRoom(float rnd){
		// 4 pillar room
		float x = 12.5f;
		float y = 12.5f;
		if(room.getState () == "boss"){
			temp = Instantiate(tileSet[2], new Vector3(room.getX ()*50 , 1, room.getY ()*50 + 10),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;
		}
		if(rnd <= 1.5f){
			for(int i = 0; i < 2; i++){
				for(int j = 0; j < 2; j++){
					temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50 + x, -5, room.getY ()*50 + y),Quaternion.identity) as GameObject;
					temp.transform.parent = transform.parent;
					y *= -1;
				}
				x *= -1;
			}
		}
		// 6 pillar room
		if(rnd >100 && rnd <= 0){
			temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;
			temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;
			temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;
			temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;
			temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;
			temp = Instantiate(tileSet[1], new Vector3(room.getX ()*50, 0, room.getY ()*50),Quaternion.identity) as GameObject;
			temp.transform.parent = transform.parent;		
		}
	}
	
	[RPC]
	void incPlayerCount(){
		room.incPlayer();
	}
	
	[RPC]
	bool instantiateRoom(string r){
		GameObject temp;	
		room.fromString (r);
		int xi = 4;
		float l = (float) Mathf.Sqrt (room.layout.Length) -1;
		for(int x = 0; x <= l ; x++){
			int yi = 4;
			for(int y = 0; y <= l ; y++){
				if(room.layout[y,x] != ""){
					temp = Instantiate(tileSet[0], new Vector3(room.getX ()*50 - yi*10 +20, 0, 
					                                           room.getY ()*50 - xi*10 +20),Quaternion.identity) as GameObject;
					temp.transform.parent = transform.parent;
				}
				else{
					temp = Instantiate(tileSet[3], new Vector3(room.getX ()*50 - yi*10 +20, 5f, 
					                                           room.getY ()*50 - xi*10 +20),Quaternion.identity) as GameObject;
					temp.transform.parent = transform.parent;
				}
				yi--;
			}
			xi--;
		}
		return true;
	}
	
	void OnTriggerEnter(Collider other){
		float rng = Random.Range (1,100);
		if(other.gameObject.tag == "Player"){
			playerInfo info = other.gameObject.GetComponent<playerInfo>() as playerInfo;
			playerController control = other.gameObject.GetComponent<playerController>() as playerController;
			if(other.GetComponent<NetworkView>().isMine){					 					//player collides goes to new room
	//			control.setMoveDirection(Vector3.zero);
	//			control.setControllable (false);
				if(room.getState () == "boss" || room.getState () == "start" || room.getState () == "treasure"){
					room.PathFull();
					room.Built = true;
				}
				if(!room.Built){																					//build room if room not built
					room.PathConnect();
					room.Built = true;
					
				}  
				if(!built){
					instantiateRoom (room.toString());
					fillRoom(rng);
					//networkView.RPC ("instantiateRoom", RPCMode.AllBuffered, room.toString ());
				}
				room.active = true;
				if(info.getCurrentRoom () != null){
					info.getCurrentRoom ().decPlayer ();
				}
				room.incPlayer ();
				//networkView.RPC ("incPlayerCount", RPCMode.All, null);
				if(room != info.getCurrentRoom ()){
					if(info.getCurrentRoomObject () != null){
						temp = info.getCurrentRoomObject();

					}
					info.setCurrentRoom (room);
					info.setCurrentRoomObject (transform.parent.gameObject);
					if(!Camera.main.enabled)
						Camera.main.enabled = true;
					MouseOrbit orbit = Camera.main.GetComponent<MouseOrbit>();
					orbit.setTarget (gameObject.transform);
					if(!room.getBeaten() ){
					} 
					//showRoom ();
					GameObject mapCam = GameObject.FindGameObjectWithTag ("MapCamera") as GameObject;
					mapCam.transform.position = new Vector3(room.getX ()*50,mapCam.transform.position.y,room.getY ()*50);
					//Debug.Log ("Player in room" + room.getX () + "," + room.getY ());
				}
			}
			else {
				room.active = true;
				if(info.getCurrentRoom () != null && room != info.getCurrentRoom ()){
					info.getCurrentRoom ().decPlayer ();
					//info.setCurrentRoom (room);
					info.setCurrentRoom (room);
					info.setCurrentRoomObject (transform.parent.gameObject);
				}
				//if(room != info.getCurrentRoom ()){
					//showRoom();
				//}
			} 
			//networkView.RPC ("incPlayerCount", RPCMode.All, null);
			info = other.gameObject.GetComponent<playerInfo>() as playerInfo;
			showRoom ();
			//room.incPlayer ();
			built = true;
			GetComponent<NetworkView>().RPC ("updateNetworkRoom", RPCMode.OthersBuffered, room.toString (), this.transform.root.name, rng);
			
			
//			if(!built){
//				instantiateRoom (room.toString());
//				//networkView.RPC ("instantiateRoom", RPCMode.AllBuffered, room.toString ());
//				built = true;
//			}
//			Invoke ( control.setControllable (true);
		}
	}
	
	void setControllable(playerController control){
		control.setControllable (true);
	}
	
	void hideRoom(){
		Color type;
		if(room.getState () == "treasure"){
			type = Color.yellow;
			}
		else if(room.getState () == "boss"){
			type = Color.red;
			}
		else{
			type = Color.black;
			}
		foreach(MeshRenderer r in this.transform.parent.GetComponentsInChildren<MeshRenderer>()){
			if(r.gameObject.tag != "Map" && r.gameObject.tag != "MapWall")
				r.enabled = false;
			if(r.gameObject.tag == "Map"){
				r.GetComponent<Renderer>().material.color = type;
			}
			if(r.gameObject.tag == "MapWall"){
				r.GetComponent<Renderer>().material.color= Color.white;
			}
		}
		
	}
	
	void showRoom(){
		Color type;
		if(room.getState () == "treasure"){
			type = Color.yellow;
		}
		else if(room.getState () == "boss"){
			type = Color.red;
		}
		else{
			type = Color.green;
		}
		foreach(MeshRenderer r in transform.parent.GetComponentsInChildren<MeshRenderer>()){
			r.enabled = true;
			if(r.gameObject.tag == "Map"){
				r.GetComponent<Renderer>().material.color = type;
			}
		}
	}
}
