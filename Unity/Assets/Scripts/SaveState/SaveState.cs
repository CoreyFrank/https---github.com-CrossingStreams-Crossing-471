using UnityEngine;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Holds the state of the game for a single save file. This then gets converted
 * to a JSON string and encoded before being saved.
 * 
 * Versions - BT-216 Create a save game object (Billy)
 */
[System.Serializable]
public class SaveState {

	// *** CONSTANTS ***

	// Place any achievement constants here, powers of 2
	public const int ACHIEVEMENT_SAVED_STATE = 1;	// This is reserved for testing the save state. It is set, or cleared by test

	// *** JSON VARIABLES - must be public ***

	/* Achievements just flags. If need more than 32, can add more achievement variables*/
	public int achievments = 0;

	/* GAME SETTINGS */

	public string save_name = "no-name";

	// TODO place game settings here

	/* STATISTICS */

	public long monsters_killed = 0;
	public long coins_collected = 0;
	public long power_ups_collected = 0;
	public long hits_taken = 0;
	public long seconds_played = 0;
	// TODO place additional stats here

	/** GAME PROGRESS - powerup's collected and ??? */

	// TODO place any hats and gears here (public bool or public int if need to count)


// ***** METHODS - Acheivment Management *****

	/** Sets the appropriate achievement flags */
	public void AddAchievment(int achievmentFlags) {
		achievments |= achievmentFlags;
	}

	/** removes the indicated achievment flags*/
	public void RemoveAchievment(int achievementFlags) {
		int mask = int.MaxValue ^ achievementFlags;
		achievments &= mask;
	}

	/** Checks to see if an achievment has been earned */
	public bool HasAchievment(int achievmentFlags) {
		return ((achievments & achievmentFlags) > 0);
	}
}
