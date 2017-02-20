using UnityEngine;
using System;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * A power-up that increases the players speed up to the cap in the player script.
 * 
 * Versions - BT-221 Create 5-10 objects (Billy)
 */
public class SpeedPowerup : MonoBehaviour {
	public float speedIncrease = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/** When player collides, give them more speed if they have not maxed out on speed */
	public void OnTriggerEnter2D(Collider2D other) {
		// only players can pick this up
		if (other.CompareTag ("Player")) {
			// adjustment made to playerscript so grab it and make sure has it
			PlayerScript player = other.gameObject.GetComponent<PlayerScript> ();
			if (player) {
				// separate function so can unit test
				GivePlayerSpeed(player);

				// play sound then remove pickup
				PlayPickupSound ();
				Destroy (gameObject);
			}
		}
	}

	/* Actually do the speed powerup */
	public void GivePlayerSpeed(PlayerScript player) {
		// perform speed adjustment
		player.speed.x = Math.Min(player.speed.x + speedIncrease, PlayerScript.MAX_SPEED);
		player.speed.y = Math.Min(player.speed.y + speedIncrease, PlayerScript.MAX_SPEED);
		Debug.Log ("Player speed is now <" + player.speed.x +","+player.speed.y+">");
	}

	/* Plays sound when player picks this up. Override to change sound.*/
	public void PlayPickupSound() {
		// TODO (Corey?)
	}

}
