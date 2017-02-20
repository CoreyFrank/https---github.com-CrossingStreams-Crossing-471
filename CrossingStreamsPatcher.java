/*
 * Crossing Streams Patcher
 * Patcher main GUI 1.0
 * Last edit: Oct. 30 : Corey Frank
 */

import java.awt.EventQueue;
import javax.swing.JFrame;
import javax.swing.JLabel;
import java.awt.Font;
import java.io.File;
import javax.swing.JProgressBar;
import javax.swing.JButton;
import javax.swing.SwingConstants;

import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;



public class CrossingStreamsPatcher {

	private  JFrame frame;
	private  JProgressBar progressBar;
	private  JLabel statusLabel;
	private  JButton btnStart;
	private static CrossingStreamsPatcher thisWindow;
	
	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					thisWindow = new CrossingStreamsPatcher();
					
				} catch (Exception e) {e.printStackTrace();}
				
			}//run()
		});
	}//main()

	/**
	 * Create the application.
	 * @throws InterruptedException 
	 */

	//Method to see if the repo already exists, so we know if we're cloning or pulling
	private static boolean myRepoExists() {
		File gitWorkDir = null;
		try{ //try to open the file directory
			gitWorkDir = new File(PatcherActions.getDir());
		}catch(Exception e) {e.printStackTrace();return false;}
		
		//check for file path
		if (!gitWorkDir.exists()) {return false;} 
		else {return true;}
	}//myRepoExists()
	
	//method called when patcher opens
	private void updatingPatcher() {
		statusLabel.setText("Checking for repository version...");
		progressTheBar(); 
		
		//check if the local repo exists on client machine (check to see if the have any version,
		// if no repo exists the client is downloading for first time, so clone operation is used
		// if repo exists issue pull command
		if (!myRepoExists()) {
			statusLabel.setText("Creating Repository...");
			progressTheBar();
			
			//run the create repo command
			PatcherActions.createRepo();
			
			statusLabel.setText("Repo created...");
			progressTheBar();
		} else {
			statusLabel.setText("Pulling from repository...");
			progressTheBar();
			progressTheBar();
			//update the client's repo
			PatcherActions.updateRepo();
			
			statusLabel.setText("Repository pulled");
			progressTheBar();
		}
		
			//when the patcher is done, enable the start button
			btnStart.setEnabled(true);
			statusLabel.setText("Streams are up to date!");
			progressBar.setValue(100);
	}//updatingPatcher()
	
	//this is the method that is called when the patcher is done updating and the client presses the start button
	//this will execute the .exe of our game and close the patcher
	private void doThings() {
		Runtime rt = Runtime.getRuntime();
		String filePath = PatcherActions.getDir() + "<FILE NAME.exe>";
		try{
			//Process p = rt.exec(filePath);
			//command to run executable, greyed out for now
		}catch (Exception e) {e.printStackTrace();}
		thisWindow.frame.setVisible(false);
		thisWindow.frame.dispose();
	}//doThings()

	public CrossingStreamsPatcher() {
		initialize();
		frame.setVisible(true);
	}//CrossingStreamsPatcher

	private void progressTheBar() {
		progressBar.setValue(progressBar.getValue() + 20);
		frame.invalidate();
		frame.validate();
		frame.repaint();
		thisWindow.frame.setVisible(true);
	}//progress the bar


	/**
	 * Initialize the contents of the frame.
	 * This block is mostly all auto-generated, with the exception of the doThings() command added to an 
	 * action listener on the start button.
	 */
	private void initialize() {
		thisWindow = this;
		
		frame = new JFrame();
		frame.setBounds(100, 100, 666, 420);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.getContentPane().setLayout(null);

		JLabel lblNewLabel = new JLabel("Crossing Streams");
		lblNewLabel.setFont(new Font("Yu Gothic", Font.BOLD | Font.ITALIC, 36));
		lblNewLabel.setBounds(145, 11, 319, 73);
		frame.getContentPane().add(lblNewLabel);

		progressBar = new JProgressBar();
		progressBar.setBounds(145, 70, 319, 14);
		frame.getContentPane().add(progressBar);

		statusLabel = new JLabel("Checking status'");
		statusLabel.setHorizontalAlignment(SwingConstants.CENTER);
		statusLabel.setBounds(145, 95, 319, 41);
		frame.getContentPane().add(statusLabel);

		btnStart = new JButton("Start");
		btnStart.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				doThings();
			}
		});
		btnStart.setBounds(210, 240, 189, 73);
		frame.getContentPane().add(btnStart);
		btnStart.setEnabled(false);
		
		updatingPatcher();
	}
}
