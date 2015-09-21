using UnityEngine;
using System.Collections;
using RSUnityToolkit;
using System.IO;
using System;

public class PXCMBodyData : MonoBehaviour {
	
	#region Public Fields
	
	/// <summary>
	/// The max depth value.
	/// </summary>
	public float MaxDepthVal = 90f;
	public Alert Status;	
	public JointData[] joints;
	public float ab_angle, ca_angle;
	public Vector3 ab, ca, old_ab, old_ca, cross,
			       dc, old_dc, cross2;
	
	public enum Alert
	{
		ALERT_HAND_NOT_DETECTED = 0,
		ALERT_SHOULDER_NOT_DETECTED = 1,
		ALERT_ELBOW_NOT_DETECTED = 2,
		ALERT_HAND_INFRONTOF_BODY = 3,
		ALERT_ARM_TRACKED
	}
	
	public class JointData
	{
		public PXCMPoint3DF32 positionWorld;
		public PXCMPoint4DF32 localRotation;
	}
	
	#endregion
	
	//游戏对象，这里是线段对象
	private GameObject LineRenderGameObject;
	
	//线段渲染器
	private LineRenderer lineRenderer;
	
	//设置线段的个数，标示一个曲线由几条线段组成
	private int lineLength = 4;
	
	//分别记录4个点，通过这4个三维世界中的点去连接一条线段
	private Vector3 v0 = new Vector3(0.0f,1.0f,0.0f);
	
	#region Private Fields
	
	private Vector3[] _vertices = null;		
	
	private double x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4;
	
	private PXCMSenseManager sm = null;
	private PXCMDataSmoothing.Smoother3D[] smoother3D= null; 
	private PXCMDataSmoothing.Smoother3D smoother4D= null;
	private PXCMDataSmoothing ds=null;
	private float Armlength = 33;
	
	private PXCMPoint4DF32 WristRotation;
	
	#endregion
	
	#region Private methods
	
	/// <summary>
	/// Sets the sense option according to the Stream field
	/// </summary>
	private void SetSenseOptions()
	{				
		SenseToolkitManager.Instance.SetSenseOption(SenseOption.SenseOptionID.PointCloud);	
		SenseToolkitManager.Instance.SetSenseOption (SenseOption.SenseOptionID.Hand);
		
	}
	/// <summary>
	/// Unsets the sense option according to the Stream field
	/// </summary>	
	private void UnsetSenseOptions()
	{			
		SenseToolkitManager.Instance.UnsetSenseOption(SenseOption.SenseOptionID.PointCloud);	
		SenseToolkitManager.Instance.UnsetSenseOption (SenseOption.SenseOptionID.Hand);
	}
	
	#endregion
	
	// Use this for initialization
	void Start () {
		sm = PXCMSenseManager.CreateInstance ();
		if (sm == null)
			Debug.LogError ("SenseManager Initialization Failed");
		
		sm.QuerySession ().CreateImpl<PXCMDataSmoothing> (out ds);
		joints = new JointData[4];
		smoother3D = new PXCMDataSmoothing.Smoother3D[4];
		
		LineRenderGameObject = GameObject.Find ("line2");
		lineRenderer = (LineRenderer)LineRenderGameObject.GetComponent ("LineRenderer");
		lineRenderer.SetVertexCount(lineLength);
		
		for (int i=0; i<=3; i++) 
		{
			joints [i] = new JointData ();
			smoother3D [i] = ds.Create3DWeighted (10);
		}
		smoother4D = ds.Create3DWeighted (10);
		var senseManager = GameObject.FindObjectOfType(typeof(SenseToolkitManager));
		if (senseManager == null)
		{
			Debug.LogWarning("Sense Manager Object not found and was added automatically");			
			senseManager = (GameObject)Instantiate(Resources.Load("SenseManager"));
			senseManager.name = "SenseManager";
		}
		
		SetSenseOptions();
		
	}
	
