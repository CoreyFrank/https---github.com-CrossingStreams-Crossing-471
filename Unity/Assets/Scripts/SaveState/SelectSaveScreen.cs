using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/**
 * COSC 470 2016 Team B project
 * Team: Ben Ward, Billy Spelchan, Corey Frank, Daniel Atkinson, Marc-Andrew Dunwell
 * Project: Crossing Streams
 * Licence: MIT License.
 * 
 * Handles the select save game screen which lets users select which save they wish to start from
 * 
 * Versions - BT-218 Select starting save (Billy)
 */
public class SelectSaveScreen : MonoBehaviour {

	// Holds the save manager instance
	private SaveManager saveManager;

	// Use this for initialization
	void Start () {
		// get the instance of the save manager
		saveManager = SaveManager.getInstance ();

		// set up the data for the 3 save slots in the display. Could be a for loop, but only 3.
		FillSaveSlotData (0);
		FillSaveSlotData (1);
		FillSaveSlotData (2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void FillSaveSlotData(int slotID) {
		// get the save slot for the current slot to be filled
		Transform slot = transform.Find("SaveSlot" + slotID);
		saveManager.ChangeCurrentSaveSlot (slotID);
		SaveState state = saveManager.saveState;

		// activate the input field so that changing text updates the save information
		InputField saveName = slot.GetComponentInChildren <InputField>() ;
		saveName.text = state.save_name;
		saveName.onValueChanged.AddListener (delegate {
			saveManager.ChangeCurrentSaveSlot(slotID);
			saveManager.saveState.save_name = saveName.text;
			saveManager.Save();
		});

		// fill the monster killed statistic line
		Transform child  = slot.Find("monsters_killed");
		Text txt = child.GetComponent<Text> ();
		txt.text = "Monsters Killed: " + state.monsters_killed;

		// fill the coins_collected statistic line
		child  = slot.Find("coins_collected");
		txt = child.GetComponent<Text> ();
		txt.text = "coins collected: " + state.coins_collected;

		// fill the power_ups_collected statistic line
		child  = slot.Find("power_ups_collected");
		txt = child.GetComponent<Text> ();
		txt.text = "Power Ups: " + state.power_ups_collected;

		// fill the hits_taken statistic line
		child  = slot.Find("hits_taken");
		txt = child.GetComponent<Text> ();
		txt.text = "Hits taken: " + state.hits_taken;

		// Activate the button so that gams starts when clicked.
		Button startGameButton = slot.GetComponentInChildren <Button>() ;
		startGameButton.onClick.AddListener (delegate {
			saveManager.ChangeCurrentSaveSlot(slotID);
			SceneManager.LoadScene("Hub");
		});
	}
}
