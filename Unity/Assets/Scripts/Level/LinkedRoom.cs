using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable()]
public class LinkedRoom : ScriptableObject {

	private bool built = false;
	private Room room = ScriptableObject.CreateInstance ("Room") as Room;
	//[field:NonSerializedAttribute()]
	private LinkedRoom leftRoom = null;
	private LinkedRoom rightRoom = null;
	private LinkedRoom upRoom = null;
	private LinkedRoom downRoom = null;
	public string[,] layout = new string[5, 5];
		
	public bool Built{
		set{this.built = value;} 
		get{return this.built;}	
	}
				
	public void setBuilt(bool b){
		built = b;
	}
	
	public void setXY(int h, int w){
		room.setXY (h,w);
	}
	
	public Room getRoom(){
		return room;
	}
	
	public void incWalls(){
		room.incWalls();;
	}
	
	public void setState(string s){
		room.setState (s);
	}
	
	public void setLinkedRoom(string dir, LinkedRoom room){
		switch(dir){
			case "left":
				leftRoom = room as LinkedRoom;
				break;
			case "right":
				rightRoom = room as LinkedRoom;
				break;
			case "up":
				upRoom = room as LinkedRoom;
				break;
			case "down":
				downRoom = room as LinkedRoom;
				break;
		}
	}
	
	public void setUp(string u){
		room.setUp (u);
	}  
	
	public void setDown(string d){
		room.setDown (d);
	}
	
	public void setLeft(string l){
		room.setLeft (l);
	}  
	
	public void setRight(string r){
		room.setRight (r);
	}
	
	public bool getBuilt(){
		return built;
	}
	
	public string getUp(){
		return room.getUp ();
	}
	
	public string getDown(){
		return room.getDown ();
	}
	
	public string getLeft(){
		return room.getLeft ();
	}
	
	public string getRight(){
		return room.getRight ();
	}
	
	public int getY(){
		return room.getY();
	}
	
	public int getX(){
		return room.getX ();
	}
	
	public int getWalls(){
		return room.getWalls();
	}
	
	public string getState(){
		return room.getState ();
	}
	
