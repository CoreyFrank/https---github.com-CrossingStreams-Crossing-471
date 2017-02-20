using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour 
{
	public Transform target;
	public Transform prevTarget = null;
	float distCovered;
	float fracJourney;
	float startTime;
	float journeyLength;
	public bool move = false;
	//public bool matte = true;
	public float distance = 10.0f;
	
	public float xSpeed = 250.0f; 
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20;
	public float yMaxLimit = 80;
	
	private float x = 0.0f;
	private float y = 0.0f;
	
	void Awake(){
	}
	 
	public void setTarget(Transform trg){
		prevTarget = target;
		target = trg;
		move = true;
		if(prevTarget != null){
			startTime = Time.time;
			journeyLength = Vector3.Distance (prevTarget.position,target.position);
		}
//		if(matte){
//		GameObject matte = 
//		matte = false;
//		}
	}
	
	void Start () 
	{
		//DontDestroyOnLoad (transform.gameObject);
		var angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>()) 
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	void Update () 
	{
			if ((target && !move) || (prevTarget == null))
			{
				x += Input.GetAxis("RightJoyX") * xSpeed * Time.deltaTime;
				y -= Input.GetAxis("RightJoyY") * ySpeed * Time.deltaTime;
				
				y = ClampAngle(y, yMinLimit, yMaxLimit);
				
				transform.rotation = Quaternion.Euler(y, x, 0);
				transform.position = ((Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position);
			}
			else{
				distCovered = (Time.time - startTime)*100.0f;
				fracJourney = distCovered/journeyLength;
				transform.position = Vector3.Lerp ((Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + prevTarget.position,
				                                   (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position,fracJourney);
			}
			if(transform.position == (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position)
				move = false;
		}
	
	static float ClampAngle(float angle, float min, float max) 
	{
		if (angle < -360)
		{
			angle += 360;
		}
		if (angle > 360)
		{
			angle -= 360;
		}
		return Mathf.Clamp(angle, min, max);
	}
}