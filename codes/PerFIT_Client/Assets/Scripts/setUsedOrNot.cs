using UnityEngine;
using System.Collections;

public class setUsedOrNot : MonoBehaviour {
	public bool isUsed = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isUsed) {
			transform.Find ("Picture1").gameObject.SetActive (true);
			transform.Find ("Picture2").gameObject.SetActive (false);
			transform.parent.Find("Scroll View").transform.Find ("LockCollider").gameObject.SetActive (false);
		} else {
			transform.Find ("Picture1").gameObject.SetActive (false);
			transform.Find ("Picture2").gameObject.SetActive (true);
			transform.parent.Find("Scroll View").transform.Find ("LockCollider").gameObject.SetActive (true);
		}
	}
}
