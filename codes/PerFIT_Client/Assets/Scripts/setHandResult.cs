using UnityEngine;
using System.Collections;
using Client.FileOp;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class setHandResult : MonoBehaviour {
	public enum planType {left,right};
	public planType choice = 0;
	
	string Info = "";
	string[] Infos;
	Dictionary<int,string> dic = new Dictionary<int,string>();
	// Use this for initialization
	string[] name = {
		"1","2","3","平均时间","最长时间","最短时间"
	};
	
	public void HoverOver(GameObject obj){
		int number = int.Parse (obj.name);
		string detail = "";
		if (dic [number + 3].Length > 6)
			detail = dic [number + 3].Substring (0, 5);
		else
			detail = dic [number + 3];
		transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
	}
	
	public void HoverOut(GameObject obj){
		int number = int.Parse (obj.name);
		string detail = name [number + 2];
		transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
	}
	
	void Start () {
		FileOperation fo;
		if(choice == planType.left)
			fo = new FileOperation ("left_hand_result.txt");
		else
			fo = new FileOperation ("right_hand_result.txt");
		
		List<byte[]> ret = new List<byte[]> ();
		Info = "";
		fo.ReadContentBinary (out ret);
		for (int i = 0; i < ret.Count; i++) {
			Info += Encoding.Default.GetString(ret[i], 0, ret[i].Length);
		}
		
		Infos = Info.Split ('\n');
		
		for (int i = 6; i < Infos.Length - 1; i++) {
			Debug.Log(Infos[i]);
			dic.Add (i - 5, Infos [i]);
			if(i > 8)
				transform.Find ((i-8).ToString()).Find("Title").GetComponent<UILabel>().text = name[i-6];
			else
				transform.Find ("Piechart").GetComponent<CallDraw>().mData[i-6] = float.Parse (Infos [i]);
		}
		
		//byte[] buffer= Encoding.GetEncoding("GBK").GetBytes(Infos [Infos.Length - 1]); 
		//string Text = Encoding.UTF8.GetString(buffer);
		Debug.Log (Infos [Infos.Length - 1]);
		transform.Find ("Label").GetComponent<UILabel> ().text = Infos [Infos.Length - 1];
		
		string temp = "您的平均时间比历史";
		string[] times = Infos[9].Split(':');
		float time1 = float.Parse(times[0]);
		float time2 = float.Parse(times[1]);
		if(time1-time2<0)
			temp+="快了"+(time2-time1).ToString()+"秒";
		else
			temp+="慢了"+(time1-time2).ToString()+"秒";
		transform.Find ("Time").GetComponent<UILabel> ().text = temp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
