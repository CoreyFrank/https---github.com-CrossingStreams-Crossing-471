using UnityEngine;
using System.Collections;
using System;



public class Dungeon_Room {
	
	protected int roomID, roomXPos, roomYPos;
	protected bool isFirstEntry, isLocked;
	protected bool[] doorCheck;
	protected char[] dungeonSeed;
	protected UnityEngine.Object[] playersList;
	protected UnityEngine.Object[] itemList;
    protected Vector3 roomPosition;

    protected UnityEngine.Object roomPrefab;
    //protected UnityEngine.Object roomMapPrefab = Resources.Load("Prefabs/Dungeon/MapRoom");

    public GameObject thisRoom;

    // Use this for initialization
    void Start()
    {
		
	}
	
	// Update is called once per frame
    void Update()
    {

    }

 

    //public Dungeon_Room (int id, int xPos, int yPos, char[] seed, bool[] neighbors) {
    //	roomID = id;
    //	roomXPos = xPos;
    //	roomYPos = yPos;
    //	dungeonSeed = seed;
    //}

    public Dungeon_Room(){
        //used for temp holding value instead of null
        
    }

    public Dungeon_Room(int id, int xPos, int yPos, char[] seed)
    {
        roomID = id;
        roomXPos = xPos;
        roomYPos = yPos;
        dungeonSeed = seed;
        roomPrefab =  Resources.Load("Dungeon/Room");


    }

    public void lockRoom() {
		isLocked = true;
	}
	
	public void unlockRoom() {
		isLocked = false;
	}

    //public void spawnObject(Object spawnObj, vector2 position) { }
		
	
	
	public void onFirstEntry() {
		
	}
	
    internal void defaultInstantiate() {
        Vector3 position = new Vector3((float)(-5.236511 + (7.6 * 2 * roomXPos)), (float)(-4.374775 + (-5 * 2 * roomYPos)), 0);
        roomPosition = position;
        Quaternion quat = Quaternion.Euler(0, 0, 0);
        thisRoom = (GameObject)GameObject.Instantiate(roomPrefab, position, quat);
       // GameObject thisMap = (GameObject)GameObject.Instantiate(roomMapPrefab, new Vector3(position.x, position.y, 80), quat);
    }

    internal virtual void instantiate(Vector3 position, Quaternion quaternion)
    {
        thisRoom = (GameObject)GameObject.Instantiate(Resources.Load("Dungeon/Room"), position,Quaternion.Euler(0, 0, 0));
        roomPosition = position;
        //  GameObject thisMap = (GameObject)GameObject.Instantiate(roomMapPrefab, new Vector3(position.x, position.y, 80),Quaternion.Euler(0, 0, 0));
        //throw new NotImplementedException();
    }
}