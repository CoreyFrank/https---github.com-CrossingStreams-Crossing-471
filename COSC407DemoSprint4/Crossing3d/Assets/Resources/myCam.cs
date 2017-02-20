using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCam : MonoBehaviour {

    public float speed = 50.0F;
    public float tiltAngle = 30.0F;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //moving basic camera
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }


        //tilting camera
       // float tiltZ = Input.GetAxis("Horizontal") * tiltAngle;
       // float tiltX = Input.GetAxis("Vertical") * tiltAngle;
       // Quaternion target = Quaternion.Euler(tiltX, 0, tiltZ);
       // transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2.0F);


        if (Input.GetKey(KeyCode.DownArrow))
        {
            Quaternion target = Quaternion.Euler(speed, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * tiltAngle);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Quaternion target = Quaternion.Euler(-speed, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * tiltAngle);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Quaternion target = Quaternion.Euler(0, speed, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * tiltAngle);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Quaternion target = Quaternion.Euler(0, -speed, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * tiltAngle);
        }






    }
    
}