	// Update is called once per frame
	void Update () {
		Status = Alert.ALERT_ARM_TRACKED;
		
		if (SenseToolkitManager.Instance.PointCloud != null)
		{	
			int _gridSize = 3;
			int width = SenseToolkitManager.Instance.ImageDepthOutput.info.width/_gridSize;
			int height = SenseToolkitManager.Instance.ImageDepthOutput.info.height/_gridSize;
			//Debug.LogWarning("width:"+width.ToString());
			// Build vertices and UVs
			if (_vertices == null)
			{
				_vertices = new Vector3[width * height];
			}		
			
		/*	PXCMHandData _handData;
			PXCMHandData.IHand _iHand;
			PXCMHandData.JointData WristData;
			
			if (SenseToolkitManager.Instance.Initialized && SenseToolkitManager.Instance.HandDataOutput != null)
			{
				_handData = SenseToolkitManager.Instance.HandDataOutput;
				if (_handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_FIXED,0,out _iHand) == pxcmStatus.PXCM_STATUS_NO_ERROR)
				{
					if (_iHand.QueryTrackedJoint ((PXCMHandData.JointType)0, out WristData) == pxcmStatus.PXCM_STATUS_NO_ERROR)	
					{
						x1 = -WristData.positionWorld.x*100f;
						y1 = WristData.positionWorld.y*100f;
						z1 = WristData.positionWorld.z*100f;
					}
					if (_iHand.QueryTrackedJoint ((PXCMHandData.JointType)0, out WristData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
						WristRotation = WristData.localRotation;
				}else{
					Status = Alert.ALERT_HAND_NOT_DETECTED;
					return ;
				}
			}*/
			
			int i = 0;
			for (int y=0; y <  height; y++) 
			{
				for (int x=0; x <  width - 1; x++)  
				{				
					
					int j = y * width * _gridSize * _gridSize + x * _gridSize;
					
					_vertices [i].x  = SenseToolkitManager.Instance.PointCloud[j].x / 10;
					_vertices [i].y  = SenseToolkitManager.Instance.PointCloud[j].y / 10;
					_vertices [i].z  = -SenseToolkitManager.Instance.PointCloud[j].z / 10;		
					
					i++;
				}
			}
			
			if (!FindShoulder(height,width)) return ;
			if (!FindElbow(height,width)) return ;
			
		}
		
//		if (Status == Alert.ALERT_ARM_TRACKED)
			Status = CheckStatus ((float)x1,(float) y1,(float) z1,(float) x2, (float)y2,(float) z2,(float) x3,(float) y3, (float)z3
		                      , (float)x4, (float)y4, (float)z4);
	/*	lineRenderer.SetPosition (0, new Vector3 ((float)x1,(float)y1,(float)z1));
		lineRenderer.SetPosition (1, new Vector3 ((float)x3,(float)y3,(float)z3));
		lineRenderer.SetPosition (2, new Vector3 ((float)x2,(float)y2,(float)z2));*/
	}
	
