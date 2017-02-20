using UnityEngine;
using System.Collections;
using System;



public class Dungeon_Floor {

	public const int STORE_MAX = 10, STORE_MIN = 4, TREASURE_MAX = 10, TREASURE_MIN = 4;
	public const int MAP_HEIGHT_MIN = 15, MAP_HEIGHT_MAX = 50, MAP_WIDTH_MIN = 15, MAP_WIDTH_MAX = 50;
	private char[] dungeonSeed;

    //private GameObject tempRoom;


    protected int floorNumber, floorMapHeight, floorMapWidth, storeCount, treasureCount, totalFloors;
	private bool[,] dungeonBoolMap;
	private int[,] dungeonIntMap;
	protected Dungeon_Room[,] dungeonRoomMap;

    // Use this for initialization
    void Start()
    {
        initialBuild();
        mapPhaseTwo();
        mapPhaseThree();
        printMap();
        buildDungeonRooms();
    }




    // Update is called once per frame
    void Update()
    {

    }
	
	public Dungeon_Floor(int floorNmb, int floorCnt, char[] seed) {
		floorNumber = floorNmb;
		totalFloors = floorCnt;
        dungeonSeed = seed;
        floorMapHeight = generateMapHeight();
        floorMapWidth = generateMapWidth();
        dungeonRoomMap = new Dungeon_Room[floorMapHeight, floorMapWidth];
        Start();
		
	}

