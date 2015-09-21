using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using RSUnityToolkit;

public class HandsTracking : MonoBehaviour {
	[HideInInspector]
	public DataManager dm;

	public MCTTypes.RunModes RunMode;
	public String FilePath = "";
	protected string[] args;
	protected GameObject[][] Fingers;
	protected Quaternion fixedData1 = Quaternion.AngleAxis (90f, Vector3.up) * Quaternion.AngleAxis (450f, Vector3.forward);
	protected Quaternion fixedData2 = Quaternion.AngleAxis (90f, Vector3.up) * Quaternion.AngleAxis (270f, Vector3.forward);

	protected StreamWriter sw;
	protected StreamReader sr;
	protected FileStream fs;
	private bool IsFinished = false;

	public void CloseStream(){
		fs.Close ();
		sw.Close ();
		sr.Close ();
	}

	float getNearestAngle(float angle, float sample){
		while (Mathf.Abs (angle - sample) > 200f) {
			if (angle > sample)
				angle -= 360f;
			else 
				angle += 360f;
		}
		return angle;
	}

	protected void CreateFileName(String str){
		FilePath = str+"-"+DateTime.UtcNow.ToString();
		args = FilePath.Split(new char[3]{'/',' ',':'});
		FilePath = args[0]+"-"+args[1]+"-"+args[2]+"-"+args[3]+"-"+args[4]+"-"+args[5];
	}

	void OnEnable(){
		RunMode = SceneManager.Global.RunMode;
		if (RunMode == MCTTypes.RunModes.PlayFromFile) {
			//CHECK IF FILE EXISTS
			FilePath = SceneManager.Global.RecordName;
			fs = new FileStream(FilePath, FileMode.Open);
			sr = new StreamReader(fs);
		}
		
		if (RunMode == MCTTypes.RunModes.RecordToFile) {
			CreateFileName("Hand");
			fs = new FileStream(FilePath, FileMode.Create);
			sw = new StreamWriter(fs);
		}
	}

	void OnDisable(){
		Debug.LogWarning("success!");
		IsFinished = true;
		fs.Close ();
		sw.Close ();
		sr.Close ();
	}

		// Use this for initialization
	void Start () {
		dm = GameObject.Find ("DataManager").GetComponent<DataManager>();
		RunMode = SceneManager.Global.RunMode;
		InitializeGameobjects ();
	}
	
