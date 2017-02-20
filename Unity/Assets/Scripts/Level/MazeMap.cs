using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable()]
public class MazeMap : ScriptableObject{ 

		  
	public int step;
    public GameObject[] TileSet;
    private LinkedRoom lRoom;

    public string type = null;
	Dictionary<string, LinkedRoom> linkedRoomDictionary = new Dictionary<string, LinkedRoom>();
	public List<LinkedRoom> rooms = new List<LinkedRoom>();
  
  	public string toString(){
  		string result = "";
  		result += step.ToString ();
  		return result;
  	}
  
  	public List<LinkedRoom> getRooms(){
  		return rooms;
  	}
  	
  	public void setRooms(List<LinkedRoom> r){
  		rooms = r;
  	}
  	
  	public Dictionary<string, LinkedRoom> getLinkedRoomDictionary(){
  		return linkedRoomDictionary;
  	}
  	
  	public void setLinkedRoomDictionary(Dictionary<string, LinkedRoom> r ){
  		linkedRoomDictionary = r;
  	}
  
//	public byte[] serialize(Object obj){
//		if(obj == null)
//			return null;
//		BinaryFormatter bf = new BinaryFormatter();
//		MemoryStream ms = new MemoryStream();
//
//		bf.Serialize (ms, obj);
//		return ms.ToArray ();
//		
//	}
//	
//	public MazeMap Deserialize(byte[] arrBytes){
//		MemoryStream memStream = new MemoryStream();
//		BinaryFormatter binForm = new BinaryFormatter();
//		memStream.Write (arrBytes,0,arrBytes.Length);
//		memStream.Seek (0,SeekOrigin.Begin);
//		MazeMap obj = (MazeMap) binForm.Deserialize (memStream);
//		return obj;
//	}

//	public void updateRooms(){
//		foreach(LinkedRoom r in rooms){
//			r.updateRoom();
//		}
//	}

//	static XmlSerializer serializer = new XmlSerializer(typeof(MazeMap));
//
//	public string Serialize(){
//		StringBuilder builder = new StringBuilder();
//		
//		serializer.Serialize (
//			System.Xml.XmlWriter.Create (builder),
//			this);
//		return builder.ToString ();
//	}
//	
//	public static MazeMap Deserialize(string serializedData){
//		return serializer.Deserialize (new StringReader(serializedData)) as MazeMap;
//	}  

    

	public void initializeMap(int s, GameObject[] tileSet){
        step = s;
        TileSet = tileSet;
        //generateMap (tileSet);
    }

    public void onEnable()
    {
        lRoom = CreateInstance("LinkedRoom") as LinkedRoom; //setup initial room
        lRoom.setXY(0, 0);
        lRoom.setState("created");
        oneNullMaze(1, null, lRoom, null, TileSet);
    }

	public void generateMap(GameObject[] tileSet){
		LinkedRoom lRoom = ScriptableObject.CreateInstance ("LinkedRoom") as LinkedRoom;	//setup initial room
		lRoom.setXY (0,0);
		lRoom.setState ("created");
		oneNullMaze(step, null, lRoom, null, tileSet);
	}
	
