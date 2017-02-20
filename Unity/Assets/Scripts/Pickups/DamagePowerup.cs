using UnityEngine;
using System;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * A power-up that increases the amount of damage that player shots cause up to the
 * maximum set within the PlayerScript.
 * 
 * Versions - BT-221 Create 5-10 objects (Billy)
 */
public class DamagePowerup : MonoBehaviour {
	// Attribute for controlling how much addtional damage player does
	public float damageMultiplierAdjustment = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/** When player collides, give them more damage if they have not maxed their damage multiplier */
	public void OnTriggerEnter2D(Collider2D other) {
		// only players can pick this up
		if (other.CompareTag ("Player")) {
			// adjustment made to playerscript so grab it and make sure has it
			PlayerScript player = other.gameObject.GetComponent<PlayerScript> ();
			if (player) {
				// separate function for testing
				ApplyDamageMultiplier(player);

				// play sound then remove pickup
				PlayPickupSound ();
				Destroy (gameObject);
			}
		}
	}

	/* perform damage multiplier adjustment */
	public void ApplyDamageMultiplier(PlayerScript player) {
		player.damageMultiplier = Math.Min(player.damageMultiplier + damageMultiplierAdjustment, PlayerScript.MAX_DAMAGE_MULTIPLIER);
		Debug.Log ("Player damage multipler is now " + player.damageMultiplier);
	}

	/* Plays sound when player picks this up. Override to change sound.*/
	public void PlayPickupSound() {
		// TODO (Corey?)
	}
}
