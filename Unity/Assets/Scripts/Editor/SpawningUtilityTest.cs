using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class SpawningUtilityTest {

	[Test]
	public void SpawnMeleeTest()
	{
		//Arrange
		var gameObject = SpawningUtility.SpawnEnemy(Vector3.zero, SpawnableEnemyTypes.TestMeleeEnemy);

		//Act
		ChargingAI chargingAI = gameObject.GetComponent<ChargingAI>();
		FleeingAI fleeingAI = gameObject.GetComponent<FleeingAI>();

		//Assert
		Assert.IsTrue( chargingAI != null );
		Assert.IsFalse( fleeingAI != null );
	}

	[Test]
	public void SpawnRangedTest()
	{
		//Arrange
		var gameObject = SpawningUtility.SpawnEnemy(Vector3.zero, SpawnableEnemyTypes.TestRangedEnemy);

		//Act
		ChargingAI chargingAI = gameObject.GetComponent<ChargingAI>();
		FleeingAI fleeingAI = gameObject.GetComponent<FleeingAI>();
		ShootingAI shootingAI = gameObject.GetComponent<ShootingAI>();

		//Assert
		Assert.IsFalse( chargingAI != null );
		Assert.IsTrue( fleeingAI != null );
		Assert.IsTrue (shootingAI != null);
	}
}
