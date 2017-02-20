using UnityEngine;
using System.Collections;
using System;



public class Dungeon_Room_Monster : Dungeon_Room {

	protected bool isCleared;
	protected UnityEngine.Object[] enemiesList = null;
	protected UnityEngine.Object[] rewardItems;
    

    private const int MONSTER_COUNT_MAX = 12;
    private const int MONSTER_COUNT_MIN = 3;

    private const int REWARD_COUNT_MIN = 0;
    private const int REWARD_COUNT_MAX = 3;

    private int totalMonsters = 0;

    // Use this for initialization
    void Start()
    {
        lockRoom();
	}
	
	// Update is called once per frame, it will check to see if the monsters have all been defeated, if they are all dead spawn a reward
    void Update() {
        if (anyMonstersRemaining() == false) {
            spawnReward();
            unlockRoom();
        }
    }
	
	public Dungeon_Room_Monster (int id, int xPos, int yPos, char[] seed) : base ( id, xPos, yPos, seed ) {
        roomPrefab = Resources.Load("Dungeon/MonsterRoom");
        // roomMapPrefab = Resources.Load("Prefabs/Dungeon/MapTreasureRoom");
        enemiesList = determineEnemies();
        rewardItems = determineRewards();
    }
	
    //returns true if any monsters from this room still exist
	public bool anyMonstersRemaining () {
        for (int i = 0; i < enemiesList.Length; i++) {
            //if a monster exists, return true
            if (enemiesList[i] != null) {
                return true;
            }
        }
		return false;
	}//monstersRemaining()

    public UnityEngine.Object[] determineRewards() {
        int count = ((dungeonSeed[1] * REWARD_COUNT_MIN) + (roomXPos * 7 + dungeonSeed[2] * REWARD_COUNT_MAX) * roomID) % REWARD_COUNT_MAX+1;
        if (count < REWARD_COUNT_MIN) { count = REWARD_COUNT_MIN; }
        else if (count > REWARD_COUNT_MAX) { count = REWARD_COUNT_MAX; }

        UnityEngine.Object[] rewardList = new UnityEngine.Object[count];


        //grab current 5 prefab powerups to be deleted later
        //these will be used for testing purposes only for now, eventully switch to powerup builder
        UnityEngine.Object i_Coins = Resources.Load("Prefabs/Pickups/Coin");
        UnityEngine.Object i_Damage = Resources.Load("Prefabs/Pickups/DamagePowerup");
        UnityEngine.Object i_Health = Resources.Load("Prefabs/Pickups/HealthPickup");
        UnityEngine.Object i_Container = Resources.Load("Prefabs/Pickups/HeartContainer");
        UnityEngine.Object i_Speed = Resources.Load("Prefabs/Pickups/SpeedPowerup");

        //build temp array to be our powerups list
        UnityEngine.Object[] tempList = new UnityEngine.Object[5];
        tempList[0] = i_Coins;
        tempList[1] = i_Container;
        tempList[2] = i_Damage;
        tempList[3] = i_Health;
        tempList[4] = i_Speed;
        //DELETE THIS AND ALL ABOVE LATER!!!!

        for (int i = 0; i < count; i++) {
            int dec1 = (i * roomID +  i * roomXPos * dungeonSeed[9]) * 1433;
            dec1 = dec1 % 5;
            rewardList[i] = tempList[dec1];
        }
        return rewardList;
    }//determine rewards

    public UnityEngine.Object[] determineEnemies() {
        int count = ((roomID * dungeonSeed[2] * MONSTER_COUNT_MIN) + (roomXPos * roomYPos + dungeonSeed[8] * MONSTER_COUNT_MAX) * dungeonSeed[3]) % MONSTER_COUNT_MAX+1;
        
        if (count < MONSTER_COUNT_MIN) { count = MONSTER_COUNT_MIN; }
        else if (count > MONSTER_COUNT_MAX) { count = MONSTER_COUNT_MAX; }
        totalMonsters = count;

        UnityEngine.Object[] monsterList = new UnityEngine.Object[count];

        //create one monster for each COUNT, add monster to monsterList[i]
        for (int i = 0; i < count; i++) {
            //temp code for now, do not have monsters yet, so will need to change later
            UnityEngine.Object monster = Resources.Load("Dungeon/tempMonster");

            
            monsterList[i] = monster;
        }
        return monsterList;
    }//determineEnemies


    public void spawnMonsters () {
        if (enemiesList == null) { enemiesList = determineEnemies(); }

        //maths that can be used to determine monster type later
        int dec1 = MONSTER_COUNT_MIN * roomID + roomXPos * dungeonSeed[2];
        int dec2 = roomYPos + MONSTER_COUNT_MIN * roomID + dungeonSeed[1] * dungeonSeed[7];
        int dec3 = roomID + (MONSTER_COUNT_MAX * roomXPos) + (dungeonSeed[3] * dungeonSeed[4] + dungeonSeed[2]);

        for (int i = 0; i < MONSTER_COUNT_MAX; i++)
        {
            var monster = thisRoom.transform.FindChild("tempMonster" + (i)).gameObject.gameObject;
            //apply components here

            if (i >= totalMonsters) { GameObject.Destroy(monster); }

        }

    }
	
	public void spawnReward() {
		for (int i = 0; i < rewardItems.Length; i++) {
            GameObject tempItem = (GameObject)GameObject.Instantiate(rewardItems[i], new Vector3((float)(roomPosition.x + i),(float)0.1,  (float)(roomPosition.y * i)), Quaternion.Euler(0, 0, 0));
        }
	}

    internal override void instantiate(Vector3 position, Quaternion quaternion)    {
        thisRoom = (GameObject)GameObject.Instantiate(roomPrefab, position, Quaternion.Euler(0, 0, 0));
        roomPosition = position;
        spawnMonsters();
    }

    }