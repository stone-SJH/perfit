using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using RSUnityToolkit;

public class ArmTracking : HandsTracking {
	public GameObject[] arms;
	public GameObject target;
	private PXCMArmData ad;
	public GUIText myText;
	private double angle;

	new void CreateFileName(){
		FilePath = "Arm-"+DateTime.UtcNow.ToString();
		args = FilePath.Split(new char[3]{'/',' ',':'});
		FilePath = args[0]+"-"+args[1]+"-"+args[2]+"-"+args[3]+"-"+args[4]+"-"+args[5];
	}

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
			CreateFileName("Arm");
			fs = new FileStream(FilePath, FileMode.Create);
			sw = new StreamWriter(fs);
		}
	}

	// Use this for initialization
	void Start () {
		ad = GameObject.Find ("DataManager").GetComponent<PXCMArmData>();	
		arms = new GameObject[3];
		target = new GameObject ();
		target = GameObject.Find ("Arms").transform.Find ("arms").gameObject;
		arms [0] = target.transform.Find ("spine").transform.Find ("LeftShoulder").transform.Find ("LeftArm").gameObject;
		arms [1] = arms [0].transform.Find ("LeftForeArm").gameObject;
		arms [2] = arms [1].transform.Find ("LeftForeArmRoll").transform.Find ("LeftHand").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
//		myText.text = ad.Status.ToString();
		
		Quaternion temp = ad.joints [1].localRotation;;
		Vector3 rotEuler;
	/*	Debug.LogWarning ("rotEuler: " + rotEuler.ToString());
		rotEuler.x = rotEuler.x/2f;
		rotEuler.y = Mathf.Clamp (rotEuler.y, -3f, 3f);
		rotEuler.z = Mathf.Clamp (rotEuler.z - 225f, -45f, -30f);
		
		Debug.LogWarning ("trueRotEuler: " + rotEuler.ToString ());
		arms [0].transform.localRotation = Quaternion.Euler (rotEuler);*/
/*		if (RunMode == MCTTypes.RunModes.PlayFromFile) {
			string[] args = sr.ReadLine().Split();
			rotEuler.x = Convert.ToSingle(args[0]);
			rotEuler.y = Convert.ToSingle(args[1]);
			rotEuler.z = Convert.ToSingle(args[2]);
		} else {
			temp = ad.joints [1].localRotation;
			rotEuler = temp.eulerAngles;
			rotEuler.x = 0;
			rotEuler.y = 0;
			rotEuler.z = -200f + rotEuler.z;
		}
		if (RunMode == MCTTypes.RunModes.RecordToFile)
			sw.WriteLine ("{0} {1} {2}", rotEuler.x, rotEuler.y, rotEuler.z);
		arms [1].transform.localRotation = Quaternion.Euler (rotEuler);*/
		angle = 2 * Math.Acos(ad.joints[1].localRotation.w);
		if (RunMode == MCTTypes.RunModes.PlayFromFile) {
			string[] args = sr.ReadLine().Split();
			temp.x = Convert.ToSingle(args[0]);
			temp.y = Convert.ToSingle(args[1]);
			temp.z = Convert.ToSingle(args[2]);
			temp.w = Convert.ToSingle(args[3]);
		} else {
			temp.x = 0;
			temp.y = 0;
			temp.z = (float)Math.Sin ((Math.PI-angle)/2.0);
			temp.w = (float)Math.Cos ((Math.PI-angle)/2.0);
		}
		Debug.Log (temp.ToString ());
		if (RunMode == MCTTypes.RunModes.RecordToFile)
			sw.WriteLine ("{0} {1} {2} {3}", temp.x, temp.y, temp.z, temp.w);
		arms [1].transform.localRotation = (PXCMPoint4DF32)temp;
	}

	void OnDisable(){
		sw.Close ();
		fs.Close ();
	}
}