using UnityEngine;
using System.Collections;
using System;

public class BodyMatching : BaseBodyMatching {
	
	private double angle,angle2;
	
	// Use this for initialization
	protected override void getStatus(PXCMBodyData.JointData[] data, bool isLeft)
	{
		if (isLeft)
			lhstatus = checkMotion (data);
		label1.GetComponent<UILabel>().text = lhstatus.ToString();
	}
	
	protected override Status checkMotion(PXCMBodyData.JointData[] data)
	{
		angle = 2 * Math.Acos(data[1].localRotation.w);
		angle2 = 2 * Math.Acos (data [0].localRotation.w);
		Debug.LogWarning (angle.ToString ());
		if (angle < MaxAngle)
			MaxAngle = angle;
		if (angle2 < MaxAngle2)
			MaxAngle2 = angle2;
		if (isPrepared (data))
			return Status.Prepared;
		if (isDone (data))
			return Status.Done;
		if (isBad (data))
			return Status.Bad;
		if (isGood(data))
			return Status.Good;
		if (isGreat(data))
			return Status.Great;
		return Status.None;
	}
	
	protected override bool isPrepared(PXCMBodyData.JointData[] data)
	{
		return (IsDetecting && angle > 2.3);
	}
	
	protected override bool isDone(PXCMBodyData.JointData[] data)
	{
		return (!IsDetecting && angle > 2.3);
	}
	
	protected override bool isBad(PXCMBodyData.JointData[] data)
	{
		return (angle > 2.0);
	}
	
	protected override bool isGood(PXCMBodyData.JointData[] data)
	{
		return (angle > 1.7);
	}
	
	protected override bool isGreat(PXCMBodyData.JointData[] data)
	{
		return (angle > 1.4);
	}
	
	void OnGUI(){
		
		if (GUI.Button (new Rect (50, 250, 200, 30), "finish")) {
			Application.LoadLevel (1); 
		}
	}
}