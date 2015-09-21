using UnityEngine;
using System.Collections;

public class getFileFromServer : MonoBehaviour {
	public enum playType {hand, arm, body}
	public playType choice = 0;

	void OnClick(){
		string ret = "";
		string name = transform.Find ("Title").GetComponent<UILabel>().text;
		Debug.LogWarning (name);
		if (choice == playType.hand) {
			Connection.getInstance ().RecvMovie (name, "hand", out ret);
		} else if (choice == playType.arm) {
			Connection.getInstance ().RecvMovie (name, "arm", out ret);
		} else {
			Connection.getInstance ().RecvMovie (name, "body", out ret);
		}
		Debug.LogWarning (ret);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