	protected void InitializeGameobjects ()
	{
		//Initalize
		Fingers = new GameObject[dm.MaxHands][];
		Fingers[0] = new GameObject[dm.MaxJoints];
		Fingers[1] = new GameObject[dm.MaxJoints];
		Fingers [0] [0] = this.gameObject.transform.Find ("L_Wrist").gameObject;
		Fingers [1] [0] = this.gameObject.transform.Find ("R_Wrist").gameObject;
		for (int i = 0; i < dm.MaxHands; i++) {
			for (int j = 0; j < 5; j++){
				Fingers[i][j*4+2] = Fingers[i][0].transform.Find((j*4+2).ToString()).gameObject;
				Fingers[i][j*4+3] = Fingers[i][j*4+2].transform.Find ((j*4+3).ToString()).gameObject;
				Fingers[i][j*4+4] = Fingers[i][j*4+3].transform.Find ((j*4+4).ToString()).gameObject;
				Fingers[i][j*4+5] = Fingers[i][j*4+4].transform.Find ((j*4+5).ToString()).gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		try
		{
			if (IsFinished) return ;
			for (int i = 0; i < dm.MaxHands; i++){
				if (dm.hands.jointData [i] [0] != null && dm.hands.jointData [i] [0].confidence == 100) {
					//test is used for wrist to move -- location
					Vector3 test = dm.hands.jointData[i][0].positionWorld;// - Fingers[i][0].gameObject.transform.position;

					//rotEuler is used for wrist to rotate -- rotation
					Vector3 rotEuler = ((Quaternion) dm.hands.jointData [i] [0].globalOrientation).eulerAngles;
					dm.smoother3D[i][0].AddSample(rotEuler);
					Vector3 sample = dm.smoother3D[i][0].GetSample();

					rotEuler.x = getNearestAngle(rotEuler.x, sample.x);
					rotEuler.y = getNearestAngle(rotEuler.y, sample.y);
					rotEuler.z = getNearestAngle(rotEuler.z, sample.z);

					if (dm.hands.isLeft && i == 0){
						//transform.Translate(test); //This doesn't work.
						//this.gameObject.transform.Find("L_Wrist").transform.position = ( new Vector3(-test.x , test.y, test.z) * 100f); //This worked, but not so good.
						this.gameObject.transform.Find("L_Wrist").transform.localRotation = Quaternion.Euler(new Vector3(-rotEuler.x, -rotEuler.y, rotEuler.z)) * fixedData1;
					}
					if (dm.hands.isRight && i == 1){
						//transform.Translate(test);
						//this.gameObject.transform.Find("R_Wrist").transform.position = ( new Vector3(-test.x , test.y, test.z) * 100f);
						this.gameObject.transform.Find("R_Wrist").transform.localRotation = Quaternion.Euler(new Vector3(-rotEuler.x, -rotEuler.y, rotEuler.z)) * fixedData2;
					}
				} else {
					Fingers [i] [0].SetActive (false);
				}

			}
			//down here is where this program truly run.
			updateBones();
			if(RunMode != MCTTypes.RunModes.PlayFromFile)
				checkExistence();
		}
		catch (IOException ex)
		{
			Console.WriteLine("An IOException has been thrown!");
			Console.WriteLine(ex.ToString());
			Console.ReadLine();
			return;
		}
	}

	//If hands is out of the camera, it shouldn't show on the scene.
	protected void checkExistence(){
		this.gameObject.transform.Find("hand_left").gameObject.SetActive(true);
		this.gameObject.transform.Find("hand_right").gameObject.SetActive(true);
	
		if (!dm.gesture[1].isExist) {
			this.gameObject.transform.Find("hand_left").gameObject.SetActive(false);
		}
		if (!dm.gesture[0].isExist) {
			this.gameObject.transform.Find("hand_right").gameObject.SetActive(false);
		}
	}

	//The function used to update the fingers' movement, joints from 3~21.
	//Joint 0 is Wrist, 1 is not existed in this model, 2 will make some troubles.
	protected void updateBones()
	{
		for (int i = 0; i < dm.MaxHands; i++) 
		for (int j = 0; j < dm.MaxJoints; j++) {
			if (j == 1 || j == 0)
				continue;
			if (RunMode == MCTTypes.RunModes.PlayFromFile && j != 2 && j != 3 && j != 6 && j != 10 && j != 14 && j != 18 && sr.Peek() > 0){
				Vector3 rotEuler;
				string[] args = sr.ReadLine().Split();
				int recordi = Convert.ToInt32(args[0]);
				int recordj = Convert.ToInt32(args[1]);
				rotEuler.x = Convert.ToSingle(args[2]);
				rotEuler.y = Convert.ToSingle(args[3]);
				rotEuler.z = Convert.ToSingle(args[4]);
				Fingers [recordi] [recordj - 1].transform.localRotation = Quaternion.Euler (rotEuler);
			}
			else if (dm.hands.jointData [i] [j] != null && dm.hands.jointData [i] [j].confidence == 100) {
				Fingers [i] [j].SetActive (true);
				if (j != 2 && j != 3 && j != 6 && j != 10 && j != 14 && j != 18) {
					Vector3 rotEuler = ((Quaternion)dm.hands.jointData [i] [j].localRotation).eulerAngles;

					/*Some mystery realization*/
					if (rotEuler.x < 300f)
						rotEuler.z = Mathf.Clamp (-rotEuler.x, -80f, 0);
					else
						rotEuler.z = Mathf.Clamp (rotEuler.x - 360, -80f, 0);
			
					rotEuler.y = Mathf.Clamp (-rotEuler.y, -6f, 6f);
					rotEuler.x = 0;

					dm.smoother3D [i] [j].AddSample (rotEuler);
					rotEuler = dm.smoother3D [i] [j].GetSample ();

					if (RunMode == MCTTypes.RunModes.RecordToFile) 
						sw.WriteLine ("{0} {1} {2} {3} {4}", i,j,rotEuler.x, rotEuler.y, rotEuler.z);
					Fingers [i] [j - 1].transform.localRotation = Quaternion.Euler (rotEuler);
				}
				dm.hands.jointData [i] [j] = null;
			} else {
				Fingers [i] [j].SetActive (false);
			}
		}
	}
}