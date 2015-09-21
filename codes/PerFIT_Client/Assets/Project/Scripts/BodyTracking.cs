using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using RSUnityToolkit;

public class BodyTracking : HandsTracking {
	public GameObject[] arms;
	public GameObject target,body;
	private PXCMBodyData ad;
	public GUIText myText;
	private double angle,angle2;

	void OnEnable(){
		RunMode = SceneManager.Global.RunMode;
		if (RunMode == MCTTypes.RunModes.PlayFromFile) {
			//CHECK IF FILE EXISTS
			if (!System.IO.File.Exists (FilePath)) {
				Debug.LogWarning ("No Filepath Set Or File Doesn't Exist, Run Mode Will Be Changed to Live Stream");
				RunMode = MCTTypes.RunModes.LiveStream;
			} else {
				fs = new FileStream(FilePath, FileMode.Open);
				sr = new StreamReader(fs);
			}
		}
		
		if (RunMode == MCTTypes.RunModes.RecordToFile) {
			CreateFileName("Body");
			fs = new FileStream(FilePath, FileMode.Create);
			sw = new StreamWriter(fs);
		}
	}

	// Use this for initialization
	void Start () {
		ad = GameObject.Find ("DataManager").GetComponent<PXCMBodyData>();	
		arms = new GameObject[6];
		target = new GameObject ();
		body = new GameObject ();
		/*target = GameObject.Find ("Arms").transform.Find ("arms").gameObject;
		arms [0] = target.transform.Find ("spine").transform.Find ("LeftShoulder").transform.Find ("LeftArm").gameObject;
		arms [1] = arms [0].transform.Find ("LeftForeArm").gameObject;
		arms [2] = arms [1].transform.Find ("LeftForeArmRoll").transform.Find ("LeftHand").gameObject;
		arms [3] = target.transform.Find ("spine").transform.Find ("frArms_skinMale_RightShoulder").transform.Find ("frArms_skinMale_RightArm").gameObject;
		arms [4] = arms [3].transform.Find ("frArms_skinMale_RightForeArm").gameObject;
		arms [5] = arms [4].transform.Find ("frArms_skinMale_RightForeArmRoll").transform.Find ("frArms_skinMale_RightHand").gameObject;*/
		body = GameObject.Find ("Ethan").gameObject;
		target = GameObject.Find ("Ethan").transform.Find ("EthanSkeleton").transform.Find("EthanHips").transform.Find ("EthanSpine")
			.transform.Find ("EthanSpine1").transform.Find ("EthanSpine2").transform.Find ("EthanNeck").gameObject;
		arms [0] = target.transform.Find ("EthanLeftShoulder").transform.Find ("EthanLeftArm").gameObject;
		arms [1] = arms [0].transform.Find ("EthanLeftForeArm").gameObject;
		arms [2] = arms [1].transform.Find ("EthanLeftHand").gameObject;
		arms [3] = target.transform.Find ("EthanRightShoulder").transform.Find ("EthanRightArm").gameObject;
		arms [4] = arms [3].transform.Find ("EthanRightForeArm").gameObject;
		arms [5] = arms [4].transform.Find ("EthanRightHand").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//		myText.text = ad.Status.ToString();
		
		Quaternion temp = ad.joints [1].localRotation;
		Quaternion temp2 = ad.joints [0].localRotation;
		Quaternion temp3 = ad.joints [1].localRotation;
		Quaternion temp4 = ad.joints [0].localRotation;
		Vector3 rotEuler = new Vector3();
		Vector3 rotEuler2 = new Vector3 ();
		angle = 2 * Math.Acos(ad.joints[1].localRotation.w);
		angle2 = 2 * Math.Acos(ad.joints[0].localRotation.w);
//		myText.text = angle.ToString ();
		if (RunMode == MCTTypes.RunModes.PlayFromFile) {
			string[] args = sr.ReadLine().Split();
			temp.x = Convert.ToSingle(args[0]);
			temp.y = Convert.ToSingle(args[1]);
			temp.z = Convert.ToSingle(args[2]);
			temp.w = Convert.ToSingle(args[3]);
			rotEuler.x = Convert.ToSingle(args[4]);
			rotEuler.y = Convert.ToSingle(args[5]);
			rotEuler.z = Convert.ToSingle(args[6]);
		} else {
			temp.x = (float)Math.Sin ((Math.PI-angle)/2.0);
			temp.y = 0;
			temp.z = 0;
			temp.w = (float)Math.Cos ((Math.PI-angle)/2.0);
	/*		temp2.x = 0;
			temp2.y = 0;
			temp2.z = (float)Math.Sin ((Math.PI-angle2)/2.0);
			temp2.w = (float)Math.Cos ((Math.PI-angle2)/2.0);*/
			rotEuler = temp2.eulerAngles;
			rotEuler.x = 100f-rotEuler.y;
			rotEuler.y = -45f;
			rotEuler.z = 0;
		}
		Debug.Log (temp.ToString ());
		if (RunMode == MCTTypes.RunModes.RecordToFile)
			sw.WriteLine ("{0} {1} {2} {3} {4} {5} {6}", 
			              temp.x.ToString("f4"), temp.y.ToString("f4"), temp.z.ToString("f4"), temp.w.ToString("f4"), 
			              rotEuler.x.ToString("f4"), rotEuler.y.ToString("f4"), rotEuler.z.ToString("f4"));
		arms [1].transform.localRotation = (PXCMPoint4DF32)temp;
		arms [0].transform.localRotation = Quaternion.Euler (rotEuler);


		temp3.x = -temp.x;temp3.y = -temp.y;temp3.z = -temp.z;temp3.w = temp.w;
		rotEuler2 = -rotEuler;
		arms [4].transform.localRotation = (PXCMPoint4DF32)temp3;
		arms [3].transform.localRotation = Quaternion.Euler (rotEuler2);
		body.transform.position = new Vector3 ((float)3.372, (float)angle/(float)10.0, (float)0.543);
		Debug.LogWarning (rotEuler.x+" "+rotEuler.y+" "+rotEuler.z);
//		camera.transform.position = new PXCMPoint3DF32(
//			camera.transform.position.x, arms [2].transform.position.y, camera.transform.position.z);
//		arms [2].transform.localRotation = new PXCMPoint4DF32 (0, (float)Math.Sin(-0.8), 0,(float)Math.Cos(-0.8));
	}
	
	void OnDisable(){
		sw.Close ();
		fs.Close ();
	}
}