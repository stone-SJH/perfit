using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;

public class BaseActMatching : MonoBehaviour {
	public DataManager dm;
	public GUIText myTextLeft;//GuiText for Left hand
	public GUIText myTextRight;//Pointer for Right hand
	public GameObject label1, label2;
	public GameObject lockBG, endMenu;

	protected double MaxDelta = 0.015;
	protected double deviation = 0.3;
	protected Status lhstatus = 0; // Left hand status
	protected Status rhstatus = 0; // Right hand status
	protected Status BestPerform = Status.Bad;

	protected FileStream fs;
	protected StreamWriter sw;
	protected String FileName,MovieName;
	protected DateTime lst,now;
	protected TimeSpan ts;
	protected int point;
	protected string[] args;
	protected bool IsRelaxing = false, IsFinished = false;

	protected int count = 0;
	protected Boolean IsDetecting = false;
	protected enum Status{
		Prepared = 0,
		Bad = 1,
		Good = 2,
		Great = 3,
		Done = 4,
		None
	}
	// Use this for initialization
	void Start () {
		dm = GameObject.Find ("DataManager").GetComponent<DataManager>();
		FileName = "handtime-"+DateTime.UtcNow.ToString()+".txt";
		args = FileName.Split(new char[3]{'/',' ',':'});
		FileName = args[0]+"-"+args[1]+"-"+args[2]+"-"+args[3]+"-"+args[4]+"-"+args[5]+"-"+args[6];
		fs = new FileStream (FileName, FileMode.Create);
		sw = new StreamWriter (fs);
		sw.AutoFlush = true;
		args = SceneManager.Global.Handplan.Split('-');
		point = 0;
	}
	
	// Update is called once per frame
	void Update () {
		try
		{
			if (IsFinished) return ;
			if (IsRelaxing){
				if (((TimeSpan)(DateTime.UtcNow-now)).TotalSeconds > 5f){
					IsRelaxing = false;
					count = 0;
					point++;
					lockBG.SetActive(false);
				}else{
					label1.GetComponent<UILabel>().text = "休息5秒："+((TimeSpan)(DateTime.UtcNow-now)).TotalSeconds.ToString("f4");
					return ;
				}
			}
			for (int i = 0; i < dm.NumOfHands; i++) {
				string name = dm.gesture[i].gestureData.name;
				if (name.Equals("spreadfingers")){ 
					if (!IsDetecting) {
						lst = DateTime.UtcNow;
						count++;
						IsDetecting = true;
						label2.GetComponent<UILabel>().text += string.Format("{0}:",count);
						BestPerform = Status.Bad;
					}
				}else if ((name.Equals("fist") || name.Equals("full_pinch")) && IsDetecting){
					ts = DateTime.UtcNow - lst;
					sw.WriteLine("0 "+BestPerform+" "+Convert.ToSingle(ts.TotalSeconds).ToString());
					label2.GetComponent<UILabel>().text += BestPerform.ToString();
					label2.GetComponent<UILabel>().text += '\n';
					IsDetecting = false;
					if (count == Convert.ToInt32(args[point])){
						IsRelaxing = true;
						now = DateTime.UtcNow;
						lockBG.SetActive(true);
						if (point == 3){
							SceneManager.Global.HandTag = true;
							endMenu.GetComponent<TweenPosition>().PlayForward();
							label1.GetComponent<UILabel>().text = "完成计划！";
							IsFinished = true;
//							MovieName = FileName.Remove(FileName.Length-4,4);
//							MovieName = MovieName.Replace("h","H");
							string ret;
							fs.Close();
							sw.Close();
							GameObject movie = GameObject.Find("Hands").gameObject;
							movie.SetActive(false);
							MovieName = movie.GetComponent<HandsTracking>().FilePath;
	//						movie.GetComponent<HandsTracking>().CloseStream();
							Debug.LogWarning (MovieName.ToString());
							Connection.getInstance().HandHandle(MovieName,FileName,out ret);
						}
						return ;
					}
				}
					
				if (dm.hands.jointData != null) {
					getStatus(dm.hands.jointData[i], dm.hands.isLeft);
				}
			}
		}
		catch (IOException ex)
		{
			Console.WriteLine("An IOException has been thrown!");
			Console.WriteLine(ex.ToString());
			Console.ReadLine();
			return;
		}
	}

	protected virtual void getStatus(PXCMHandData.JointData[] data, bool isLeft){
		if (isLeft)
			lhstatus = checkMotion (data);
		else
			rhstatus = checkMotion (data);
	}

	protected virtual Status checkMotion(PXCMHandData.JointData[] data){
		if (isPrepared (data))
			return Status.Prepared;
		if (isBad (data))
			return Status.Bad;
		if (isGood (data))
			return Status.Good;
		if (isGreat (data))
			return Status.Great;
		if (isDone (data))
			return Status.Done;
		return Status.None;
	}

	protected virtual bool isPrepared(PXCMHandData.JointData[] data){
		return false;
	}
	protected virtual bool isBad(PXCMHandData.JointData[] data){
		return false;
	}
	protected virtual bool isGood(PXCMHandData.JointData[] data){
		return false;
	}
	protected virtual bool isGreat(PXCMHandData.JointData[] data){
		return false;
	}
	protected virtual bool isDone(PXCMHandData.JointData[] data){
		return false;
	}
}