	Alert CheckStatus(float x1,float y1, float z1,float x2, float y2, float z2, float x3, float y3, float z3, float x4, float y4, float z4)
	{
		Debug.Log("111 "+x4.ToString());
		old_ca = ca;
		old_ab = ab;
		old_dc = dc;
		joints [2].positionWorld = new Vector3 (x1, y1, z1);
		joints [1].positionWorld = new Vector3 (x3, y3, z3);
		joints [0].positionWorld = new Vector3 (x2, y2, z2);
		joints [3].positionWorld = new Vector3 (x4, y4, z4);
		for (int i=0; i<4; i++) {
			smoother3D [i].AddSample (joints [i].positionWorld);
			joints [i].positionWorld = new Vector3(
				smoother3D [i].GetSample ().x, smoother3D [i].GetSample ().y, smoother3D [i].GetSample ().z);
		}
		Debug.Log("222");
		lineRenderer.SetPosition (0, new Vector3(
			(float)smoother3D [2].GetSample ().x, (float)smoother3D [2].GetSample ().y, (float)smoother3D [2].GetSample ().z));
		lineRenderer.SetPosition (1, new Vector3(
			(float)smoother3D [1].GetSample ().x, (float)smoother3D [1].GetSample ().y, (float)smoother3D [1].GetSample ().z));
		lineRenderer.SetPosition (2, new Vector3(
			(float)smoother3D [0].GetSample ().x, (float)smoother3D [0].GetSample ().y, (float)smoother3D [0].GetSample ().z));
		lineRenderer.SetPosition (3, new Vector3(
			(float)smoother3D [3].GetSample ().x, (float)smoother3D [3].GetSample ().y, (float)smoother3D [3].GetSample ().z));
		Debug.Log("333");
		ab = (Vector3)joints [1].positionWorld - (Vector3)joints [2].positionWorld;
		ca = (Vector3)joints [0].positionWorld - (Vector3)joints [1].positionWorld;
		dc = (Vector3)joints [3].positionWorld - (Vector3)joints [0].positionWorld;
//		ca_angle = Mathf.Acos(Vector3.Dot (ca, old_ca) / (ca.magnitude * old_ca.magnitude));
		ca_angle = Mathf.PI - Mathf.Acos(Vector3.Dot (ca, dc) / (ca.magnitude * dc.magnitude));
		//ab_angle = Mathf.Acos(Vector3.Dot (ab, old_ab) / (ab.magnitude * old_ab.magnitude));
		ab_angle = Mathf.PI - Mathf.Acos(Vector3.Dot (ab, ca) / (ab.magnitude * ca.magnitude));
		//Debug.LogWarning ("AngleOfAncon: " + ab_angle.ToString ());
		
		cross = Vector3.Cross (old_dc, old_ca).normalized;
		joints[0].localRotation = new Quaternion (
			Mathf.Sin (ca_angle / 2) * cross.x, Mathf.Sin (ca_angle / 2) * cross.y,
			Mathf.Sin (ca_angle / 2) * cross.z, Mathf.Cos (ca_angle / 2));
		/*Vector3 oldEuler = ((Quaternion)joints [0].localRotation).eulerAngles;
		smoother4D.AddSample (oldEuler);
		Vector3 rotEuler = smoother4D.GetSample ();
		rotEuler.x = getNearestAngle (rotEuler.x, oldEuler.x);
		rotEuler.y = getNearestAngle (rotEuler.y, oldEuler.y);
		rotEuler.z = getNearestAngle (rotEuler.z, oldEuler.z);
		joints [0].localRotation = Quaternion.Euler (rotEuler);*/
		
		//cross = Vector3.Cross (ab, old_ab);
		cross = Vector3.Cross (old_ca, old_ab).normalized;
		//Debug.LogWarning ("cross: " + cross);
		//cross = new Vector3 (0, 0, 1);
		joints[1].localRotation = new Quaternion (
			Mathf.Sin (ab_angle / 2) * cross.x, Mathf.Sin (ab_angle / 2) * cross.y,
			Mathf.Sin (ab_angle / 2) * cross.z, Mathf.Cos (ab_angle / 2));
		//Debug.LogWarning ("rotEuler2:" + ((Quaternion)joints [1].localRotation).eulerAngles.ToString ());
		//smoother3D [1].AddSample (((Quaternion)joints [1].localRotation).eulerAngles);
		//joints [1].localRotation = Quaternion.Euler( smoother3D [1].GetSample ());
		//Debug.Log(ca_angle.ToString());
		
		/*cross = Vector3.Cross (ab, ca);
		joints[3].localRotation = new Quaternion (
			Mathf.Sin (angle / 2) * cross.x, Mathf.Sin (angle / 2) * cross.y,
			Mathf.Sin (angle / 2) * cross.z, Mathf.Cos (angle / 2));*/
		//	if (!(x1<x3 && x3<x2))
		joints [2].localRotation = WristRotation;
		
		return Alert.ALERT_ARM_TRACKED;
	}
	
	float getNearestAngle(float angle, float sample){
		while (Mathf.Abs (angle - sample) > 180f) {
			if (angle > sample)
				angle -= 360f;
			else 
				angle += 360f;
		}
		return angle;
	}
	
	bool FindShoulder(int height, int width)
	{
		int targetx = 0;
		Alert tmp = Status ;
		Status = Alert.ALERT_SHOULDER_NOT_DETECTED;
		
		for (int y = height - 3; y <= height - 2; y++)
		{
			int x;
			bool p = false;
		//	for (x = width - 2; x >= 0; x--)
			for (x = 0; x < width - 1; x++)
			{
				if (!IsEdge(x, y, width)) {
					x1 = -_vertices[y * width + x].x;
					y1 = _vertices[y * width + x].y;
					z1 = -_vertices[y * width + x].z;
					break;
				}
			}
//			//	Debug.Log(x);
//			for ( ; x >= 0; x--)
//			{
//				if (IsEdge(x, y, width) && IsEdge(x+1, y, width) && IsEdge(x+2, y, width) && IsEdge(x+3, y, width) && IsEdge(x+4, y, width)
//				    && IsEdge(x+5, y, width)) break;
//			}
//			//		Debug.Log(x);
//			for ( ; x >= 0; x--)
//			{
//				if (!IsEdge(x, y, width)) 
//				{
//					//				Debug.Log(x);
//					Status = Alert.ALERT_ELBOW_NOT_DETECTED;
//					return false;
//				}
//			}
//			if (p){
//				Status = tmp;
//				break;
//			}
		}

		for (int y = 10; y <= height - 2; y++) {
			int l,r;
			for (l = 5; l <= width - 1; l++)
				if (!IsEdge(l,y,width)) break;
			for (r = l; r <= width - 1; r++){
				if (IsEdge(r, y, width) && IsEdge(r-1, y, width) && IsEdge(r-2, y, width) && IsEdge(r-3, y, width) && IsEdge(r-4, y, width)
					&& IsEdge(r-5, y, width)) break;
			}
		//	Debug.Log("distance:"+Math.Abs(_vertices[y * width + l].x-_vertices[y * width + r].x).ToString());
			if (Math.Abs(_vertices[y * width + l].x-_vertices[y * width + r].x) > 20)
			{
				x2 = -_vertices[y * width + l].x;
				y2 = _vertices[y * width + l].y;
				z2 = -_vertices[y * width + l].z;
				x4 = x2 - 10;
				y4 = y2;
				z4 = z2;
				break;
			}
		}

		
//		tmp = Status;
//		Status = Alert.ALERT_SHOULDER_NOT_DETECTED;
//		bool[] pan = new bool[1111];
//		for (int y = height - 30; y >= 0 ; y--)
//		{
//			int x = targetx - 5;
//			pan[y] = IsEdge(x, y, width);
//			if (pan[y] && pan[y+1] && pan[y+2] && pan[y+3] && pan[y+4] && pan[y+5] && pan[y+6])
//			{
//				y = y + 16;
//				x2 = -_vertices[y * width + x].x;
//				y2 = _vertices[y * width + x].y;
//				z2 = -_vertices[y * width + x].z;
//				Status = tmp;
//				break;
//			}
//			
//		}
		
		return true;
		
	}
	
