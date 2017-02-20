using UnityEngine;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Simple AI for charging at player.
 * 
 * Versions - BT-112 Charging AI by Billy D. Spelchan
 */
public class ChargingAI : MonoBehaviour {

	public float chargeRange = 5.0f;
	public float speed = 1.0f;

	private Rigidbody2D body = null;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	// Called at regular interval and should be used for physics related stuff
	void FixedUpdate() {
		// make call so that it is easier to unit test this class
		ProcessChargeBehaviour ();
	}

	public bool ProcessChargeBehaviour() {
		// find location of player(s)
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		// assume not fleeing while looping over players to find if any within range choosing nearest
		bool isCharging = false;
		GameObject nearest = null;
		float nearDist = chargeRange;
		for (int cntr = 0; cntr < players.Length; ++cntr) {
			float distance = Vector3.Distance (transform.position, players [cntr].transform.position);
			//			Debug.Log ("distance from object :" + distance);
			if (distance < nearDist) {
				isCharging = true;
				nearest = players [cntr];
				nearDist = distance;
			}
		}

		// move towards nearest player if in range, otherwise stop moving
		if (body == null)
			body = GetComponent <Rigidbody2D>();
		if (nearest != null) {
			Vector2 chargeDir = new Vector2 (nearest.transform.position.x - transform.position.x, nearest.transform.position.y - transform.position.y);
			chargeDir.Normalize ();
			chargeDir.x *= speed;
			chargeDir.y *= speed;
			body.velocity = chargeDir;
		} else
			body.velocity = Vector2.zero;

		// might as well let caller know if charging
		return isCharging;
	}
}