    internal void destroyFloor()
    {
        var objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject temp in objects)
        {
            if (temp.tag != "DND") { GameObject.Destroy(temp); }
            
        }
        


        
    }

    public void buildDungeonRooms() {
        for (int x = 0; x < floorMapHeight; x++){
            for (int y = 0; y < floorMapWidth; y++){

                if (dungeonIntMap[x, y] == -1) { dungeonRoomMap[x, y] = null; }

                else if (dungeonIntMap[x, y] >= 1 && dungeonIntMap[x, y] < 50)
                {
                    dungeonRoomMap[x, y] = new Dungeon_Room(dungeonIntMap[x, y], x, y, dungeonSeed);
                }
                else if (dungeonIntMap[x, y] >= 100 && dungeonIntMap[x, y] < 500)
                {
                    dungeonRoomMap[x, y] = new Dungeon_Room_Monster(dungeonIntMap[x, y], x, y, dungeonSeed);
                }
                else if (dungeonIntMap[x, y] == 1000)
                {
                    dungeonRoomMap[x, y] = new Dungeon_Room_Hub(dungeonIntMap[x, y], x, y, dungeonSeed);
                }

                else if (dungeonIntMap[x, y] == 1001)
                {
                    dungeonRoomMap[x, y] = new Dungeon_Room_Treasure(dungeonIntMap[x, y], x, y, dungeonSeed);
                }
                else if (dungeonIntMap[x, y] == 1002)
                {
                    dungeonRoomMap[x, y] = new Dungeon_Room_Shop(dungeonIntMap[x, y], x, y, dungeonSeed);
                }
                else if (dungeonIntMap[x, y] == 2000)
                {
                    dungeonRoomMap[x, y] = new Dungeon_Room_Boss(dungeonIntMap[x, y], x, y, dungeonSeed);
                }

            }//forloop through map
        }//forloop through map

    }//buildDungeonRooms()

    public void instantiateFloor() {
        int tempi = floorMapHeight;
        int tempj = floorMapWidth;

        var gap = 80;//should be my room size?

        for (int x = 0; x < tempi; x++)
        {
            for (int y = 0; y < tempj; y++)
            {
                var basefloor = Resources.Load("Dungeon/BaseFloor");
                



                Vector3 position = new Vector3(x * gap, 0, y * gap);
                Quaternion quat = Quaternion.Euler(0, 0, 0);

                if (dungeonRoomMap[x, y] != null)
                {
                    dungeonRoomMap[x, y].instantiate(position, quat);
                }

            }
        }
}//map

    private int generateMapHeight() {
		int height = floorNumber + MAP_HEIGHT_MAX * (dungeonSeed[0] + (MAP_HEIGHT_MIN * totalFloors) + dungeonSeed[2] / MAP_HEIGHT_MIN + (2 + dungeonSeed[6]) * MAP_HEIGHT_MAX + (dungeonSeed[1] * 7) )* 13;
		if (height < 0) {height *= -1;}		//ensure not negative
		height = height % MAP_HEIGHT_MAX;	//break height down until less than max value
		if (height < MAP_HEIGHT_MIN) {height = MAP_HEIGHT_MIN;}
		return height;
	}
	
	private int generateMapWidth() {
		int width = (MAP_WIDTH_MIN + dungeonSeed[1] + MAP_WIDTH_MAX + dungeonSeed[3] + MAP_WIDTH_MIN * dungeonSeed[7]) * 421 * floorNumber * MAP_WIDTH_MAX / 31;
		if (width < 0) {width *= -1;}//ensure not negative
		width = width % MAP_WIDTH_MAX;
		if (width < MAP_WIDTH_MIN) {width = MAP_WIDTH_MIN;}
		return width;
	}
	
	//the initial build function makes our map array by defining which rooms to be plotted in the boolean array, then applies a roomID to each cell, this
	//id will be used to determine which type of room is going to be placed in that cell space.
	//this function initializes the dungeonBoolMap[][] and dungeonIntMap[][]
	private void initialBuild() {
        dungeonBoolMap = new bool[floorMapWidth,floorMapWidth];


        dungeonIntMap = new int[floorMapHeight,floorMapWidth];
		
		for (int i = 0; i < floorMapHeight; i++) {
            for (int j = 0; j < floorMapWidth; j++) {
                //creating integers from seed values, these initial integers will be used to calculate the RoomID;
                //these 4 variables use up every seed value as well as applying formulas and multiplying by large prime numbers
                //this will give us a nice large base to start with.
                int a = floorNumber * (dungeonSeed[0] + (i * floorMapWidth) + dungeonSeed[2] + 4 + (i + floorMapHeight) * 3 + (j * 7 * floorMapWidth) )* 13;
                int b = (i + dungeonSeed[1] + floorMapWidth + dungeonSeed[3] + i * floorMapHeight) * 421 * floorNumber * floorMapHeight * 31;
                int c = ((dungeonSeed[4] * floorMapHeight + i*3 + dungeonSeed[5] / (floorNumber + 3 + j)) + ((dungeonSeed[8] + j * 13 + dungeonSeed[9]) / 3)) * 6661; //6661 is PRIME! A7X foREVer!
                int d = (dungeonSeed[6] * floorNumber * (7 + j)) * 331 + (i + dungeonSeed[7] * floorMapWidth + 223) * (j * j + i);

                int roomInt = a + b + c + d; //add together
                
                roomInt = roomInt * 2141;   //multiply by big prime number
                roomInt = roomInt % 601;    //% by nice prime number giving 601 possible roomID
                if (roomInt < 0) { roomInt *= -1; } //ensure all are positive
                //for now we will need many empty/null rooms, represented by room ID -1, we will grab a large number 
                //of rooms and identify them as -1, for now we will take roughly 40% of rooms
                if (roomInt > 250) { roomInt = -1; }

                //add roomInt to our array;
                dungeonIntMap[i, j] = roomInt;

                if (roomInt == -1){// if the room is not null, mark room as exist
                    dungeonBoolMap[i, j] = false;
                } else { dungeonBoolMap[i, j] = true;}
            }//for j
        }//for i
	}//initialBuild()
	
	//step two of map generation, this step ensures that all rooms have neighbors and that no island rooms exist
    private void mapPhaseTwo() {
        bool phaseCheck = false;        //while loop check, runs through the steps until it doesn't have to do them anymore
       
        while (phaseCheck == false) {
            int phaseCount = 0;         //counting how many rooms it adds, if none then exit while loop

            // for loops searching through array
            for (int i = 0; i < floorMapHeight; i++) {              
                for (int j = 0; j < floorMapWidth; j++) {

                    if (dungeonBoolMap[i, j] == true){         //if the current room exists,      
                        if (hasNeighbor(i, j) == false) {   //check to see if it has any neighbors, if it doesn't it's an island
                            //if the current room has no neighbor
                            //check if neighbor has a neighbor, use seed to determine which direction to check
                            bool dir1 = false;  //determined by seed, use to check path way, so it doesnt do the same order every time
                            bool dir2 = false;  // ^ ditto ^
                            bool check = false; //check variable to see if room is added (to stop from searching and adding two rooms)

                            if (dungeonSeed[4] % 2 == 0) { dir1 = true; } //determining direction from seed
                            if (dungeonSeed[7] % 2 == 0) { dir2 = true; } //determining direction from seed

                            //using dir1/dir2 choose order of checking next, if it ever returns true add room and break.
                            //if true true path, (checking neighbor right, counter-clockwise)
                            if (dir1 == true && dir2 == true && check == false){
                                if (checkRight(i, j) == true) { addRoom(i, j + 1); check = true; }
                                else if (checkUp(i, j) == true) { addRoom(i - 1, j); check = true; }
                                else if (checkLeft(i, j) == true) { addRoom(i, j - 1); check = true; }
                                else if (checkDown(i, j) == true) { addRoom(i + 1, j); check = true; }
                            }//if true true path, (checking neighbor right, counter-clockwise)

                            //true false (checking up, clockwise)
                            if (dir1 == true && dir2 == false && check == false){
                                if (checkUp(i, j) == true) { addRoom(i - 1, j); check = true; }
                                else if (checkRight(i, j) == true) { addRoom(i, j + 1); check = true; }
                                else if (checkDown(i, j) == true) { addRoom(i + 1, j); check = true; }
                                else if (checkLeft(i, j) == true) { addRoom(i, j - 1); check = true; }
                            }//true false (checking up, clockwise)

                            //false true (checking left, clockwise)
                            if (dir1 == false && dir2 == true && check == false) {
                                if (checkLeft(i, j) == true) { addRoom(i, j - 1); check = true; }
                                else if (checkUp(i, j) == true) { addRoom(i - 1, j); check = true; }
                                else if (checkRight(i, j) == true) { addRoom(i, j + 1); check = true; }
                                else if (checkDown(i, j) == true) { addRoom(i + 1, j); check = true; }

                            }//false true (checking left, clockwise)

                            //false false (checking down counter-clockwise)
                            if (dir1 == false && dir2 == false && check == false) {
                                if (checkDown(i, j) == true) { addRoom(i + 1, j); check = true; }
                                else if (checkRight(i, j) == true) { addRoom(i, j + 1); check = true; }
                                else if (checkUp(i, j) == true) { addRoom(i - 1, j); check = true; }
                                else if (checkLeft(i, j) == true) { addRoom(i, j - 1); check = true; }
                            }//false false (checking down counter-clockwise)

                            //if the neighbor rooms have no neighbors, this room is an island, and should be removed
                            if (check == false) {
                                removeRoom(i, j);   //removing current room
                            }
                            else {//if a room was added, increment the phaseCount
                                phaseCount++;
                            }// if room was added

                        }//if hasNeighbor()

                    }//if room esists
                }//for j
            }//for i

            ////////////////////////////////////////////
            //After the for loops, still in while loop//
            ////////////////////////////////////////////

            //printout the phaseCount for debuging
            Debug.Log("PHASECOUNT IS: " + phaseCount);
            if (phaseCount == 0) { phaseCheck = true; }  //if no rooms were added or removed exit loop
        }//while (phaseCheck)
    }//mapPhaseTwo()

