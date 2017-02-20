using UnityEngine;
using System.Collections;
using System.Diagnostics;
//using UnityEngine.Experimental.Networking;

public class LANMenuManager : MonoBehaviour {

	string ipAddress;
	Texture2D c;
	GUIStyle style;
	string hPort = "port number";
	int gameMode = 0;
	string cIp = "IP Address";
	string cPort = "port number";
	string remoteAddress = "10.1.144.91";

	// Use this for initialization
	void Start() {
		ipAddress = Network.player.ipAddress;

		c = new Texture2D(1, 1);
		c.SetPixel(0, 0, Color.grey);
		c.wrapMode = TextureWrapMode.Repeat;
		c.Apply();
		style = new GUIStyle();
		style.normal.background = c;
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	void OnGUI() {
		float w = Screen.width / 2;
		float h = Screen.height - (Screen.height / 4);
		GUI.Box(new Rect(w/2, (Screen.height - h)/2, w, h), "", style);
		//GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 36), ipAddress);
		if(GUI.Button(new Rect((Screen.width / 2) - 128, 64, 128, 36), "Host a LAN Game")) {
			gameMode = 0;
		}
		if(GUI.Button(new Rect((Screen.width / 2) + 128, 64, 128, 36), "Host Game")) {
			gameMode = 1;
		}
		if(GUI.Button(new Rect((Screen.width / 2) + 256, 64, 128, 36), "Connect to LAN Game")) {
			gameMode = 2;
		}

		if(gameMode == 0) {
			GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 200, 36), ipAddress);
			hPort = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 + 48, 200, 36), hPort, 20);

			if(GUI.Button(new Rect((Screen.width - 128) / 2, Screen.height / 2 + 88, 75, 36), "Launch")) {
				LaunchGame(ipAddress, hPort);
				StartCoroutine(PostRequest(ipAddress+":"+hPort));
			}
		} else if(gameMode == 1) {
			if(GUI.Button(new Rect((Screen.width - 128) / 2, Screen.height / 2 + 88, 75, 36), "Launch")) {
				StartCoroutine(PostRequest(remoteAddress));
			}

		} else if(gameMode == 2) {
			cIp = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 200, 36), cIp);
			cPort = GUI.TextField(new Rect((Screen.width - 128) / 2, Screen.height / 2 + 36, 75, 36), cPort);
			if(GUI.Button(new Rect((Screen.width - 128) / 2, Screen.height / 2 + 88, 75, 36), "Connect")) {
				StartCoroutine(GetRequest(cIp, cPort));
			}
		}
	}

	private void LaunchGame(string ip, string port) {
		//string filePath = (Application.dataPath) + "/Server/server.txt";
		string filePath = "./Server/crossingServer";
		//System.Diagnostics.Process.Start(filePath);
		Process p = new Process();
		p.StartInfo.FileName = filePath;
		p.StartInfo.Arguments = port;
		p.Start();
	}

	private IEnumerator PostRequest(string ip) {
		WWWForm form = new WWWForm();
		UnityEngine.Debug.Log("Posting");
		form.AddField("seed", 1234567890);
		UnityEngine.Debug.Log(form.data);

		//UnityWebRequest r = UnityWebRequest.Put("http://" + ip + "/send", "1234567890");
		//r.method = "Post";
		//yield return r.Send();
		UnityEngine.Networking.UnityWebRequest req = UnityEngine.Networking.UnityWebRequest.Post("http://" + ip + "/send", form);
		yield return req.Send();

		UnityEngine.Debug.Log("Post Result");
		UnityEngine.Debug.Log(req.uploadHandler.data);
		UnityEngine.Debug.Log(req.error);
	}

	private IEnumerator GetRequest(string ip, string port) {
		UnityEngine.Debug.Log(ip);
		UnityEngine.Networking.UnityWebRequest req = UnityEngine.Networking.UnityWebRequest.Get("http://" + ip +":" + port + "/get");
		//UnityWebRequest req = UnityWebRequest.Get("10.1.144.91/get");
		//UnityWebRequest req = UnityWebRequest.Get("10.1.169.147:25687/get");
		yield return req.Send();

		UnityEngine.Debug.Log("Get Result");
		UnityEngine.Debug.Log(req.error);
		UnityEngine.Debug.Log(req.downloadHandler.text);
	}
}
