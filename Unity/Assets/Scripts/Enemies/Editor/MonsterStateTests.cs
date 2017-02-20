using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class MonsterStateTests {

	[Test]
	public void DamageTest()
	{
		//Arrange
		var gameObject = new GameObject();
		MonsterState monsterState = gameObject.AddComponent<MonsterState> ();
		monsterState.health = 3;

		//Act
		monsterState.TakeDamage(1);

		//Assert
		Assert.IsTrue(monsterState.health == 2);
	}
}
