using UnityEngine;
using RSUnityToolkit;
using System.Collections;

public class HandsModel {
	/*A structure for handsmodel, jointData is the most important thing*/
	public PXCMHandData.JointData[][] jointData;
	public int MaxHands = 2; //Max Hands
	public int MaxJoints = PXCMHandData.NUMBER_OF_JOINTS; //Max Joints
	public bool isLeft;/* whether left hand is existed */
	public bool isRight;/* whether right hand is existed */

	public HandsModel(){
		jointData = new PXCMHandData.JointData[MaxHands][];
		isLeft = false;
		isRight = false;
		
		for (int i = 0; i < MaxHands; i++) {
			jointData [i] = new PXCMHandData.JointData[MaxJoints];
		}
		
		for (int i = 0; i < MaxHands; i++)
		for (int j = 0; j < MaxJoints; j++) {
			jointData [i] [j] = new PXCMHandData.JointData ();
		}
	}
}

