using UnityEngine;
using UnityEditor;
using NUnit.Framework;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Simple AI for fleeing from player.
 * 
 * Versions - BT-111 Fleeing AI by Billy D. Spelchan
 */
[TestFixture]
public class FleeingAITests {

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
		testAI.AddComponent <FleeingAI>();

	}

	/* first, make sure monster knows when it has to flee */
	[Test]
	public void ShouldMoveTest()
	{
		//Arrange
		setUpPositions(new Vector3(0,0,0), new Vector3(3,0,0)); 
		//Act
		FleeingAI ai = testAI.GetComponent("FleeingAI") as FleeingAI;
		// verify
		Assert.IsTrue(ai.ProcessFleeBehaviour ());
	}

	/* if fleeing, make sure is moving away */
	[Test]
	public void IsMovingTest()
	{
		//Arrange
		setUpPositions(new Vector3(0,0,0), new Vector3(3,0,0)); 
		//Act
		FleeingAI ai = testAI.GetComponent("FleeingAI") as FleeingAI;
		ai.ProcessFleeBehaviour ();
		//verify
		Rigidbody2D body = ai.GetComponent<Rigidbody2D> ();
		Assert.IsTrue (body.velocity.x > 0);
	}

	/* if player not too close, should stay still */
	[Test]
	public void ShouldStayTest()
	{
		//Arrange
		setUpPositions(new Vector3(0,0,0), new Vector3(13,0,0)); 
		//Act
		FleeingAI ai = testAI.GetComponent("FleeingAI") as FleeingAI;
		//verify
		Assert.IsFalse(ai.ProcessFleeBehaviour ());
	}


}