	public LinkedRoom getRoom(string dir){
		switch(dir){
		case "left":
			return leftRoom;
		case "right":
			return rightRoom;
		case "up":
			return upRoom;
		case "down":
			return downRoom;
		default:
			return null;
		}
	}
	
//	public void PathConnect(){
//		bool connected = false;
//		bool full = false;
//		string final = "";
//		if(leftRoom != null && leftRoom.getState () == "active"){
//			layout[0,2] = "L";
//			final += "L";
//		}
//		if(rightRoom != null && rightRoom.getState () == "active"){
//			layout[4,2] = "R";
//			final += "R";
//		}
//		if(upRoom != null && upRoom.getState () == "active"){
//			layout[2,0] = "U";
//			final += "U";
//		}
//		if(downRoom != null && downRoom.getState () == "active"){
//			layout[2,4] = "D";
//			final += "D";
//		}
//		sort (final);
//		while(!connected && !full){
//			List<string> paths = new List<string>();
//			for(int x = 0; x <= 4; x++){
//				for(int y = 0; y <= 4; y++){
//					if(layout[x,y] != null){
//						if(final.Length == layout[x,y].Length){
//							connected = true;
//						}
//						if(!paths.Contains (layout[x,y].ToUpper()) && layout[x,y] == layout[x,y].ToUpper ()){
//							paths.Add (layout[x,y]);
//							string temp = layout[x,y];
//							layout[x,y] = layout[x,y].ToLower ();
//							full = rndDir(x,y, temp);
//						}
//						continue;
//					}					
//				}
//			}
//		}
//	}
//	
//	private bool rndDir(int x, int y, string temp){
//		List<string> dir = new List<string>();
//		int rnd;
//		bool full;
//		if(x > 0)
//			dir.Add ("left");
//		if(x < 4)
//			dir.Add ("right");
//		if(y > 0)
//			dir.Add ("up");
//		if(x < 4)
//			dir.Add ("down");
//		if(dir.Count > 0){			// path still has ability to grow from current point
//			rnd = UnityEngine.Random.Range (0,dir.Count -1);
//			
//			switch(dir[rnd]){
//			case "left":
//				x--;
//				layout[x,y] = temp;
//				break;
//			case "right":
//				x++;
//				layout[x,y] = temp;
//				break;
//			case "up":
//				y--;
//				layout[x,y] = temp;
//				break;
//			case "down":
//				y++;
//				layout[x,y] = temp;
//				break;
//			}
//			chkNeighbors(x,y);
//			return false;
//		}
//		else{					// path is blocked from growing in current spot
//			full = unblockPath(temp, true);
//			return full;
//		}
//	}
//	
//	private bool unblockPath(string temp, bool newDir){
//		int x = 0;
//		int y = 0;
//		bool full = true;
//		for(int i = 0; i <= 4; i++){
//			for(int j = 0; j <= 4; j++){
//				if(layout[i,j] == temp){
//					if(pathAvailable(i,j)){
//						x = i;
//						y = j;
//						full = false;
//						break;
//					}
//				}
//			}
//		}
//		if(!full){	
//			if(newDir)
//				rndDir (x, y, temp);
//			else{
//				layout[x,y] = layout[x,y].ToUpper();
//			}
//			return false;
//		}
//		else{
//			return true;
//		}
//	}
//	
//	private bool pathAvailable(int x, int y){
//		List<string> dir = new List<string>();
//		if(x > 0){
//			if(layout[x-1,y] == null)
//				dir.Add ("left");
//		}
//		if(x < 4){
//			if(layout[x+1,y] == null)
//				dir.Add ("right");
//		}
//		if(y > 0){
//			if(layout[x,y-1] == null)
//				dir.Add ("up");
//		}
//		if(x < 4){
//			if(layout[x,y+1] == null)
//				dir.Add ("down");
//		}
//		if(dir.Count > 0)
//			return true;
//		else {
//			return false;
//				}
//	}
//	
//	private void chkNeighbors(int x, int y){
//		List<string> paths = new List<string>();
//		string temp = layout[x,y];
//		paths.Add (layout[x,y]);
//		if(x > 0){
//			if(layout[x-1,y] != null){
//				if(!paths.Contains (layout[x-1,y].ToUpper ())){
//					paths.Add (layout[x-1,y].ToUpper ());
//					temp += layout[x-1,y].ToUpper ();
//				}
//			}
//		}
//		if(x < 4){
//			if(layout[x+1,y] != null){
//				if(!paths.Contains (layout[x+1,y].ToUpper ())){
//					paths.Add (layout[x+1,y].ToUpper ());
//					temp += layout[x+1,y].ToUpper ();
//				}
//			}	
//		}
//		if(y > 0){
//			if(layout[x,y-1] != null){
//				if(!paths.Contains (layout[x,y-1].ToUpper ())){
//					paths.Add (layout[x,y-1].ToUpper ());
//					temp += layout[x,y-1].ToUpper ();
//				}
//			}
//		}
//		if(y < 4){
//			if(layout[x,y+1] != null){
//				if(!paths.Contains (layout[x,y+1].ToUpper ())){
//					paths.Add (layout[x,y+1].ToUpper ());
//					temp += layout[x,y+1].ToUpper ();
//				}
//			}
//		}
//		sort (temp);
//		layout[x,y] = temp;
//		if(paths.Count > 1){
//			foreach(string value in paths){
//				updatePaths(value, temp.ToLower ());
//			}
//			
//		}
//	}
//	
//	private void updatePaths(string target, string final){
//		for(int i = 0; i <= 4; i++){
//			for(int j = 0; j <= 4; j++){
//				if(layout[i,j] != null){
//					if(layout[i,j].ToUpper () == target)
//						layout[i,j] = final;
//				}
//			}
//		}
//	}
//	
//	static string sort(string str)
//	{
//		char[] chr = str.ToCharArray();
//		Array.Sort(chr);
//		return new string(chr);
//	}
//	
}
