using UnityEngine;
using System.Collections;

public class MonsterState : MonoBehaviour {
	// internal constants for handling debug logging
	private const bool  DEBUG_MONSTER_HIT = true;

	// How many hit points the monster has
	public int health = 2;
	// Right now the only thing we have for monsters is their health but will have more later

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// react to bullet hits
	void OnTriggerEnter2D(Collider2D other) {
		ProcessMonsterHit (other);
	}

	/*Handles monster collision*/
	public void ProcessMonsterHit(Collider2D other) {
		BasicProjectile projectile = other.gameObject.GetComponent<BasicProjectile> ();
		if (projectile != null) {
			if (DEBUG_MONSTER_HIT)
				Debug.Log ("Monster " + gameObject.name + " hit by " + other.gameObject.name);
			if ( ! TakeDamage (projectile.damage))
				ProcessDeath ();
			projectile.ImpactedSomething (gameObject);
		}		
	}

	/* Monster damage handler. Returns true if monster still alive */
	public bool TakeDamage(int amount) {
		health -= amount;
		return (health > 0);
	}

	/* handle the dying monster.
	Right now just remove them, but this makes it easy to change or overload */
	public void ProcessDeath() {
		Destroy (gameObject);
	}
}
