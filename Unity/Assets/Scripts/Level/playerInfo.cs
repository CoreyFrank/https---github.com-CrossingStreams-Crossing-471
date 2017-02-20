using UnityEngine;
using System.Collections;

public class playerInfo : MonoBehaviour {

	private Room currentRoom;
	private GameObject currentRoomObject = null;
	
	public void buildRoom(){
		currentRoom.PathConnect ();
	}
	
	public void setCurrentRoom(Room room){
		currentRoom = room;
	}
	
	public void setBuilt(bool b){
		currentRoom.Built = b;
	}
	
	public void setCurrentRoomObject(GameObject room){
		currentRoomObject = room;
	}
	
	public bool getBuilt(){
		return currentRoom.Built;
	}
	
	public Room getCurrentRoom(){
		return currentRoom;
	}
	
	public GameObject getCurrentRoomObject(){
		return currentRoomObject;
	}

	// Use this for initialization
	void Start () {
		currentRoom = ScriptableObject.CreateInstance ("Room") as Room;
		currentRoom.setXY (0,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
