using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Manages the save games, including reading and writing to disk.
 * 
 * Versions - BT-216 Create a save game object (Billy)
 * 			BT-217 Save an object locally (Billy)
 * 			BT-219 Load an object locally (Billy)
 */
public class SaveManager{
	// *** CONSTANTS ***
	// Number of save slots
	const int NUM_SAVES = 3;
	// Key for encoding json text to obfuscate it
	const string SIMPLE_ENCODING = "CROSSING STREAMS crossing streams. Sorry Ben, but this is my key to saving!";

	// *** VARIABLES ***
	// holds the saves
	private SaveState[] saves;
	// current save being played
	private int currentSaveSlot = 0;
	// gets save state of current save being played as simple variable so array hidden
	public SaveState saveState {
		get { return saves [currentSaveSlot]; }
		set { saves [currentSaveSlot] = value; }
	}

	private static SaveManager instance = null;

	// *** CONSTRUCTOR ***

	/** Should use getInstance instead of creating this, as get instance only has one copy
	 * this is public instead of private to allow for unit testing.
	 * 
	 * Loads the current games from disk if they exist, creating new ones if they don't */
	public SaveManager() {
		// create the save slots
		saves = new SaveState[NUM_SAVES];

		// load the saves, creating default slots if no save file
		for (int cntr = 0; cntr < NUM_SAVES; ++cntr) {
			// create default save that will be overwritten if file exists
			saves [cntr] = new SaveState ();

			// if save exists, load it and overwrite state with proper state
			if (File.Exists ("Save" + cntr)) {
				StreamReader saveFile = null;
				try {
					// load data from file
					saveFile = File.OpenText ("Save" + cntr);
					string saveData = DecodeString(saveFile.ReadToEnd ());
					Debug.Log (saveData);
					// todo may want to do additional data verification
					// convert from json to object
					JsonUtility.FromJsonOverwrite (saveData, saves [cntr]);
				} catch (IOException e){
					Debug.LogException (e);
				} finally {
					saveFile.Close ();
				}
			} 
		}
	}

	/** Gets the singleton version of the Save Manager */
	public static SaveManager getInstance() {
		if (instance == null)
			instance = new SaveManager ();
		return instance;
	}

	// *** METHODS - SAVE MANAGEMENT ***

	/** selects the save slot that is being used. Slots start from 0 */
	public int ChangeCurrentSaveSlot(int saveSlot) {
		int oldSlot = currentSaveSlot;
		// make sure slot is in valid range
		if ((saveSlot >= 0) && (saveSlot < NUM_SAVES))
			currentSaveSlot = saveSlot;
		return oldSlot;
	}

	/** Writes current save slot to disk */
	public void Save() {
		StreamWriter saveFile = null;

		try {
			saveFile = File.CreateText ("Save" + currentSaveSlot);
			string saveData = EncodeString(JsonUtility.ToJson (saves [currentSaveSlot]));
			Debug.Log(saveData);
			// write data to file
			saveFile.Write(saveData);
		} catch(IOException e) {
			Debug.LogException (e);
		} finally {
			saveFile.Close ();
		}
	}

	// *** METHODS - OBFUCATION ***

	/** Obfuscates string using one-time pad method (may want to do proper encryption in future) */
	public string EncodeString(string s) {
		StringBuilder sb = new StringBuilder ();
		int len = s.Length;
		int encLen = SIMPLE_ENCODING.Length;
		for (int cntr = 0; cntr < len; ++cntr) {
			sb.Append ((char)(s [cntr] ^ (SIMPLE_ENCODING [cntr % encLen])));
		}
		return sb.ToString ();
	}

	/** Deobjfuscates string using one time pad method (may want to do proper encryption in future) */
	public string DecodeString(string s) {
		return EncodeString (s);
	}
}
