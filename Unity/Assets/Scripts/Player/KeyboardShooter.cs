using UnityEngine;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Control behaviour where cursor keys will shoot in indicated direction
 * Test by applying to an object on the screen then when running use cursor keys to validate.
 * Versions:
 * - BT-131	Link Character and Projectile by Billy D. Spelchan
 * - BT-221 for damage multipliers
 */
public class KeyboardShooter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject bullet = null;

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			bullet = SpawningUtility.SpawnBullet (gameObject.transform.position, .6f, Vector2.up);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			bullet = SpawningUtility.SpawnBullet (gameObject.transform.position, .6f, Vector2.right);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			bullet = SpawningUtility.SpawnBullet (gameObject.transform.position, .6f, Vector2.down);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			bullet = SpawningUtility.SpawnBullet (gameObject.transform.position, .6f, Vector2.left);
		}

		if (bullet) {
			PlayerScript player = GetComponentInParent<PlayerScript> ();
			if (player) {
				BasicProjectile projectile = bullet.GetComponent<BasicProjectile> ();
				if (projectile) {
					projectile.damage = (int)((float)projectile.damage * player.damageMultiplier);
					//Debug.Log ("bullet damage set to " + projectile.damage);
				}
			}
		}
	}
}
