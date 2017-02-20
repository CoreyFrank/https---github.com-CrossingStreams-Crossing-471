using UnityEngine;
using System.Collections;
using System;



/*
 * TO DO:
 *  Spawn items, (6)
 *  set prices, item type determines?
 *  item for sale? 50% off
 * 
 */


public class Dungeon_Room_Shop : Dungeon_Room {

    protected UnityEngine.Object[] allItems;
    protected UnityEngine.Object[] itemsForSale;

    private const int PRICE_MAX = 25;
    private const int PRICE_MIN = 5;
	
	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
    void Update()
    {

    }
	
	public Dungeon_Room_Shop (int id, int xPos, int yPos, char[] seed) : base ( id, xPos, yPos, seed) {

        roomPrefab = Resources.Load("Dungeon/StoreRoom");
        //roomMapPrefab = Resources.Load("Prefabs/Dungeon/MapStoreRoom");

        itemsForSale = determineItems();

    }//Dungeon_Room_Shop

    public UnityEngine.Object[] determineItems() {
        UnityEngine.Object[] tempItems = new UnityEngine.Object[6];


        //temp items list
        UnityEngine.Object[] tempList = new UnityEngine.Object[50];
        for (int i = 0; i < 50; i++)        {
            tempList[i] = Resources.Load("powerupWithTag");
        }
        allItems = tempList;

        for (int i = 0; i < tempItems.Length; i++) {
            //maths for determining 4 items, if needed later this can be changed from base items to components, for more flexible items (random items
            int itemC = ((roomID + 1) * roomYPos * dungeonSeed[2] + roomYPos) * (i * dungeonSeed[1] + i * 81) + (dungeonSeed[9] * (i + 77) * 211 * dungeonSeed[5]);
            itemC = itemC % allItems.Length;

            tempItems[i] = allItems[1];
                //allItems[itemC];
        }
        return tempItems;
    }

    private void updatePrice(GameObject item) {
        int price = 0;
        bool forSale = false;

        var itemPriceComponent = item.GetComponent<itemScript>();
        var itemPrefab = item.transform.FindChild("priceTag").gameObject.gameObject;
        var itemText = itemPrefab.transform.FindChild("PriceText").gameObject.GetComponent<TextMesh>();

        int currenti = (int)(item.transform.position.x - roomPosition.x) / 2;

        price = (roomYPos * roomXPos) + dungeonSeed[4]* currenti + dungeonSeed[1]* currenti * roomXPos * dungeonSeed[5] + dungeonSeed[6];

        int temp = price % 100;
        if (temp >= 96) { forSale = true; }

        price = price % PRICE_MAX;
        if (price < PRICE_MIN) { price = PRICE_MIN; }

        if (forSale == true) {
            price = price / 2;
            itemPriceComponent.isOnSale = true;
            itemText.color = Color.red;
        }
        itemPriceComponent.price = price;

        //get the text field of the price label
        

        itemText.text = "$ " + price;
     

    }

    public void stockShop() {
        itemsForSale = determineItems();

		for (int i = 0; i < itemsForSale.Length; i++) {
            //Vector3 tempVect = new Vector3((float)((thisRoom.transform.position.x - 5) + i * 2), (float)(thisRoom.transform.position.y + 1.5), 5);
            //GameObject item = (GameObject)GameObject.Instantiate(itemsForSale[i], tempVect , Quaternion.Euler(0, 0, 0));
            //previously instantiated items from blank, this time we have blanks to update

            int n = i + 1;
            String itemName = "item" + n;
            var item = thisRoom.transform.FindChild(itemName).gameObject.gameObject;      

            //determine price of item;
            updatePrice(item);

            //display price on screen, red text if on sale:



        }
	}

    internal override void instantiate(Vector3 position, Quaternion quaternion)
    {
        thisRoom = (GameObject)GameObject.Instantiate(roomPrefab, position, Quaternion.Euler(0, 0, 0));
        //  GameObject thisMap = (GameObject)GameObject.Instantiate(roomMapPrefab, new Vector3(position.x, position.y, 80),Quaternion.Euler(0, 0, 0));
        //throw new NotImplementedException();

        //after room is instantiated spawn the items
        stockShop();
    }

}