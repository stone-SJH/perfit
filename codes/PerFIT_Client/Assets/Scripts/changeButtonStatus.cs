using UnityEngine;
using System.Collections;

public class changeButtonStatus : MonoBehaviour {
	public bool isUsing = false;
	public void OnClick(){
		transform.parent.Find ("SportPlanButton").transform.Find ("Picture2").gameObject.SetActive (false);
		transform.parent.Find ("StartSportButton").transform.Find ("Picture2").gameObject.SetActive (false);
		transform.parent.Find ("CheckSportButton").transform.Find ("Picture2").gameObject.SetActive (false);
		transform.parent.Find ("SportPlanButton").gameObject.GetComponent<changeButtonStatus> ().isUsing = false;
		transform.parent.Find ("StartSportButton").gameObject.GetComponent<changeButtonStatus> ().isUsing = false;
		transform.parent.Find ("CheckSportButton").gameObject.GetComponent<changeButtonStatus> ().isUsing = false;
		isUsing = (bool)(!isUsing);
		if (!isUsing) {
			//transform.Find ("Picture1").gameObject.SetActive (true);
			transform.Find ("Picture2").gameObject.SetActive (false);
		} else {	
			transform.Find ("Picture2").gameObject.SetActive (true);
			//transform.Find ("Picture1").gameObject.SetActive (false);
		}
		//transform.parent.Find ("SportPlanButton").GetComponent<changeButtonStatus> ().isUsing = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
