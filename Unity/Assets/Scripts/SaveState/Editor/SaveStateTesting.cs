using UnityEngine;
using UnityEditor;
using NUnit.Framework;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Unit tests for manages the save games, including reading and writing to disk.
 * 
 * Versions - BT-216 Create a save game object(Billy)
 * 			BT-217 Save an object locally(Billy)
 * 			BT-219 Load an object locally(Billy)
 */
[TestFixture]
public class SaveStateTesting {

	[Test]
	public void SaveStateTest()
	{
		//Arrange
		SaveManager saveManager = new SaveManager();

		//Act
		Debug.Log("Loading first save");
		saveManager.ChangeCurrentSaveSlot(2);
		bool oldTestState = saveManager.saveState.HasAchievment (SaveState.ACHIEVEMENT_SAVED_STATE);
		if (oldTestState)
			saveManager.saveState.RemoveAchievment (SaveState.ACHIEVEMENT_SAVED_STATE);
		else
			saveManager.saveState.AddAchievment (SaveState.ACHIEVEMENT_SAVED_STATE);
		Debug.Log("Writing changes save");
		saveManager.Save ();

		//Try - create new save manager to load saved state - note should only be one save manager
		Debug.Log("Loading second save");
		SaveManager testManager = new SaveManager();
		testManager.ChangeCurrentSaveSlot(2);

		//Assert
		Assert.IsFalse(oldTestState == testManager.saveState.HasAchievment(SaveState.ACHIEVEMENT_SAVED_STATE));
	}
}
