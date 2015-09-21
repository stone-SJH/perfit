
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;

public class BaseArmMatching : BaseActMatching {
	[HideInInspector]
	public new PXCMArmData dm;
	protected Status lststatus = 0;
	protected double MaxAngle = 4;
	protected String FileName = "armtime.txt";

	// Use this for initialization
	void Start () {
		FileName = "armtime-"+DateTime.UtcNow.ToString()+".txt";
		args = FileName.Split(new char[3]{'/',' ',':'});
		FileName = args[0]+"-"+args[1]+"-"+args[2]+"-"+args[3]+"-"+args[4]+"-"+args[5]+"-"+args[6];
		fs = new FileStream (FileName, FileMode.Create);
		sw = new StreamWriter (fs);
		sw.AutoFlush = true;
		args = SceneManager.Global.Armplan.Split('-');
		point = 0;
		dm = GameObject.Find ("DataManager").GetComponent<PXCMArmData>();
	}
	
	// Update is called once per frame
	void Update () {
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
		lststatus = lhstatus;
		getStatus(dm.joints, true);
		if (lhstatus !=Status.None && lhstatus != Status.Done && lhstatus > BestPerform)
			BestPerform = lhstatus;
		if (lststatus == Status.Prepared && lhstatus != Status.Prepared) {
			lst = DateTime.UtcNow;
			label2.GetComponent<UILabel>().text += string.Format("{0}: ",count++);
			BestPerform = Status.Bad;
			IsDetecting = false;
		}
		if (lststatus != Status.Done && lhstatus == Status.Done) {
			ts = DateTime.UtcNow - lst;
			sw.WriteLine(Convert.ToSingle(ts.TotalSeconds).ToString()+" "+MaxAngle.ToString()+" "+BestPerform);
			label2.GetComponent<UILabel>().text += BestPerform.ToString();
			label2.GetComponent<UILabel>().text += '\n';
			IsDetecting = true;
			if (count == Convert.ToInt32(args[point])){
				IsRelaxing = true;
				now = DateTime.UtcNow;
				lockBG.SetActive(true);
				if (point == 1){
					SceneManager.Global.ArmTag = true;
					endMenu.GetComponent<TweenPosition>().PlayForward();
					label1.GetComponent<UILabel>().text = "完成计划！";
					IsFinished = true;
					string ret;
					fs.Close();
					sw.Close();
					Connection.getInstance().ShoulderHandle(FileName,out ret);
				}
				return ;
			}
		}
	}
	
	#region Arm Data
	
	protected virtual void getStatus(PXCMArmData.JointData[] data, bool isLeft){}
	protected virtual Status checkMotion(PXCMArmData.JointData[] data){
		return Status.None;
	}
	protected virtual bool isPrepared(PXCMArmData.JointData[] data){
		return false;
	}
	protected virtual bool isBad(PXCMArmData.JointData[] data){
		return false;
	}
	protected virtual bool isGood(PXCMArmData.JointData[] data){
		return false;
	}
	protected virtual bool isGreat(PXCMArmData.JointData[] data){
		return false;
	}
	protected virtual bool isDone(PXCMArmData.JointData[] data){
		return false;
	}
	#endregion

}
