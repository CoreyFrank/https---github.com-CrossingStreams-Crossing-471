using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class Room : ScriptableObject {
	private bool built = false;
	private int playerCount = 0;
	private int x = 0;
	private int y = 0;
	private int walls = 0;
	private string type = "null";
	private string state = "null";
	private string up = null;
	private string down = null;
	private string left = null;
	private string right = null;
	private bool beaten = false;
	public string[,] layout = new string[5, 5];
	public bool active = false;
	//[System.Serializable]
	
	public Room(){
	}
	
	public string toString(){
		String result = "";
		result += built.ToString () + ",";
		result += playerCount.ToString () + ",";
		result += x.ToString () + ",";
		result += y.ToString () + ",";
		result += walls.ToString () + ",";
		result += type + ",";
		result += state + ",";
		result += up + ",";
		result += down + ",";
		result += left + ",";
		result += right + ",";
		result += beaten.ToString () + ",";
		for(int i = 0; i < 5; i ++){
			for (int j = 0; j < 5; j++){
				result += layout[i,j] + ",";
			}
		}
		return result;
	}
	
	public void fromString(String r){
		String[] s = r.Split (',');
		//Debug.Log ("room string array Length was: " + s.Length.ToString ());
		this.built = Convert.ToBoolean (s[0]);
		this.playerCount = Convert.ToInt32 (s[1]);
		this.x = Convert.ToInt32 (s[2]);
		this.y = Convert.ToInt32 (s[3]);
		this.type = s[5];
		this.state = s[6];
		this.up = s[7];
		this.down = s[8];
		this.left = s[9];
		this.right = s[10];
		this.beaten = Convert.ToBoolean (s[11]);
		int i = 12;
		for (int x = 0; x < 5; x ++){
			for(int y = 0; y < 5; y++){
				this.layout[x,y] = s[i];
				i++;
			}
		} 
	}
	
	public Room(bool built, int x, int y, bool beaten, string type, string state, 
	            string up, string down, string left, string right , string[,] layout, int count){
		this.built = built;
		this.playerCount = count;
		this.x = x;
		this.y = y;
		this.type = type;
		this.state = state;
		this.up = up;
		this.down = down;
		this.left = left; 
		this.right = right;
		this.beaten = beaten;
		this.layout = layout;  
	}
		
	public bool Built{
		set{this.built = value;} 
		get{return this.built;}	
	}	
	
	public int PlayerCount{
		set{this.playerCount = value;}
		get{return this.playerCount;}
	}
	
	public bool Active{
		set{this.active = value;}
		get{return this.active;}
	}
	
	public int X{
		set{this.x = value;}
		get{return this.x;}
	}
	
	public int Y{
		set{this.y = value;}
		get{return this.y;}
	}
	
	public string Left{
		set{this.left = value;}
		get{return this.left;}
	}
	
	public string Right{
		set{this.right = value;}
		get{return this.right;}
	}
	
	public string Up{
		set{this.up = value;}
		get{return this.up;}
	}
	
	public string Down{
		set{this.down = value;}
		get{return this.down;}
	}
	
	public string[,] Layout{
		set{this.layout = value;}
		get{return this.layout;}
	}
	  
	public bool getBeaten(){
		return beaten;
	}
	
	public void setBeaten(bool b){
		beaten = b;
	}
	
		
	public void incPlayer(){
		playerCount++;
	}
	
	public void decPlayer(){
		playerCount--;
	}
	
	public int getPlayerCount(){
		return playerCount;
	}
	
	public void setType(string t){
		type = t;
	}
	
	public void setState(string a){
		state = a;
	}
	
	public void setXY(int height, int width){
		x = height;
		y = width;
	}
	public void incWalls(){
		walls++;
	}
	
	public void setUp(string u){
		up = u;
	}

	public void setDown(string d){
		down = d;
	}
	
	public void setLeft(string l){
		left = l;
	}
	
	public void setRight(string r){
		right = r;
	}
		
	public int getX(){
		return x;
	}
	
	public int getY(){
		return y;
	}
	
	public int getWalls(){
		return walls;
	}
	
	public string getType(){
		return type;
	}
	
	public string getState(){
		return state;
	}
	
	public string getUp(){
		return up;
	}
	
	public string getDown(){
		return down;
	}
	public string getLeft(){
		return left;
	}
	
	public string getRight(){
		return right;
	}
	
	public void PathFull(){
		for(int x = 0; x < 5; x++){
			for(int y = 0; y < 5; y ++){
				layout[x,y] = "f";
			}
		}
	}
	
	public void PathConnect(){
//		for(int x = 0; x < 5; x++){
//			for(int y = 0; y < 5; y++){
//				layout[x,y] = null;
//			}
//		}
		bool connected = false;
		bool full = false;
		string final = "";
		if(left != null && left  != "null"){
			layout[0,2] = "L";
			final += "L";
		}
		if(right != null && right != "null"){
			layout[4,2] = "R";
			final += "R";
		}
		if(up != null && up != "null"){
			layout[2,0] = "U";
			final += "U";
		}
		if(down != null && down != "null"){
			layout[2,4] = "D";
			final += "D";
		} 
		sort (final);
		while(!connected && !full){
			List<string> paths = new List<string>();
			for(int x = 0; x <= 4; x++){
				for(int y = 0; y <= 4; y++){
					if(layout[x,y] != ""){
						if(final.Length == layout[x,y].Length){
							connected = true;
							break;
						}
						if(!paths.Contains (layout[x,y].ToUpper()) && layout[x,y] == layout[x,y].ToUpper ()){
							paths.Add (layout[x,y]);
							string temp = layout[x,y];
							layout[x,y] = layout[x,y].ToLower ();
							full = rndDir(x,y, temp);
						}
					}					
				}
			}
		}
Debug.Log ("Done");
	}
	
	private bool rndDir(int x, int y, string temp){
		List<string> dir = new List<string>();
		int rnd;
		bool full;
		if(x > 0)
			if(layout[x-1,y] == "")
				dir.Add ("left");
		if(x < 4)
			if(layout[x+1,y] == "")
				dir.Add ("right");
		if(y > 0)
			if(layout[x,y-1] == "")	
				dir.Add ("up");
		if(y < 4)
			if(layout[x,y+1] == "")
				dir.Add ("down");
		if(dir.Count > 0){			// path still has ability to grow from current point
			rnd = UnityEngine.Random.Range (0,dir.Count -1);
			
			switch(dir[rnd]){
			case "left":
				x--;
				layout[x,y] = temp;
				break;
			case "right":
				x++;
				layout[x,y] = temp;
				break;
			case "up":
				y--;
				layout[x,y] = temp;
				break;
			case "down":
				y++;
				layout[x,y] = temp;
				break;
			}
			chkNeighbors(x,y);
			return false;
		}
		else{					// path is blocked from growing in current spot
			full = unblockPath(temp, true);
			return full;
		}
	}
	
	private bool unblockPath(string temp, bool newDir){
		int x = 0;
		int y = 0;
		bool full = true;
		for(int i = 0; i <= 4; i++){
			for(int j = 0; j <= 4; j++){
				if(layout[i,j].ToLower () == temp.ToLower ()){
					if(pathAvailable(i,j)){
						x = i;
						y = j;
						full = false;
						break;
					}
				}
			}
		}
		if(!full){	
			if(newDir)
				rndDir (x, y, temp);
			else{
				layout[x,y] = layout[x,y].ToUpper();
			}
			return false;
		}
		else{
			return true;
		}
	}
	
	private bool pathAvailable(int x, int y){
		//List<string> dir = new List<string>();
		if(x > 0){
			if(layout[x-1,y] == "")
				return true;
		}
		if(x < 4){
			if(layout[x+1,y] == "")
				return true;
		}
		if(y > 0){
			if(layout[x,y-1] == "")
				return true;
		}
		if(y < 4){
			if(layout[x,y+1] == "")
				return true;
		}
		//if(dir.Count > 0)
			
		//else {
			return false;
				//}
	}
	
	private void chkNeighbors(int x, int y){
		List<string> paths = new List<string>();
		string temp = layout[x,y];
		paths.Add (layout[x,y]);
		if(x > 0){
			if(layout[x-1,y] != ""){
				if(!paths.Contains (layout[x-1,y].ToUpper ())){
					paths.Add (layout[x-1,y].ToUpper ());
					temp += layout[x-1,y].ToUpper ();
				}
			}
		}
		if(x < 4){
			if(layout[x+1,y] != ""){
				if(!paths.Contains (layout[x+1,y].ToUpper ())){
					paths.Add (layout[x+1,y].ToUpper ());
					temp += layout[x+1,y].ToUpper ();
				}
			}	
		}
		if(y > 0){
			if(layout[x,y-1] != ""){
				if(!paths.Contains (layout[x,y-1].ToUpper ())){
					paths.Add (layout[x,y-1].ToUpper ());
					temp += layout[x,y-1].ToUpper ();
				}
			}
		}
		if(y < 4){
			if(layout[x,y+1] != ""){
				if(!paths.Contains (layout[x,y+1].ToUpper ())){
					paths.Add (layout[x,y+1].ToUpper ());
					temp += layout[x,y+1].ToUpper ();
				}
			}  
		}
		sort (temp);
		layout[x,y] = temp;
		if(paths.Count > 1){
			foreach(string value in paths){
				updatePaths(value, temp.ToLower ());
			}
			
		}
	}
	
	private void updatePaths(string target, string final){
		for(int i = 0; i <= 4; i++){
			for(int j = 0; j <= 4; j++){
				if(layout[i,j] != ""){
					if(layout[i,j].ToUpper () == target)
						layout[i,j] = final;
				}
			}
		}
	}
	
	static string sort(string str)
	{
		char[] chr = str.ToCharArray();
		Array.Sort(chr);
		return new string(chr);
	}
}