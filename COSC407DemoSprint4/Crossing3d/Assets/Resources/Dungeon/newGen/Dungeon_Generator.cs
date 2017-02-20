using UnityEngine;
using System.Collections;
using System;



public class Dungeon_Generator : MonoBehaviour {

    private char[] gameSeed;
    private Dungeon gameDungeon;
    private int floorCount;
	
	// Use this for initialization
    void Start()
    {
		gameSeed = generateSeed();
		Debug.Log("Game seed is: " + getGameSeedString());

        Debug.Log("Creating Dungeon");
        gameDungeon = createDungeonFromSeed(gameSeed);
        Debug.Log("Dungeon Created");

        floorCount = gameDungeon.determineFloorCount();

        Debug.Log("Loading First Floor");
        gameDungeon.instantiateFloor(0);
        Debug.Log("First Floor Loaded");
    }

    private void OnGUI()
    {


        for (int i = 0; i < floorCount; i++)
        {
            if (GUILayout.Button("Floor:" + i)) { 
                Debug.Log("Destroying Current Floor.");
                gameDungeon.destroyCurrentFloor();
                Debug.Log("Loading Floor:" + i);
                gameDungeon.instantiateFloor(i);
                Debug.Log("Floor: " + i + " has been loaded.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
	
	public Dungeon createNewDungeon() {
        return new Dungeon(generateSeed());
    }

    public Dungeon createDungeonFromSeed(char[] seed){
        Dungeon temp = new Dungeon(seed);
        return temp;
    }
	

//method used to randomly create a dungeon seed
    public char[] generateSeed() {
        char[] returnSeed = new char[10]; //size of seed will be 10

        for (int i = 0; i < 10; i++){        //for loop to create a rondom seed
            //float rand1 = UnityEngine.Random.value; //starts by creating a rondom int between 0-25 (Capital Letter Range)
            int rand1 = UnityEngine.Random.Range(0, 25);
            rand1 += 65;     //Create new ASCII int starting at 65('A') and adding random value
            char val = (char)rand1;          //transform ASCII value to character
            returnSeed[i] = val;             //ass charater to seed array
        }//for loop
        return returnSeed;
    }//generateSeed()

    //method to print string representation of seed, used for printing mostly
    public string getGameSeedString() {
        string x = new string(gameSeed);
        return x;
    }//getGameSeedString()

    //method to return the game seed as a char[] array
    public char[] getGameSeedArray(){
        return gameSeed;
    }//getGameSeedArray()

}