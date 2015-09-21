using UnityEngine;
using System.Collections;
using Client.FileOp;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class setBodyResult : MonoBehaviour {
	Dictionary<int,string> dic = new Dictionary<int,string>();
	string Info = "";
	string[] Infos;
	string[] name = {
		"1","2","3","个数","总用时","肘部弯曲","肩部伸直","动作总得分"
	};
	
	public void HoverOver(GameObject obj){
		int number = int.Parse (obj.name);
		string detail = "";
		if (dic [number + 3].Length > 6)
			detail = dic [number + 3].Substring (0, 5);
		else
			detail = dic [number + 3];
		Debug.LogWarning(detail);
		transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
		if (number >= 3 && number <= 5) {
			Debug.LogWarning(number.ToString());
			string temp = "您的"+name[number+2]+"比历史";
			string[] times = Infos[number+2].Split(':');
			float time1 = float.Parse(times[0]);
			float time2 = float.Parse(times[1]);
			if(time1-time2<0)
				temp+="减少了"+(time2-time1).ToString()+"分";
			else
				temp+="增加了"+(time1-time2).ToString()+"分";
			transform.Find ("Time").GetComponent<UILabel> ().text = temp;
		}
	}
	
	public void HoverOut(GameObject obj){
		int number = int.Parse (obj.name);
		string detail = name [number + 2];
		transform.Find (number.ToString ()).Find ("Title").GetComponent<UILabel> ().text = detail;
		transform.Find ("Time").GetComponent<UILabel> ().text = "";
	}
	
	// Use this for initialization
	void Start () {
		FileOperation fo = new FileOperation ("pushup_result.txt");
		List<byte[]> ret = new List<byte[]> ();
		Info = "";
		fo.ReadContentBinary (out ret);
		for (int i = 0; i < ret.Count; i++) {
			Info += Encoding.UTF8.GetString(ret[i], 0, ret[i].Length);
		}
		Infos = Info.Split ('\n');
		Debug.LogWarning (Info);
		
		for (int i = 0; i < Infos.Length - 1; i++) {
			Debug.LogWarning(Infos[i]);
			dic.Add (i + 1, Infos [i]);
			if(i > 2)
				transform.Find ((i-2).ToString()).Find("Title").GetComponent<UILabel>().text = name[i];
			else
				transform.Find ("Piechart").GetComponent<CallDraw>().mData[i] = 1;
		}
		Debug.LogWarning (Infos [Infos.Length - 1]);
		transform.Find ("Label").GetComponent<UILabel> ().text = Infos[Infos.Length - 1];
		//Debug.LogWarning (buf);
		/*FileStream fs;
		fs = new FileStream ("shoulder_result.txt", FileMode.Open);

		StreamReader sr = new StreamReader (fs);
		string Info = sr.ReadToEnd();
		Debug.LogWarning (Info);*/
	}
	// Update is called once per frame
	void Update () {
		
	}
}
