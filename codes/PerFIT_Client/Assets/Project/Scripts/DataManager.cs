using UnityEngine;
using RSUnityToolkit;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;

public class DataManager : MonoBehaviour {
	/* A class used to package the hands' data got from RealSense UnityToolkit */
	public HandsModel hands = null;
	public Gesture[] gesture = null;
	public PXCMDataSmoothing dataSmoothing = null; //Smoothing module instancep
	public PXCMDataSmoothing.Smoother3D[][] smoother3D= null; 

	[HideInInspector]
	public int MaxHands; //Max Hands
	[HideInInspector]
	public int MaxJoints; //Max Joints

	protected PXCMSenseManager senseManager = null; //SenseManager Instance
	protected pxcmStatus status; //StatusType Instance
	protected PXCMHandModule handAnalyzer; //Hand Module Instance
	[HideInInspector]
	public int NumOfHands = 0;
	public Hashtable handList;
	
	private int weightsNum = 15; //smoothing factor

	// Use this for initialization
	void Start () {
		hands = new HandsModel ();
		MaxHands = hands.MaxHands;
		MaxJoints = hands.MaxJoints;
		handList = new Hashtable ();

		senseManager = PXCMSenseManager.CreateInstance ();
		if (senseManager == null)
			Debug.LogError ("SenseManager Initialization Failed");
		
		/* Enable hand tracking and retrieve an hand module instance to configure */
		status = senseManager.EnableHand ();
		handAnalyzer = senseManager.QueryHand ();
		if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
			Debug.LogError ("PXCSenseManager.EnableHand: " + status);
		
		/* Initialize the execution pipeline */
		status = senseManager.Init ();
		if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
			Debug.LogError ("PXCSenseManager.Init: " + status);

		/* Retrieve the the DataSmoothing instance */
		senseManager.QuerySession ().CreateImpl<PXCMDataSmoothing> (out dataSmoothing);
		smoother3D = new PXCMDataSmoothing.Smoother3D[MaxHands][];

		/* Configure a hand - Enable Gestures and Alerts */
		PXCMHandConfiguration hcfg = handAnalyzer.CreateActiveConfiguration ();
		hcfg.EnableAllGestures ();
		hcfg.EnableAlert (PXCMHandData.AlertType.ALERT_HAND_NOT_DETECTED);
		hcfg.ApplyChanges ();
		hcfg.Dispose ();

		InitObject ();
	}
	
	// Update is called once per frame
	void Update () {
		/* Make sure SenseManager Instance is valid */
		if (senseManager == null)
			return;
		
		/* Wait until any frame data is available */
		if (senseManager.AcquireFrame (false) != pxcmStatus.PXCM_STATUS_NO_ERROR)
			return;
		
		/* Retrieve hand tracking Module Instance */
		handAnalyzer = senseManager.QueryHand ();
		
		try
		{
			if (handAnalyzer != null) {
				/* Retrieve hand tracking Data */
				PXCMHandData _handData = handAnalyzer.CreateOutput ();
				if (_handData != null) {
					_handData.Update ();

					PXCMHandData.IHand[] _iHand = new PXCMHandData.IHand[MaxHands];
					NumOfHands = _handData.QueryNumberOfHands ();

					hands.isLeft = false;
					hands.isRight = false;

					/* Retrieve all joint Data */
					if (_handData.QueryHandData (PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS, 0, out _iHand[0]) == pxcmStatus.PXCM_STATUS_NO_ERROR) {
						/*Identify left/right hand */
						gesture[0].isExist = true;
						hands.isLeft = true;

						for (int i = 0; i <  _handData.QueryFiredGesturesNumber(); i++){
							//Debug.Log (i.ToString());
							if (_handData.QueryFiredGestureData (i, out gesture[0].gestureData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
								Debug.Log (gesture[0].gestureData.name);
								//continue;//If you want to use this gesture info, you need to realize a SEND function here.
						}
						for (int j = 0; j < MaxJoints; j++) {
							if (_iHand[0].QueryTrackedJoint ((PXCMHandData.JointType)j, out hands.jointData [0] [j]) != pxcmStatus.PXCM_STATUS_NO_ERROR)					
								hands.jointData [0] [j] = null;
						/*	hands.smoothPosition[0][j].AddSample(hands.jointData[0][j].positionWorld);
							hands.smoothLocalRotation[0][j].AddSample(((Quaternion)hands.jointData[0][j].localRotation).eulerAngles);
							hands.smoothGlobalRotation[0][j].AddSample(((Quaternion)hands.jointData[0][j].globalOrientation).eulerAngles);
						*/}

						if (!handList.ContainsKey (_iHand[0].QueryUniqueId ()))
							handList.Add (_iHand[0].QueryUniqueId (), _iHand[0].QueryBodySide ());
					}else{
						gesture[0].isExist = false;
					}

					if (_handData.QueryHandData (PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS, 0, out _iHand[1]) == pxcmStatus.PXCM_STATUS_NO_ERROR) {
						/*Identify left/right hand */	
						gesture[1].isExist = true;
						hands.isRight = true;

						for (int i = 0; i < _handData.QueryFiredGesturesNumber(); i++)
							if (_handData.QueryFiredGestureData (i, out gesture[1].gestureData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
								continue;//If you want to use this gesture info, you need to realize a SEND function here.

						for (int j = 0; j < MaxJoints; j++) {
							if (_iHand[1].QueryTrackedJoint ((PXCMHandData.JointType)j, out hands.jointData [1] [j]) != pxcmStatus.PXCM_STATUS_NO_ERROR)					
								hands.jointData [1] [j] = null;
						}
						
						if (!handList.ContainsKey (_iHand[1].QueryUniqueId ()))
							handList.Add (_iHand[1].QueryUniqueId (), _iHand[1].QueryBodySide ());
					}else{
						gesture[1].isExist = false;
					}
				}
				_handData.Dispose ();
			}
		}
		catch (IOException ex)
		{
			Console.WriteLine("An IOException has been thrown!");
			Console.WriteLine(ex.ToString());
			Console.ReadLine();
			return;
		}
		handAnalyzer.Dispose ();
		senseManager.ReleaseFrame ();
	}

	void InitObject(){
		gesture = new Gesture[MaxHands];

		for (int i = 0; i < MaxHands; i++) {
			smoother3D [i] = new PXCMDataSmoothing.Smoother3D[MaxJoints];
			gesture[i] = new Gesture();
		}
		
		for (int i = 0; i < MaxHands; i++)
		for (int j = 0; j < MaxJoints; j++) {
			smoother3D [i] [j] = dataSmoothing.Create3DWeighted (weightsNum);
		}
	}
}
