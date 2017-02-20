using UnityEngine;
using System;
using System.Collections;


/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Picks up a bundle of coins. The number of coins in the bundle is controlled by an
 * attribute so it can easily be adjusted to support a variety of coin packs, and even
 * can support removing the player's coins by using negative values.
 * 
 * Versions - BT-221 Create 5-10 objects (Billy)
 */
public class Coin : MonoBehaviour {
	public int coinsInBundle = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/** When player collides, give them the coins */
	public void OnTriggerEnter2D(Collider2D other) {
		// only players can pick this up
		if (other.CompareTag ("Player")) {
			// adjustment made to playerscript so grab it and make sure has it
			PlayerScript player = other.gameObject.GetComponent<PlayerScript> ();
			if (player) {
				GetCoin (player);
				// play sound then remove pickup
				PlayPickupSound ();
				Destroy (gameObject);
			}
		}
	}

	/* have action separate for testing */
	public void GetCoin(PlayerScript player) {
		// perform adjustment. Removing too many coins will remove all coins.
		var coinAdjust = coinsInBundle;
		if (coinsInBundle < 0)
			coinAdjust = Math.Max (coinAdjust, -player.coins);
		player.AdjustCoins (coinAdjust);
	}

	/* Plays sound when player picks this up. Override to change sound.*/
	public void PlayPickupSound() {
		// TODO (Corey?)
	}

}
