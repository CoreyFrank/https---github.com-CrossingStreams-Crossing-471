using UnityEngine;
using System.Collections;
using System;

//UnityEngine.MonoBehaviour:.ctor()

public class Dungeon {
	
	private const int FLOOR_MAX_COUNT = 10, FLOOR_MIN_COUNT = 4;
	protected int floorCount, currentFloor; 
	protected Dungeon_Floor[] floorList;
	protected char[] dungeonSeed;
	
	// Use this for initialization
	void Start()
    {
        
		
	}
	
	// Update is called once per frame
    void Update()
    {

    }

	public Dungeon(char[] gameSeed) {
		dungeonSeed = gameSeed;
		floorCount = determineFloorCount();
		floorList = instantiateFloors();
    }	
	
	public Dungeon_Floor[] instantiateFloors () {
		//programming to generate each floor,
		Dungeon_Floor[] returnList = new Dungeon_Floor[floorCount];
		for (int i = 0; i < floorCount; i++) {
			returnList[i] = new Dungeon_Floor(i + 1,floorCount,dungeonSeed);
		}
		return returnList;
	}

    public void instantiateFloor(int floorNumber) {
        currentFloor = floorNumber;
        floorList[floorNumber].instantiateFloor();
    }

    public void destroyCurrentFloor(){
        floorList[currentFloor].destroyFloor();
    }

	
	public int determineFloorCount() {
		//maths for selecting floor count, create two big ints using the floor min/max, as well as seed char values and prime numbers
		//this creates two unique numbers which will be random based on the seed, but will be generated the same every time.
		int a = FLOOR_MAX_COUNT * (dungeonSeed[0] + (FLOOR_MIN_COUNT) + dungeonSeed[2] / 4 + (FLOOR_MIN_COUNT) * 3 + (7 * FLOOR_MIN_COUNT + dungeonSeed[7]) )* 13;
        int b = (dungeonSeed[1] + FLOOR_MIN_COUNT + dungeonSeed[3]) * 421 * FLOOR_MAX_COUNT * 31;
		//add unique ints together, ensure not negative, and % by a prime number, then by FLOOR_MAX to get the number of floors,
		int returnFloorCount = (a + b);
		if (returnFloorCount < 0) {returnFloorCount *= -1;}
		returnFloorCount = returnFloorCount % 1049;
		returnFloorCount = returnFloorCount % FLOOR_MAX_COUNT;
		//though a check must be done to ensure floors > FLOOR_MIN, if floorCount < FLOOR_MIN, set floorCount to FLOOR_MIN
		if (returnFloorCount < FLOOR_MIN_COUNT) {returnFloorCount = FLOOR_MIN_COUNT;}
		return returnFloorCount;
	}


}