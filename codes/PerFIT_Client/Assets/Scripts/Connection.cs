using UnityEngine;
using System.Collections;
using Client.CS;

public class Connection : MonoBehaviour{
	private static CSocket connection;
	public static CSocket getInstance(){
		if (connection == null) {
			connection = new CSocket ("59.78.22.46", 5555);
			connection.StartConnection();
		}
		Debug.Log (connection.ToString ());
		return connection;
	}
	void OnApplicationQuit(){
		connection.CloseConnection ();
		string ret = "";
		connection.SendFile ("bodyplan.txt",out ret);
		connection.SendFile ("armplan.txt",out ret);
		connection.SendFile ("handplan.txt",out ret);
		Debug.Log (ret);
		Debug.LogWarning ("Close Connection...");
	}
	void OnDestroy(){
		Debug.LogWarning ("Connection Destroy!");
	}
	void Awake(){
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
