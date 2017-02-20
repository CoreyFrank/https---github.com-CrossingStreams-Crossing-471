using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class PickupTests {
	GameObject player = null;
	PlayerScript playerScript = null;
//	CircleCollider2D playerCollider = null;

	GameObject CreatePlayer() {
		if (player == null) {
			player = new GameObject ();
			player.tag = "Player";
			playerScript = player.AddComponent<PlayerScript> ();
//			playerCollider = player.AddComponent<CircleCollider2D> ();
		}
		return player;
	}

	/** Verify picking up coins adds to players money */
	[Test]
	public void CoinTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var coin = gameObject.AddComponent<Coin> ();
		int coins = playerScript.coins;

		//Act
		coin.GetCoin (playerScript);

		//Assert
		Assert.IsTrue(playerScript.coins > coins);
	}


	/** Verify picking up heart container adds to player max health */
	[Test]
	public void AddingHeartContainerTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var heartContainer = gameObject.AddComponent<HeartContainer> ();
		// make player max health 3 so known value
		playerScript.max_health = 3;

		//Act
		heartContainer.AddHeartContainer (playerScript);

		//Assert has more max health
		Assert.IsTrue(playerScript.max_health > 3);
	}

	/** Verify picking up heart container won't exceed max health cap */
	[Test]
	public void MaxedOutHeartContainerTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var heartContainer = gameObject.AddComponent<HeartContainer> ();
		// make player max health capped amount
		playerScript.max_health = PlayerScript.MAX_HEALTH_CAP;

		//Act
		heartContainer.AddHeartContainer (playerScript);

		//Assert has more max health
		Assert.IsFalse(playerScript.max_health > PlayerScript.MAX_HEALTH_CAP);
	}

	/** Verify picking up heart adds to player health */
	[Test]
	public void AddingHealthTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var health = gameObject.AddComponent<HealthPickup> ();
		// make player max health 3 and current health 2 so needs healing
		playerScript.max_health = 3;
		playerScript.health = 2;

		//Act
		health.RestoreHealth (playerScript);

		//Assert has more max health
		Assert.IsTrue(playerScript.health > 2);
	}

	/** Verify picking up heart won't exceed max health */
	[Test]
	public void MaxedOutHealthTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var health = gameObject.AddComponent<HealthPickup> ();
		// make player max health capped amount
		playerScript.max_health = PlayerScript.MAX_HEALTH_CAP;
		playerScript.health = PlayerScript.MAX_HEALTH_CAP;

		//Act
		health.RestoreHealth (playerScript);

		//Assert has more max health
		Assert.IsFalse(playerScript.health > PlayerScript.MAX_HEALTH_CAP);
	}



	/** Verify picking up speed adds to player speed */
	[Test]
	public void AddingSpeedTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var speed = gameObject.AddComponent<SpeedPowerup> ();
		// make player speed a known value
		playerScript.speed = new Vector2(10,10);

		//Act
		speed.GivePlayerSpeed (playerScript);

		//Assert has more max health
		Assert.IsTrue(playerScript.speed.x > 10);
	}

	/** Verify picking up speed when capped speed not exceeded */
	[Test]
	public void MaxedOutSpeedTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var speed = gameObject.AddComponent<SpeedPowerup> ();
		// make player speed capped amount
		var speedCap = PlayerScript.MAX_SPEED;
		playerScript.max_health = PlayerScript.MAX_HEALTH_CAP;
		playerScript.speed = new Vector2(speedCap,speedCap);

		//Act
		speed.GivePlayerSpeed (playerScript);

		//Assert has more max health
		Assert.IsTrue(playerScript.speed.x == speedCap);
	}



	/** Verify picking up damage multiplier adds to player damage */
	[Test]
	public void AddingDamageTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var damage = gameObject.AddComponent<DamagePowerup> ();
		// make player speed a known value
		playerScript.damageMultiplier = 1.0f;

		//Act
		damage.ApplyDamageMultiplier (playerScript);

		//Assert has more max health
		Assert.IsTrue(playerScript.damageMultiplier > 1.0f);
	}

	/** Verify picking up speed when capped speed not exceeded */
	[Test]
	public void MaxedOutDamageTest()
	{
		//Arrange
		CreatePlayer();
		var gameObject = new GameObject();
		var damage = gameObject.AddComponent<DamagePowerup> ();
		// make damage the max amount
		playerScript.damageMultiplier = PlayerScript.MAX_DAMAGE_MULTIPLIER;

		//Act
		damage.ApplyDamageMultiplier (playerScript);

		//Assert has more max health
		Assert.IsTrue(playerScript.damageMultiplier == PlayerScript.MAX_DAMAGE_MULTIPLIER);
	}

}
