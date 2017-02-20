using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class ShootingAITest {

	[Test]
	public void PreditionTest()
	{
		//Arrange
		var gameObject = new GameObject();
		ShootingAI ai = gameObject.AddComponent<ShootingAI> ();

		//Act
		Vector2 testPredict = ai.PredictFuturePosition(Vector2.zero, Vector2.up, 11);
		Debug.Log (testPredict);
		//Assert
		Assert.IsTrue((testPredict.y > 11.5) && (testPredict.y < 12.5));
	}
}
