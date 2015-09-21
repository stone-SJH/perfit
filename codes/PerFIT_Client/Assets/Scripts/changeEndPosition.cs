using UnityEngine;
using System.Collections;

public class changeEndPosition : MonoBehaviour {
	public void changeEndToFirst(){
		gameObject.GetComponent<TweenPosition>().from = transform.localPosition;
		gameObject.GetComponent<TweenPosition> ().ResetToBeginning ();
		gameObject.GetComponent<TweenPosition>().to = new Vector3 (1280, 0, 0);
		gameObject.GetComponent<TweenPosition>().PlayForward ();
	}
	public void changeEndToSecond(){
		gameObject.GetComponent<TweenPosition>().from = transform.localPosition;
		gameObject.GetComponent<TweenPosition> ().ResetToBeginning ();
		gameObject.GetComponent<TweenPosition>().to = new Vector3 (0, 0, 0);
		gameObject.GetComponent<TweenPosition>().PlayForward ();
	}
	public void changeEndToThird(){
		gameObject.GetComponent<TweenPosition>().from = transform.localPosition;
		gameObject.GetComponent<TweenPosition> ().ResetToBeginning ();
		gameObject.GetComponent<TweenPosition>().to = new Vector3 (-1280, 0, 0);
		gameObject.GetComponent<TweenPosition>().PlayForward ();
	}
}
