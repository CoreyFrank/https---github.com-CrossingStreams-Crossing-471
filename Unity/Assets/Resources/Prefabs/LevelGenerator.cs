using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    public GameObject Room;
    public GameObject MapRoom;
    public int height;
    public int width;
    public int[,] map;
    public int[,] visited;

    public Renderer rend;

	// Use this for initialization
	void Start () {
        rend = Room.GetComponent<Renderer>();
        loadMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void loadMap()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % 2 != 0 && y % 2 != 0)
                {
                    map[x, y] = 0;
                } else {
                    map[x, y] = 1;
                }
            }
        }

        /*foreach (var row in map)
        {
            foreach (var col in map)
            {
                GameObject.Instantiate(Room, new Vector3((float)(-5.236511 + (-7.6 *2*i)), (float)(-4.374775 + (-5 * 2*j)), 0),
                               Quaternion.Euler(0, 0, 0));
                i++;
            }
            j++;
        }*/
        for (int x  =0;  x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject.Instantiate(Room, new Vector3((float)(-5.236511 + (7.6 * 2 * x)), (float)(-4.374775 + (-5 * 2 * y)), 0),
                                   Quaternion.Euler(0, 0, 0));
                    GameObject.Instantiate(MapRoom, new Vector3((float)(-5.236511 + (7.6 * 2 * x)), (float)(-4.374775 + (-5 * 2 * y)), 80),
                                   Quaternion.Euler(0, 0, 0));
                }
            }
        }
    }
}