//Step three of map generation, identifying the store, treasure and boss rooms
    private void mapPhaseThree() {

        int bossX = 0;
        int bossY = 0;

        //finding a generated starting position
        int starti = floorNumber * (dungeonSeed[1] + (floorMapHeight * floorMapWidth) + dungeonSeed[0] + 4 + (floorMapHeight * 2) + (floorMapWidth * 14)) * 13;
        int startj = (floorMapHeight + dungeonSeed[4] + floorMapWidth + dungeonSeed[7] + floorMapHeight * floorMapHeight) * floorNumber * floorMapHeight * 31;
        //ensure starting positions are within map range
        starti = starti % floorMapHeight;
        startj = startj % floorMapWidth;

        //find the number of treasure rooms and stores
        int tresCount = ((dungeonSeed[8] * floorMapHeight * floorMapWidth + dungeonSeed[2] * (floorNumber + 1)) + (dungeonSeed[6] * 13 + dungeonSeed[9] / (floorMapWidth + 1) * dungeonSeed[4]));
        int storeCount = (dungeonSeed[6] * floorNumber) * 331 + (dungeonSeed[7] * floorMapWidth + 223) * 626 + floorMapHeight * 2;
        //ensure number of store/treasure is less than max
        tresCount = tresCount % TREASURE_MAX;
        storeCount = storeCount % STORE_MAX;
        if (tresCount < TREASURE_MIN) { tresCount = TREASURE_MIN; }
        if (tresCount < STORE_MIN) { storeCount = STORE_MIN; }
        //total rooms to add, the plus two is for boss and hub
        int roomCount = tresCount + storeCount + 2;
        //room step refers to how many rooms the algorithm will jump before placing the next room, this ensures equal placement of rooms
        int roomStep = (floorMapHeight * floorMapWidth) / roomCount; //roomStep = (total rooms / roomsToAdd) = average distance between rooms.

    //we will be generating the rooms in the following order,
    // HUB --> alternate tres and shop until half rooms gone, --> boss --> remaining shop/treasure alternating
    // this will ensure that the treasure/shop rooms dont get placed in groups and that the boss will always be 
    // at least a certain distance from the hub, this will need to be changed as it will always be a similar range

    //if the starting position is a room, we will search for the next room, we want an empty space to put the HUB.

    int[] nextRoom = new int[2];

        if (dungeonBoolMap[starti, startj] == true) {
            nextRoom = findNextRoom(starti, startj);
        } else { nextRoom[0] = starti; nextRoom[1] = startj; }

        //not the nextRoom[] will be the placement of our HUB
        //we will set this roomID to be unique, HUB is 1000;
        dungeonBoolMap[nextRoom[0],nextRoom[1]] = true;  //make room value true
        dungeonIntMap[nextRoom[0], nextRoom[1]] = 1000;    //set room ID to 1000 - UNIQUE TO HUB
        roomCount--;

        

        //instantiate half the store rooms and half the treasure rooms
        for ( int i = 0; i < roomCount /2; i++) {
            for (int j = 0; j < roomStep; j++) { nextRoom = findNextRoom(nextRoom[0], nextRoom[1]); }
            if (tresCount > 0) { //if there are more treasure rooms to place, place them, if not go to shops
                nextRoom = findNextRoom(nextRoom[0], nextRoom[1]);
                dungeonBoolMap[nextRoom[0], nextRoom[1]] = true;  //make room value true
                dungeonIntMap[nextRoom[0], nextRoom[1]] = 1001;    //set room ID to 1001 - UNIQUE TO TREASUREROOM
                tresCount--; roomCount--;
            }
            for (int j = 0; j < roomStep;j++) { nextRoom = findNextRoom(nextRoom[0], nextRoom[1]); }
            if (storeCount > 0){ //if there are more store rooms to place, place them, if not go BOSS
                nextRoom = findNextRoom(nextRoom[0], nextRoom[1]);
                dungeonBoolMap[nextRoom[0], nextRoom[1]] = true;  //make room value true
                dungeonIntMap[nextRoom[0], nextRoom[1]] = 1002;    //set room ID to 1002 - UNIQUE TO STORE

                


                tresCount--; roomCount--;
            }
        }//first set of rooms

        //BOSS ROOM
        for (int j = 0; j < roomStep; j++) { nextRoom = findNextRoom(nextRoom[0], nextRoom[1]); }
        nextRoom = findNextRoom(nextRoom[0], nextRoom[1]);
        dungeonBoolMap[nextRoom[0], nextRoom[1]] = true;  //make room value true
        dungeonIntMap[nextRoom[0], nextRoom[1]] = 2000;    //set room ID to 2000 - UNIQUE TO BOSS

        bossX = nextRoom[0];
        bossY = nextRoom[1];

        roomCount--;
        //since the boss room will be a 2x2, we need to make the adjacent rooms null to prevent overlap.
        //im not doing this now, future!

        //REMAINING ROOMS
        while (roomCount > 0) {//place the remaining rooms
            for (int j = 0; j < roomStep; j++) { nextRoom = findNextRoom(nextRoom[0], nextRoom[1]); }
            if (tresCount > 0)
            { //if there are more treasure rooms to place, place them, if not go to shops
                nextRoom = findNextRoom(nextRoom[0], nextRoom[1]);
                dungeonBoolMap[nextRoom[0], nextRoom[1]] = true;  //make room value true
                dungeonIntMap[nextRoom[0], nextRoom[1]] = 1001;    //set room ID to 1001 - UNIQUE TO TREASUREROOM
                tresCount--; roomCount--;
            }
            for (int j = 0; j < roomStep; j++) { nextRoom = findNextRoom(nextRoom[0], nextRoom[1]); }
            if (storeCount > 0)
            { //if there are more store rooms to place, place them, if not go BOSS
                nextRoom = findNextRoom(nextRoom[0], nextRoom[1]);
                dungeonBoolMap[nextRoom[0], nextRoom[1]] = true;  //make room value true
                dungeonIntMap[nextRoom[0], nextRoom[1]] = 1002;    //set room ID to 1002 - UNIQUE TO STORE
                tresCount--; roomCount--;
            }
        }//placing remaining rooms

        adjustForBoss(bossX, bossY);

    }//mapPhaseThree()

    //as the boss room is 2x2, we need to make sure no rooms are beside the boss room when generating, 
    //to do this we will remove the three rooms at (x+1,y) (x,y+1) (x+1,y+1) 
    private void adjustForBoss(int bossXPos, int bossYPos){
        int x = bossXPos; int y = bossYPos;
        try { if (dungeonBoolMap[x+1,y] == true) {
            dungeonBoolMap[x + 1, y] = false;
            dungeonIntMap[x + 1, y] = -1;
        } }catch (Exception e) { Debug.Log(e); }

        try { if (dungeonBoolMap[x,y+1] == true){
            dungeonBoolMap[x, y + 1] = false;
            dungeonIntMap[x, y+1] = -1;
        } }catch (Exception e) { Debug.Log(e); }

        try { if (dungeonBoolMap[x+1,y+1] == true){
            dungeonBoolMap[x + 1, y + 1] = false;
            dungeonIntMap[x+1, y + 1] = -1;
        } }catch (Exception e) { Debug.Log(e); }

    }

    //room used to find the next room in the boolean array, it will return int[2] of the i,j co-ords.
    private int[] findNextRoom(int i, int j) {
        
        int[] retval = new int[2];
        int[] tempval = new int[2]; tempval[0] = i; tempval[1] = j;

        //check values first before checking in array,
            tempval[1]++;
            if (tempval[1] >= floorMapWidth) { tempval[1] = 0; tempval[0]++; } //if j is at the width, , and set the i value up,
            if (tempval[0] >= floorMapHeight) { tempval[0] = 0; }               //if i is at the height, reset it to 0

        //returns true if room is there
        bool check1 = dungeonBoolMap[tempval[0],tempval[1]];

        if (check1 == true) { //if there is a room next, we need to find the next null room
            return findNextRoom(tempval[0], tempval[1]);
        } else { //if there is no room next that is the room we want to select!
            retval = tempval;
        }
        return retval;
    }//findNextRoom()
	
	 /*
     *Private functions for checking neighboring rooms
     */

    //method to check if a certain room has neighbors on any side
    private bool hasNeighbor(int x, int y) { 
            bool val = false;
            //each check is done inside a try/catch in the event of ArrayOutOfBoundsException
            try {if (dungeonBoolMap[x, y + 1] == true) {val = true;} } catch (Exception e) { }
            try {if (dungeonBoolMap[x, y - 1] == true) {val = true;} } catch (Exception e) { }
            try {if (dungeonBoolMap[x + 1, y] == true) {val = true;} } catch (Exception e) { }
            try {if (dungeonBoolMap[x - 1, y] == true) {val = true;} } catch (Exception e) { }
            return val;
    }//hasNeighbor

    private bool checkRight(int i, int j) {
        try { if (dungeonBoolMap[i - 1, j + 1] == true || dungeonBoolMap[i, j + 2] == true || dungeonBoolMap[i + 1, j + 1] == true) { return true; }
        else { return false; } }catch (Exception e) { return false; }
    }//chackRight

    private bool checkUp(int i, int j) {
        try { if (dungeonBoolMap[i - 1, j - 1] == true || dungeonBoolMap[i -2, j] == true || dungeonBoolMap[i - 1, j + 1] == true) { return true; }
        else { return false; } }catch (Exception e) { return false; }
    }//checkUp

    private bool checkLeft(int i, int j){
        try { if (dungeonBoolMap[i - 1, j - 1] == true || dungeonBoolMap[i , j - 2] == true || dungeonBoolMap[i + 1, j - 1] == true) { return true; }
        else { return false; } }catch (Exception e) { return false; }
    }//checkLeft

    private bool checkDown(int i, int j){
        try { if (dungeonBoolMap[i + 1, j - 1] == true || dungeonBoolMap[i + 2, j] == true || dungeonBoolMap[i + 1, j + 1] == true) { return true; }
        else { return false; } }catch (Exception e) { return false; }
    }//checkDown

    //private function to remove room (before the room array is created)
    private void removeRoom(int i, int j) {
        dungeonBoolMap[i, j] = false;  //set room to false
        dungeonIntMap[i, j] = -1;       //set roomID to -1 (blank room)
    }//removeRoom()

    //private function to add room (before the room array is created)
    private void addRoom(int i, int j) {
        dungeonBoolMap[i, j] = true;                                                                                       //make room value true
        int roomid = floorNumber * (dungeonSeed[0] * (i * floorMapWidth) + dungeonSeed[2] * 3 + (i + floorMapHeight) * 7 + (j * 13 * floorMapWidth)) * 21;    //generate roomID
        roomid = roomid % 250; if (roomid < 0) { roomid *= -1; }                                                        //ensure roomID is positive
        dungeonIntMap[i, j] = roomid;                                                                                       //update roomID map
    }//addRoom()
	
	//method to print out the floor map to DEBUG
    public void printMap(){
        string x = "";      //printing blank map of X and _
        string y = "";      //printing map of RoomID's
        for (int i = 0; i < floorMapWidth; i++){
            for (int j = 0; j < floorMapHeight; j++){
                if (dungeonBoolMap[j,i] == true) {
                    if (dungeonIntMap[j, i] == 1000) { x += "H"; }
                    else if (dungeonIntMap[j, i] == 1001) { x += "T"; }
                    else if (dungeonIntMap[j, i] == 1002) { x += "S"; }
                    else if (dungeonIntMap[j, i] == 2000) { x += "B"; }
                    else { x += "X"; }
                } else { x += "_"; }  //if room exists, add X, else add _
                y += " " + dungeonIntMap[j, i] + " ,";    //add RoomID to y 
            }//for j
            x += "\n"; //print next line
            y += "\n"; //print next line
        }//for i

        Debug.Log(x);//printing Basic Map (Text)
        Debug.Log(y);//printing roomIDs
    }//printMap()
	
}