	public void oneNullMaze(int steps, string previousDir, LinkedRoom currentRoom, LinkedRoom previousRoom, GameObject[] tileSet){
		
		int rng;
		int roomsExists = 0;
		bool nulled = false;
		List<string> createdRooms = new List<string>();
		
		int x = currentRoom.getX ();
		int y = currentRoom.getY();
		
		if(currentRoom.getState () == "active"){
			initializeNeighborRooms (currentRoom);
			return;
		}
		
		if(currentRoom.getState () == "null" ){
			previousRoom.setLinkedRoom (previousDir,currentRoom);
			linkedRoomDictionary.Remove (x.ToString() + "," + y.ToString ());
			linkedRoomDictionary.Add (x.ToString() + "," + y.ToString (), currentRoom);
			initializeNeighborRooms (currentRoom);
			return;
		}
		if(steps >= 0){
			currentRoom.setState ("active");
		}
		else{
			currentRoom.setState ("null");
			previousRoom.setLinkedRoom (previousDir,currentRoom);
			initializeNeighborRooms (currentRoom);
			return;
		}
		
		if(previousRoom != null){
			currentRoom.getRoom ().setBeaten(true);
			previousRoom.setLinkedRoom (previousDir,currentRoom);
			}
		

		if(!testKey(x,y)){
			linkedRoomDictionary.Add (x.ToString() + "," + y.ToString (), currentRoom);
			rooms.Add (currentRoom);
		}
		else{
			linkedRoomDictionary.Remove (x.ToString() + "," + y.ToString ());
			linkedRoomDictionary.Add (x.ToString() + "," + y.ToString (), currentRoom);
			rooms.Add (currentRoom);
			
		}
		
		
		
		LinkedRoom left;															//setup new paths
		LinkedRoom right;
		LinkedRoom up;
		LinkedRoom down;
																					//go through dictionary and link linked rooms
		if(testKey (x-1,y)){														//to entries if they exist or create new instances
			left = getFromDictionary (x-1,y);										//if they don't exist
			roomsExists++;
			left.setLinkedRoom("right", currentRoom);
			if(left.getState () == "null"){
				nulled = true;
				currentRoom.incWalls ();	
			}
		}
		else{
			left = ScriptableObject.CreateInstance ("LinkedRoom") as LinkedRoom;
			left.setState("created");
			left.setXY (x-1,y);
			createdRooms.Add ("left");
			linkedRoomDictionary.Add (left.getX ().ToString() + "," + left.getY ().ToString (), left);
		}

		if(testKey (x+1, y)){
			right = getFromDictionary (x+1,y);
			roomsExists++;
			right.setLinkedRoom("left", currentRoom);
			if(right.getState () == "null"){
				nulled = true;
				currentRoom.incWalls ();
			}
		}
		else{
			right = ScriptableObject.CreateInstance ("LinkedRoom") as LinkedRoom;
			right.setState ("created");
			right.setXY (x+1,y);
			createdRooms.Add ("right");
			linkedRoomDictionary.Add (right.getX ().ToString() + "," + right.getY ().ToString (), right);
		}
		
		if(testKey (x,y-1)){
			up = getFromDictionary (x,y-1);
			roomsExists++;
			up.setLinkedRoom("down", currentRoom);
			if(up.getState () == "null"){
				nulled = true;
				currentRoom.setUp ("solid");
				currentRoom.incWalls ();	
			}
		}
		else{
			up = ScriptableObject.CreateInstance ("LinkedRoom") as LinkedRoom;
			up.setState ("created");
			up.setXY (x,y-1);
			createdRooms.Add ("up");
			linkedRoomDictionary.Add (up.getX ().ToString() + "," + up.getY ().ToString (), up);
		}

		if(testKey (x,y+1)){
			down = getFromDictionary (x,y+1);
			roomsExists++;
			down.setLinkedRoom("up", currentRoom);
			if(down.getState () == "null"){
				nulled = true;
				currentRoom.setDown ("solid");
				currentRoom.incWalls ();
			}
		}
		else{
			down = ScriptableObject.CreateInstance ("LinkedRoom") as LinkedRoom;
			down.setState ("created");
			down.setXY (x,y+1);
			createdRooms.Add ("down");
			linkedRoomDictionary.Add (down.getX ().ToString() + "," + down.getY ().ToString (), down);
		}
		
		if (roomsExists >= 4)
			return;
			
		if(!nulled){							//null room doesn't exists
			rng = Random.Range (1,createdRooms.Count);
			rng--;
			if(rng < 0)
				return;
			switch(createdRooms[rng]){
			case "left":
				left.setState ("null");
				rooms.Add (left);
				linkedRoomDictionary.Remove (left.getX () + "," + left.getY ());
				linkedRoomDictionary.Add (left.getX () + "," + left.getY (), left);
				break;
			case "right":
				right.setState ("null");
				rooms.Add (right);
				linkedRoomDictionary.Remove (right.getX () + "," + right.getY ());
				linkedRoomDictionary.Add (right.getX () + "," + right.getY (), right);
				break;
			case "up":
				up.setState ("null");
				rooms.Add (up);
				currentRoom.setUp ("solid");
				linkedRoomDictionary.Remove (up.getX () + "," + up.getY ());
				linkedRoomDictionary.Add (up.getX () + "," + up.getY (), up);
				break;
			case "down":
				down.setState ("null");
				rooms.Add (down);
				linkedRoomDictionary.Remove (down.getX () + "," + down.getY ());
				linkedRoomDictionary.Add (down.getX () + "," + down.getY (), down);
				break;
			}
			createdRooms.TrimExcess ();
		}
		
		
		foreach(string createdRoom in createdRooms){
			switch(createdRoom){
			case "left":
				oneNullMaze(steps -1, "left", left, currentRoom, tileSet);
				break;
			case "right":
				oneNullMaze(steps -1, "right", right, currentRoom, tileSet);
				break;
			case "up":
				oneNullMaze(steps -1, "up", up, currentRoom, tileSet);
				break;
			case "down":
				oneNullMaze(steps -1, "down", down, currentRoom, tileSet);
				break;
			}
		}
		return;
	}
	
