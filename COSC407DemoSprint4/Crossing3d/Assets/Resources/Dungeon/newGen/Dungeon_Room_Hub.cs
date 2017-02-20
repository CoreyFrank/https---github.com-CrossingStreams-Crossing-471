using UnityEngine;
using System.Collections;
using System;



public class Dungeon_Room_Hub : Dungeon_Room {

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
    void Update()
    {

    }
	
	public Dungeon_Room_Hub (int id, int xPos, int yPos, char[] seed) : base ( id, xPos, yPos, seed) {
        roomPrefab = Resources.Load("Prefabs/Dungeon/Hub Room");
        //roomMapPrefab = Resources.Load("Prefabs/Dungeon/MapHubRoom");
    }
	
	public void makeDungeon() {
        Dungeon_Generator dunGen = new Dungeon_Generator();
		char[] gameSeed = dunGen.generateSeed();
		Dungeon gameDungeon = dunGen.createDungeonFromSeed(gameSeed);
	}
	
	public void beginDescent() {
		
	}


}