using UnityEngine;
using System;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Picks up a heart container that increases maximum health up to defined point.
 * 
 * Versions - BT-221 Create 5-10 objects (Billy)
 */
public class HeartContainer : MonoBehaviour {
	public int heartContainersGiven = 1;
	public bool healsAsWell = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/** When player collides, give them higher max health up to cap */
	public void OnTriggerEnter2D(Collider2D other) {
		// only players can pick this up
		if (other.CompareTag ("Player")) {
			// adjustment made to playerscript so grab it and make sure has it
			PlayerScript player = other.gameObject.GetComponent<PlayerScript> ();
			if (player) {
				AddHeartContainer (player);				

				// play sound then remove pickup
				PlayPickupSound ();
				Destroy (gameObject);
			}
		}
	}

	/* Heals the player - separate for testing */
	public void AddHeartContainer (PlayerScript player) {
		// perform adjustment
		player.max_health = Math.Min(player.max_health + heartContainersGiven, PlayerScript.MAX_HEALTH_CAP);
		if (healsAsWell)
			player.health += heartContainersGiven;
		Debug.Log ("Player maximum health adjusted to " + player.max_health);
	}

	/* Plays sound when player picks this up. Override to change sound.*/
	public void PlayPickupSound() {
		// TODO (Corey?)
	}

}
