using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;

public class MiddleActMatching : BaseActMatching {

	protected override void getStatus(PXCMHandData.JointData[] data, bool isLeft){
		if (isLeft)
			lhstatus = checkMotion (data);
		if (lhstatus != Status.None && lhstatus > BestPerform)
			BestPerform = lhstatus;
		label1.GetComponent<UILabel>().text = lhstatus.ToString();
	}

	protected override Status checkMotion(PXCMHandData.JointData[] data){
		if (isBad (data))
			return Status.Bad;
		if (isGreat (data))
			return Status.Great;
		if (isGood (data))
			return Status.Good;
		return Status.None;
	}

	protected override bool isBad(PXCMHandData.JointData[] data){
		Double DeltaIndex = Math.Sqrt(Math.Pow(data [8].positionWorld.x - data [6].positionWorld.x,2)
		                         +Math.Pow(data [8].positionWorld.z - data [6].positionWorld.z,2));
//		myTextLeft.text = DeltaIndex.ToString ();
		if (DeltaIndex > MaxDelta) {
			return true;
		}

		Double DeltaRing = Math.Sqrt(Math.Pow(data [16].positionWorld.x - data [14].positionWorld.x,2)
		                              +Math.Pow(data [16].positionWorld.z - data [14].positionWorld.z,2));
//		myTextLeft.text += "\n"+DeltaRing.ToString ();
		if (DeltaRing > MaxDelta) {
			return true;
		}

		Double DeltaPinky = Math.Sqrt(Math.Pow(data [20].positionWorld.x - data [18].positionWorld.x,2)
		                              +Math.Pow(data [20].positionWorld.z - data [18].positionWorld.z,2));
//		myTextLeft.text += "\n"+DeltaRing.ToString ();
		if (DeltaPinky > MaxDelta) {
			return true;
		}

		Double angle1 = 2 * Math.Acos(data[11].localRotation.w);
		Double angle2 = 2 * Math.Acos (data [12].localRotation.w);
		if (angle1 >= 0.5 || (angle2 <= 0.5 && angle2 >= 2))
			return true;

		return false;
	}
	protected override bool isGood(PXCMHandData.JointData[] data){
		Double angle2 = 2 * Math.Acos (data [12].localRotation.w);
		if (angle2 > 0.5 && angle2 < 2)
			return true;
		return false;
	}
	protected override bool isGreat(PXCMHandData.JointData[] data){
		Double angle2 = 2 * Math.Acos (data [12].localRotation.w);
		if (angle2 > 1.4 && angle2 < 1.6)
			return true;
		return false;
	}

	void OnGUI(){

		if (GUI.Button (new Rect (50, 250, 200, 30), "finish")) {
			Application.LoadLevel (1); 
		}
	}
}
