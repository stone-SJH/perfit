using UnityEngine;
using System.Collections;

public class Gesture{
	/*A structure for hands' gesture, I use isExist to judge the hand's existence */
	public PXCMHandData.AlertData alertData;/* Those two are not performed in my expected way */
	public PXCMHandData.GestureData gestureData;
	public bool isExist;

	public Gesture(){
		alertData = new PXCMHandData.AlertData ();
		gestureData = new PXCMHandData.GestureData ();
		isExist = true;
	}
}
