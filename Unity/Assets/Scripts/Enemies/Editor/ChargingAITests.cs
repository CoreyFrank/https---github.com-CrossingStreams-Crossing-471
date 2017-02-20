using UnityEngine;
using System.Collections;
using NUnit.Framework;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Simple AI for charging at player.
 * 
 * Versions - BT-112 Charging AI by Billy D. Spelchan
 */

[TestFixture]
public class ChargingAITests {
	GameObject testAI, mockPlayer;

	/* testing utility method for creating player and monster objects for testing */
	public void setUpPositions(Vector3 player, Vector3 monster)
	{
		// create a player object - no components needed, but need to tag it player
		mockPlayer = new GameObject ();
		mockPlayer.tag = "Player";
		mockPlayer.transform.position = player;

		// create an AI with physics and flee attached to it
		testAI = new GameObject();
		testAI.transform.position = monster;
		testAI.AddComponent <Rigidbody2D> ();
		testAI.AddComponent <ChargingAI>();

	}

	/* first, make sure monster knows when it has to charge */
	[Test]
	public void ShouldMoveTest()
	{
		//Arrange
		setUpPositions(new Vector3(0,0,0), new Vector3(3,0,0)); 
		//Act
		ChargingAI ai = testAI.GetComponent<ChargingAI>();
		// verify
		Assert.IsTrue(ai.ProcessChargeBehaviour ());
	}

	/* if charging, make sure is moving towards player */
	[Test]
	public void IsMovingTest()
	{
		//Arrange
		setUpPositions(new Vector3(0,0,0), new Vector3(3,0,0)); 
		//Act
		ChargingAI ai = testAI.GetComponent<ChargingAI>();
		ai.ProcessChargeBehaviour ();
		//verify
		Rigidbody2D body = ai.GetComponent<Rigidbody2D> ();
		Assert.IsTrue (body.velocity.x < 0);
	}

	/* if player not too close, should stay still */
	[Test]
	public void ShouldStayTest()
	{
		//Arrange
		setUpPositions(new Vector3(0,0,0), new Vector3(13,0,0)); 
		//Act
		ChargingAI ai = testAI.GetComponent<ChargingAI>();
		//verify
		Assert.IsFalse(ai.ProcessChargeBehaviour ());
	}
}