	bool FindElbow(int height, int width)
	{
		bool backGroundTriangles = false;
		double xx,yy,zz;
		double Max = 0, temp;
		
		for ( int y = 0; y < height - 1 ; y++ ) 
		{
			for ( int x = 0; x < width - 1; x++ ) 
			{	
				backGroundTriangles = (
					( Mathf.Abs(_vertices[y * width + x].z) > MaxDepthVal ) || 
					( Mathf.Abs(_vertices[y * width + x + 1].z) > MaxDepthVal ) || 
					( Mathf.Abs(_vertices[(y + 1) * width + x].z )> MaxDepthVal ) || 
					( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) > MaxDepthVal ) 
					);
				
				backGroundTriangles = backGroundTriangles || IsEdge(x, y, width);
				if (!backGroundTriangles) {
					xx = -_vertices[y * width + x].x;
					yy = _vertices[y * width + x].y;
					zz = -_vertices[y * width + x].z;
					if (Mathf.Abs((float)(y1-y2)) > Armlength)
						if (Mathf.Abs((float)Mathf.Abs((float)(yy-y1))-(float)Mathf.Abs((float)(y2-yy))) > 0.4)
							continue;
					if (yy > y2 || xx < x2+10) continue;
					
					if ((temp = calc(xx,yy,zz,x1,y1,z1,x2,y2,z2)) > Max)
					{
						Max = temp;
						x3 = xx;
						y3 = yy;
						z3 = zz;
					}
				}
			}
		}
		return true;
	}
	
	double calc(double x1, double y1, double z1,double x2, double y2, double z2, double x3, double y3, double z3)
	{
		double d1 = Math.Sqrt (Math.Pow (x1 - x2, 2) + Math.Pow (y1 - y2, 2) + Math.Pow (z1 - z2, 2));
		double d2 = Math.Sqrt (Math.Pow (x1 - x3, 2) + Math.Pow (y1 - y3, 2) + Math.Pow (z1 - z3, 2));
		double d3 = Math.Sqrt (Math.Pow (x3 - x2, 2) + Math.Pow (y3 - y2, 2) + Math.Pow (z3 - z2, 2));
		double p = (d1 + d2 + d3) / 2.0;
		return Math.Sqrt (p * (p - d1) * (p - d2) * (p - d3));
	}
	
	bool IsEdge(int x, int y, int width)
	{
		return  (Mathf.Abs (_vertices [y * width + x].z) == 0) || 
			(Mathf.Abs (_vertices [y * width + x + 1].z) == 0) || 
				(Mathf.Abs (_vertices [(y + 1) * width + x].z) == 0) || 
				(Mathf.Abs (_vertices [(y + 1) * width + x + 1].z) == 0) ||
				(Mathf.Abs (_vertices [(y - 1) * width + x].z) == 0) ||
				(Mathf.Abs (_vertices [(y - 1) * width + x + 1].z) == 0);
	}
	
	//On enable set sense options
	void OnEnable()
	{
		if (SenseToolkitManager.Instance == null)
		{
			return;
		}
		
		SetSenseOptions();
	}
	
	//On disable unset sense options
	void OnDisable()
	{
		UnsetSenseOptions();
	}
}