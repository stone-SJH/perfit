using UnityEngine;
using System.Collections;
using System;

public class ArmMatching : BaseArmMatching {

	private double angle;

	// Use this for initialization
	protected override void getStatus(PXCMArmData.JointData[] data, bool isLeft)
	{
		if (isLeft)
			lhstatus = checkMotion (data);
		label1.GetComponent<UILabel>().text = lhstatus.ToString();
	}

	protected override Status checkMotion(PXCMArmData.JointData[] data)
	{
		angle = 2 * Math.Acos(data[1].localRotation.w);
		if (angle < MaxAngle)
			MaxAngle = angle;
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

	protected override bool isPrepared(PXCMArmData.JointData[] data)
	{
		return (IsDetecting && angle > 2.3);
	}

	protected override bool isDone(PXCMArmData.JointData[] data)
	{
		return (!IsDetecting && angle > 2.3);
	}

	protected override bool isBad(PXCMArmData.JointData[] data)
	{
		return (angle > 1.6);
	}

	protected override bool isGood(PXCMArmData.JointData[] data)
	{
		return (angle > 1.0);
	}

	protected override bool isGreat(PXCMArmData.JointData[] data)
	{
		return (angle > 0.6);
	}

	void OnGUI(){
		
		if (GUI.Button (new Rect (50, 250, 200, 30), "finish")) {
			Application.LoadLevel (1); 
		}
	}
}