	public void createWallNullRoom(string previousDirection, LinkedRoom currentRoom, GameObject[] tileSet){
		switch(previousDirection){
		case "up":
			Instantiate(tileSet[2], new Vector3(currentRoom.getX ()*50, 0, currentRoom.getY ()*50),Quaternion.identity);
			break;
		case "down":
			Instantiate(tileSet[2], new Vector3(currentRoom.getX ()*50, 0, currentRoom.getY ()*50-50),Quaternion.identity);
			break;
		case "left":
			Instantiate(tileSet[2], new Vector3(currentRoom.getX ()*50, 0, currentRoom.getY ()*50),Quaternion.Euler (0f,90f,0f));
			break;
		case "right":
			Instantiate(tileSet[2], new Vector3(currentRoom.getX ()*50-50, 0, currentRoom.getY ()*50),Quaternion.Euler (0f,90f,0f));
			break;
		}
	}
	
	public void initializeNeighborRooms(LinkedRoom currentRoom){
		LinkedRoom left;															//setup new paths
		LinkedRoom right;
		LinkedRoom up;
		LinkedRoom down;
		int x = currentRoom.getX (), y = currentRoom.getY ();
								 													//go through dictionary and link linked rooms
		if(testKey (x-1,y)){														//to entries if they exist or create new instances
			currentRoom.getRoom ().setLeft (getFromDictionary (x-1,y).getState());
			left = getFromDictionary (x-1,y);										//if they don't exist
			left.setLinkedRoom("right", currentRoom);
			currentRoom.setLinkedRoom ("left", left);	
			currentRoom.getRoom ().Left = left.getState ();
			}
		else{
			currentRoom.getRoom ().Left = null;
		}
		
		if(testKey (x+1, y)){
			right = getFromDictionary (x+1,y);
			currentRoom.getRoom ().setRight (getFromDictionary (x+1,y).getState());
			right.setLinkedRoom("left", currentRoom);
			currentRoom.setLinkedRoom ("right", right);
			currentRoom.getRoom ().Right = right.getState ();
		}
		else{
			currentRoom.getRoom ().Right = null;
		}
		
		if(testKey (x,y-1)){
			currentRoom.getRoom ().setUp (getFromDictionary (x,y-1).getState());
			up = getFromDictionary (x,y-1);
			up.setLinkedRoom("down", currentRoom);
			currentRoom.setLinkedRoom ("up", up);
				currentRoom.getRoom ().Up = up.getState ();
			}
			else{
				currentRoom.getRoom ().Up = null;
			}
		
		if(testKey (x,y+1)){
			currentRoom.getRoom ().setDown (getFromDictionary (x,y+1).getState());
			down = getFromDictionary (x,y+1);
			down.setLinkedRoom("up", currentRoom);
			currentRoom.setLinkedRoom ("down", down);
					currentRoom.getRoom ().Down = down.getState ();
				}
				else{
					currentRoom.getRoom ().Down = null;
				}
	}
	
	public void oneTwoNullMaze(int steps, string previousDir, LinkedRoom currentRoom, LinkedRoom previousRoom, bool oneTwo){
		currentRoom = ScriptableObject.CreateInstance ("LinkedRoom") as LinkedRoom;
		currentRoom.setState("active");
		if(previousRoom != null){													//Link current room to previous room if
			switch(previousDir){													//previous room is not null (starting room)
			case "left":
				currentRoom.setLinkedRoom ("right", previousRoom);
				currentRoom.setXY (previousRoom.getX ()-1,previousRoom.getY());
				break;
			case "right":
				currentRoom.setLinkedRoom ("left", previousRoom);
				currentRoom.setXY (previousRoom.getX ()+1,previousRoom.getY());
				break;
			case "up":
				currentRoom.setLinkedRoom ("down", previousRoom);
				currentRoom.setXY (previousRoom.getX (),previousRoom.getY()-1);
				break;
			case "down":
				currentRoom.setLinkedRoom ("up", previousRoom);
				currentRoom.setXY (previousRoom.getX (),previousRoom.getY()+1);
				break;
			}
		}
		else{
			currentRoom.setXY (0,0);
		}
		
		linkedRoomDictionary.Add (currentRoom.getX ()+","+currentRoom.getY (),currentRoom);
		
		if(oneTwo){					//one Null
			
				
		}
		else{					//two Null
		
		}
		
		if((steps == 0) || (currentRoom.getWalls () == 3)){
			return;
		}
	}

	public bool testKey(int x, int y){
		string temp = x.ToString () + "," + y.ToString ();
		return linkedRoomDictionary.ContainsKey (temp);
	}
	
	public LinkedRoom getFromDictionary(int x, int Y){
		string temp = x.ToString () + "," + Y.ToString ();
		return linkedRoomDictionary[temp];
	}
}