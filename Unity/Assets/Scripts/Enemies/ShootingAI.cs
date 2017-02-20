using UnityEngine;
using System.Collections;

enum ShootingAIStates {
	WAITING_TO_SHOOT,
	ACQUIRING_TARGET,
	PREDICT_AND_SHOOT
}

public class ShootingAI : MonoBehaviour {

	// Time between shots in fixed frames
	public int betweenShotTime = 200;

	// How fast the bullet moves
	public float bulletSpeed = 2;

	// How long the bullet lives
	public int bulletTTL = 300;

	// range of target detected
	public int targetRange = 7;

	private ShootingAIStates state = ShootingAIStates.WAITING_TO_SHOOT;
	private int nextShot = 200;
	private GameObject target;
	private Vector2 targetDetectPosition;

	// Use this for initialization
	void Start () {
		nextShot = betweenShotTime;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		switch (state) {
		case ShootingAIStates.WAITING_TO_SHOOT:
			--nextShot;
			if (nextShot <= 0)
				state = ShootingAIStates.ACQUIRING_TARGET;
			break;

		case ShootingAIStates.ACQUIRING_TARGET:
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			target = null;
			float nearDist = targetRange;
			for (int cntr = 0; cntr < players.Length; ++cntr) {
				float distance = Vector3.Distance (transform.position, players [cntr].transform.position);
				if (distance < nearDist) {
					target = players [cntr];
					nearDist = distance;
				}
			}
			if (target) {
				targetDetectPosition = new Vector2 (target.transform.position.x, target.transform.position.y);
				state = ShootingAIStates.PREDICT_AND_SHOOT;
			}
			break;
		
		case ShootingAIStates.PREDICT_AND_SHOOT:
			Vector2 targetAnticipatedPosition = PredictFuturePosition (targetDetectPosition, (Vector2)target.transform.position, 1);
			Vector2 shootDirection = targetAnticipatedPosition - (Vector2)gameObject.transform.position;
			SpawningUtility.SpawnBullet(gameObject.transform.position, .6f, shootDirection , bulletSpeed, bulletTTL);
			state = ShootingAIStates.WAITING_TO_SHOOT;
			nextShot = betweenShotTime;
			break;
		
		default:
			Debug.LogError ("Shooting AI somehow entered into unknown state");
			state = ShootingAIStates.WAITING_TO_SHOOT;
			nextShot = betweenShotTime;
			break;
		}
	}

	public Vector2 PredictFuturePosition(Vector2 start, Vector2 end, int steps) {
		Vector2 predictedStep = end - start;
		predictedStep.x *= steps;
		predictedStep.y *= steps;

//		Debug.Log (start + " to " + end + "then after " + steps + " should be " + (end + predictedStep));
		// TODO calculate preditect position
		return end + predictedStep;
	}
}
