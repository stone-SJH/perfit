using UnityEngine;
using System.Collections;

public class getUserName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetComponent<UILabel>().text = PlayerPrefs.GetString("User");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
