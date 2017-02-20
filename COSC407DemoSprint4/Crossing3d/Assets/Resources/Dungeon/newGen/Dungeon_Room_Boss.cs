using UnityEngine;
using System.Collections;
using System;



public class Dungeon_Room_Boss : Dungeon_Room_Monster {

	protected UnityEngine.Object bossMonster;

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame, cheks to see if boss is dead, if it is opens portal to next floor
    void Update() {
        if (isBossDead() == true && anyMonstersRemaining() == false) {
            spawnReward();
            unlockRoom();
            openNextFloor();
        }
    }
	
	public Dungeon_Room_Boss (int id, int xPos, int yPos, char[] seed) : base ( id, xPos, yPos, seed) {
        roomPrefab = Resources.Load("Dungeon/BossRoom");
        //roomMapPrefab = Resources.Load("Prefabs/Dungeon/MapBossRoom");
        bossMonster = determineBoss();
    }
	
    public bool isBossDead() {
        if (bossMonster != null) { return false; }
        else { return true; }
    }

    public UnityEngine.Object determineBoss() {
        //temp bosses for now
        //since we dont have components im not even doing this part, but this is where the math would be 
        //for selecting components to add to the boss
        UnityEngine.Object boss1 = Resources.Load("Dungeon/tempBoss");
        UnityEngine.Object[] bosses = new UnityEngine.Object[50];
        bosses[0] = boss1;
        bosses[1] = boss1;

        //maths for future bosses
        int count = ((dungeonSeed[1] * 71) * enemiesList.Length + (roomXPos * 7 + dungeonSeed[2] * dungeonSeed[9]) * roomID);

        //Select boss
        UnityEngine.Object theBoss = bosses[(count % bosses.Length)];

        return theBoss;
    }

    public void spawnBoss() {
       // GameObject tempBossMonster = (GameObject)GameObject.Instantiate(bossMonster, new Vector3((float)(roomPosition.x + 5), (float)(roomPosition.y -5), 5), Quaternion.Euler(0, 0, 0));
    }

	public void openNextFloor() {
        UnityEngine.Object thePortal = Resources.Load("Prefabs/Dungeon/Portal");
		GameObject spawnPortal = (GameObject)GameObject.Instantiate(thePortal, new Vector3((float)(roomPosition.x + 5), (float)(roomPosition.y - 5), 5), Quaternion.Euler(0, 0, 0));
    }
	
	public void returnToHub() {
		
	}

    internal override void instantiate(Vector3 position, Quaternion quaternion)
    {

        Vector3 temp = new Vector3(position.x + 40, position.y, position.z + 39);

        thisRoom = (GameObject)GameObject.Instantiate(roomPrefab, temp, Quaternion.Euler(0, 0, 0));
        roomPosition = position;

        //spawnMonsters();
        spawnBoss();
        
    }

  
}