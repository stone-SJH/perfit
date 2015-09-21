using UnityEngine;
using System.Collections;
using System;
using Client.FileOp;
using System.IO;
using System.Text;
using System.Linq; 
using System.Collections.Generic;

public class AnalysisManager : MonoBehaviour {
	public enum planType {lefthand, righthand, shoudler, body}
	public planType choice = 0;
	string[] Infos;
	string Info;

	Dictionary<int,string> dic = new Dictionary<int,string>();
	// Use this for initialization
	string[] lh_name = {
		"大拇指平均得分","食指平均得分","中指平均得分","无名指平均得分","小拇指平均得分",
		"历史平均时间","大拇指最高得分","食指最高得分","中指最高得分","无名指最高得分","小拇指最高得分"
	};
	string[] arm_name = {
		"平均完成度","平均稳定性","平均波动程度","平均时间","最高完成度",
		"最高稳定性得分","最高波动程度得分"
	};
	string[] body_name = {
		"总个数","总时长","平均肘部弯曲得分","平均肩部伸直得分","平均动作完成总得分",
		"最高肘部弯曲得分","最高肩部伸直得","最高动作完成总得分"
	};

	public void HoverOver(GameObject obj){
		int number = int.Parse (obj.name);
		string detail = dic [number].Substring(0,5);
		transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
	}

	public void HoverOut(GameObject obj){
		int number = int.Parse (obj.name);
		if (choice == planType.lefthand || choice == planType.righthand) {
			string detail = lh_name [number];
			transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
		} else if (choice == planType.shoudler) {
			string detail = arm_name [number];
			transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
		} else {
			string detail = body_name [number+1];
			transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
		}
	}

	void Start () {
		FileOperation fo;
		if (choice == planType.lefthand)
			fo = new FileOperation ("lefthand_history.txt");
		else if (choice == planType.righthand)
			fo = new FileOperation ("righthand_history.txt");
		else if (choice == planType.shoudler)
			fo = new FileOperation ("shoulder_history.txt");
		else
			fo = new FileOperation ("pushup_history.txt");
		
		List<byte[]> ret = new List<byte[]> ();
		Info = "";
		fo.ReadContentBinary (out ret);
		for (int i = 0; i < ret.Count; i++) {
			Info += Encoding.Default.GetString(ret[i], 0, ret[i].Length);
		}
		Infos = Info.Split ('\n');
		Debug.Log (Info);

		if (choice == planType.lefthand || choice == planType.righthand) {
			for (int i = 1; i < 6; i++) {
				Debug.LogWarning (Infos [i]);
				dic.Add (i, Infos [i]);
				transform.Find (i.ToString ()).Find ("Title").GetComponent<UILabel> ().text = lh_name [i];
			}
			transform.Find ("Time").GetComponent<UILabel> ().text = Infos [6].Substring (0, 4);
			for (int i = 8; i < 13; i++) {
				Debug.LogWarning (Infos [i]);
				dic.Add (i - 2, Infos [i]);
				transform.Find ((i - 2).ToString ()).Find ("Title").GetComponent<UILabel> ().text = lh_name [i - 2];
			}
		} else if (choice == planType.shoudler) {
			for (int i = 1; i < 4; i++) {
				Debug.LogWarning (Infos [i]);
				dic.Add (i, Infos [i - 1]);
				transform.Find (i.ToString ()).Find ("Title").GetComponent<UILabel> ().text = arm_name [i];
			}
			transform.Find ("Time").GetComponent<UILabel> ().text = Infos [3].Substring (0, 4);
			for (int i = 5; i < 8; i++) {
				Debug.LogWarning (Infos [i]);
				dic.Add (i - 1, Infos [i - 1]);
				transform.Find ((i - 1).ToString ()).Find ("Title").GetComponent<UILabel> ().text = arm_name [i - 1];
			}
		} else if (choice == planType.body) {
			transform.Find ("Time").GetComponent<UILabel> ().text = Infos [0];
			for(int i = 1; i < 7; i++){
				dic.Add(i,Infos[i+1]);
				transform.Find (i.ToString ()).Find ("Title").GetComponent<UILabel> ().text = body_name [i+1];
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
