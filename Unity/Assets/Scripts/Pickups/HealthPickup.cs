using UnityEngine;
using System;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Picks up health. Amount determined by attribute. Default behavior is to only be
 * used when player actually needs health but this is a togglable attribute.
 * 
 * Versions - BT-221 Create 5-10 objects (Billy)
 */
public class HealthPickup : MonoBehaviour {
	public int healthToHeal = 1;
	public bool staysIfNotNeeded = true;

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
				// ignore if not needed (unless set to always pick up)
				if ((staysIfNotNeeded) && (player.health >= player.max_health))
					return;
				// do work (separate function for testing purposes)
				RestoreHealth(player);

				// play sound then remove pickup
				PlayPickupSound ();
				Destroy (gameObject);
			}
		}
	}

	public void RestoreHealth(PlayerScript player) {
		// perform health adjustment
		player.health = Math.Min(player.max_health, player.health + healthToHeal);
		Debug.Log ("Player health adjusted to " + player.health);
	}

	/* Plays sound when player picks this up. Override to change sound.*/
	public void PlayPickupSound() {
		// TODO (Corey?)
	}

}
