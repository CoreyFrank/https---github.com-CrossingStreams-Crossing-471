using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour
{
   
    public GameObject targetOut;
    public GameObject newCamera;
    public GameObject oldCamera;

    // Use this for initialization
    void Start()
    {
        oldCamera.GetComponent<Camera>().enabled = true;
        newCamera.GetComponent<Camera>().enabled = false;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Application.LoadLevel(levelName);
            other.transform.position = targetOut.transform.position;
            newCamera.GetComponent<Camera>().enabled = true;
            oldCamera.GetComponent<Camera>().enabled = false;
        }

     

    }
}