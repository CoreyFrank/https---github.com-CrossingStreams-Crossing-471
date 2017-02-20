using UnityEngine;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Simple AI for fleeing from player.
 * 
 * Versions - BT-111 Fleeing AI by Billy D. Spelchan
 */
public class FleeingAI : MonoBehaviour {

	public float fleeRange = 5.0f;
	public float speed = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Called at regular interval and should be used for physics related stuff
	void FixedUpdate() {
		// make call so that it is easier to unit test this class
		ProcessFleeBehaviour ();
	}

	public bool ProcessFleeBehaviour() {
		// find location of player(s)
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		// assume not fleeing while looping over players to find if any within range choosing nearest
		bool isFleeing = false;
		GameObject nearest = null;
		float nearDist = fleeRange;
		for (int cntr = 0; cntr < players.Length; ++cntr) {
			float distance = Vector3.Distance (transform.position, players [cntr].transform.position);
//			Debug.Log ("distance from object :" + distance);
			if (distance < nearDist) {
				isFleeing = true;
				nearest = players [cntr];
				nearDist = distance;
			}
		}

		// move away from nearest player if too close, otherwise stop moving
		Rigidbody2D body = GetComponent <Rigidbody2D>();
		if (nearest != null) {
			Vector2 fleeDir = new Vector2 (transform.position.x - nearest.transform.position.x, transform.position.y - nearest.transform.position.y);
			fleeDir.Normalize ();
			fleeDir.x *= speed ;
			fleeDir.y *= speed;
			body.velocity = fleeDir;
		} else
			body.velocity = Vector2.zero;

		// might as well let caller know if fleeing
		return isFleeing;
	}
}
