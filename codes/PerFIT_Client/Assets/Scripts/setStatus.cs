using UnityEngine;
using System.Collections;

public class setStatus : MonoBehaviour {
	public bool isSelected = false;
	public bool isDone = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isDone) {
			transform.Find ("tick-light").gameObject.SetActive (true);
			transform.Find ("tick-dark").gameObject.SetActive (true);
		}
		if (isSelected) {
			transform.Find ("Label-light").gameObject.SetActive (true);
			transform.Find ("tick-light").gameObject.SetActive (true);
			transform.Find ("bg-light").gameObject.SetActive (true);
			transform.Find ("tick-dark").gameObject.SetActive (false);
			transform.Find ("Label-dark").gameObject.SetActive (false);
		} else {
			transform.Find ("Label-light").gameObject.SetActive (false);
			transform.Find ("tick-light").gameObject.SetActive (false);
			transform.Find ("bg-light").gameObject.SetActive (false);
			transform.Find ("tick-dark").gameObject.SetActive (true);
			transform.Find ("Label-dark").gameObject.SetActive (true);
		}
		if (!isDone) {
			transform.Find ("tick-light").gameObject.SetActive (false);
			transform.Find ("tick-dark").gameObject.SetActive (false);
		}

	}
}
