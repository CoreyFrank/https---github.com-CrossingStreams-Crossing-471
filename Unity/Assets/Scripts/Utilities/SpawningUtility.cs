using UnityEngine;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Utility for spawning common items into game. Will probably be modified by many different tasks.
 * 
 * Versions:
 * - BT-130	Create Projectile Class by Billy D. Spelchan
 */

public enum SpawnableEnemyTypes {
	TestRangedEnemy, TestMeleeEnemy
}

public class SpawningUtility {
	public static GameObject SpawnBullet(Vector3 position, float offset, Vector2 direction, float speed = 1f, int ttl = 1000) {
		// create new game object
		GameObject bullet = new GameObject ();

		// add bullet image to it
		SpriteRenderer sr = bullet.AddComponent<SpriteRenderer> ();
		sr.sprite = Resources.Load<Sprite>("Sprites/Projectiles/Bullet");

		// make it act like a projectile
		BasicProjectile projectile = bullet.AddComponent<BasicProjectile> ();
		projectile.direction = direction;
		projectile.speed = speed;
		projectile.lifeInFixedFrames = ttl;

		// add a collider
		CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
		collider.radius = .1f;

		// position the object
		Vector2 adjust = direction.normalized;
		adjust.x *= offset;
		adjust.y *= offset;
		Vector3 bulletPos = new Vector3 (position.x + adjust.x, position.y + adjust.y, position.z);
		bullet.transform.position = bulletPos;

		return bullet;
	}

	public static GameObject SpawnEnemy(Vector3 position, SpawnableEnemyTypes enemyType) {
		Object prefab = null;
		switch (enemyType) {
		case SpawnableEnemyTypes.TestRangedEnemy:
			prefab = Resources.Load<Object> ("Prefabs/Enemies/TestRangedEnemy");
			break;
		case SpawnableEnemyTypes.TestMeleeEnemy:
			prefab = Resources.Load<Object> ("Prefabs/Enemies/TestMeleeEnemy");
			break;
		default:
			Debug.LogError ("Unknown enemy type specified to be spawned " + enemyType);
			return null;
		}
		GameObject gameObject = Object.Instantiate (prefab) as GameObject;
		gameObject.transform.position = position;
		return gameObject;
	}
}
