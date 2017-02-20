/*
 * Crossing Streams Patcher
 * Patcher Actions 1.0
 * Last edit: Oct. 30 : Corey Frank
 */

import java.io.File;
import org.eclipse.jgit.api.Git;
import org.eclipse.jgit.lib.Repository;
import org.eclipse.jgit.storage.file.FileRepositoryBuilder;
public class PatcherActions {

	//directory locations for remote repo and local directory
	private static String REMOTE = "https://github.com/CoreyFrank/COSC470BTEAM";
	private static String DIR = "C:/temp/gittest001/";

	//return statements for remote and directory locations
	public static String getRemote() {return REMOTE;}
	public static String getDir() {return DIR;}
	
	//method used to create the repo for the first time, if the client has never downloaded 
	//the game this method will be called
	public static void createRepo() {
		//create the new folder structure for the game repo
		File gitWorkDir = new File(DIR);
		try {
		//clone the repository to the local folder
		Git git = Git.cloneRepository()		//starts clone command
				.setURI(REMOTE + ".git")	//set the remote .git directory
				.setDirectory(gitWorkDir)	//set the local directory
				.call();					//issue clone command
		
		// this line of code can be added for username / password
		// .setCredentialsProvider(credentialsProvider)
		
		//close the git repo connection, required for leak prevention
		git.close();
		
		//catch all exceptions
		}catch (Exception e) {e.printStackTrace();}
	}//createRepo()

	//method to pull from the repo, this will update the clients version of the game files
	public static void updateRepo() {
		//get the .git local directory
		File gitWorkDir = new File(DIR + "/.git");
		//build a repositoryBuilder, which will open the repo connection
		FileRepositoryBuilder builder = new FileRepositoryBuilder().setGitDir(gitWorkDir);
		
		try {
			Repository repository = builder.build();	//get the local repository
			Git git = new Git(repository); 				//create Git connection to repository
			git.pull().call();							//Issue the pull command to get updated game data
			git.close();								//close Git connection
		}catch(Exception e) {e.printStackTrace();}		//catching all exceptions
	}//updateRepo()
	
}//PatcherActions
