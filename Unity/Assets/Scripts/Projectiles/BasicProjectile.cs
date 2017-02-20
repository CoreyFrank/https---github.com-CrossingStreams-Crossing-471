using UnityEngine;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Simple projectile.
 * 
 * Versions - BT-130 Create Projectile Class by Billy D. Spelchan
 */
public class BasicProjectile : MonoBehaviour {
	/* Speed of the projectile */
	public float speed = 1.0f;
	/* Vector indictating direction projectile should move in */
	public Vector2 direction = Vector2.up;

	/* Fixed Frame time to live */
	public int lifeInFixedFrames = 100;

	public int damage = 1;

	// Internal variable for physics so don't constantly have to grab
	private Rigidbody2D body;

	// remaining life of bullet
	private int lifeRemaining;

	// Use this for initialization
	void Start () {
		lifeRemaining = lifeInFixedFrames;
		body = gameObject.GetComponent <Rigidbody2D>();
		if (body == null) {
			gameObject.AddComponent<Rigidbody2D> ();
			body = gameObject.GetComponent <Rigidbody2D>();
			body.gravityScale = 0;
		}
		Vector2 newVelocity = direction.normalized;
		newVelocity.x *= speed;
		newVelocity.y *= speed;
		body.velocity = newVelocity;
	}

	// Update is called once per frame
	void Update () {
	
	}

	/* Update every physics frame to track life */
	void FixedUpdate() {
		--lifeRemaining;
		if (lifeRemaining <= 0)
			Destroy (gameObject);
//		Debug.Log (lifeRemaining);
	}

	/* When a bullet hits something, it should be destroyed, but some projectiles may have
	other things in mind so they can modify this behaviour by overriding this */
	public void ImpactedSomething(GameObject impactee) {
		Destroy (gameObject);
	}

}
