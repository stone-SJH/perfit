using UnityEngine;
using System.Collections;
using System.IO;

public class PlanManager : MonoBehaviour {
	public enum planType {hand, shoudler, body}
	public planType choice = 0;
	public string[] plans;
	string Info;
	
	public void ClickStartButton(){
		if (choice == planType.hand) {
			bool flag = transform.parent.Find ("HandPlan").GetComponent<setUsedOrNot> ().isUsed;
			transform.parent.Find ("HandPlan").GetComponent<setUsedOrNot> ().isUsed = !flag;
			PlayerPrefs.SetString ("plan1Using", (!flag).ToString());
		} else if (choice == planType.shoudler) {
			bool flag = transform.parent.Find ("ArmPlan").GetComponent<setUsedOrNot> ().isUsed;
			transform.parent.Find ("ArmPlan").GetComponent<setUsedOrNot> ().isUsed = !flag;
			PlayerPrefs.SetString ("plan2Using", (!flag).ToString());
		} else {
			bool flag = transform.parent.Find ("BodyPlan").GetComponent<setUsedOrNot> ().isUsed;
			transform.parent.Find ("BodyPlan").GetComponent<setUsedOrNot> ().isUsed = !flag;
			PlayerPrefs.SetString ("plan3Using", (!flag).ToString());
		}
		
	}
	
	public void ClickNumberButton(GameObject obj){
		int number = int.Parse (obj.name);
		for (int i = 1; i < 8; i++) {
			transform.parent.Find ("Scroll View").Find(i.ToString()).GetComponent<setStatus> ().isSelected = false;
		}
		transform.parent.Find ("Scroll View").Find (number.ToString ()).GetComponent<setStatus> ().isSelected = true;
		if (choice == planType.hand) {
			PlayerPrefs.SetString("plan1", plans[number-1]);
			PlayerPrefs.SetString ("handplan", number.ToString ());
		} else if (choice == planType.shoudler) {
			PlayerPrefs.SetString("plan2", plans[number-1]);
			PlayerPrefs.SetString ("armplan", number.ToString ());
		} else {
			PlayerPrefs.SetString("plan3", plans[number-1]);
			PlayerPrefs.SetString ("bodyplan", number.ToString ());
		}
		
	}
	
	void InitDetail(string[] details){
		//Debug.Log (details [0]);
		Transform label1 = transform.parent.Find ("Scroll View").Find (details[0]).Find ("Label-light");
		Transform label2 = transform.parent.Find ("Scroll View").Find (details[0]).Find ("Label-dark");
		label1.GetComponent<UILabel> ().text = "第" + details [0] + "天:" + details [1];
		label2.GetComponent<UILabel> ().text = "第" + details [0] + "天:" + details [1];
		plans [int.Parse (details [0]) - 1] = details [1];
		//Debug.Log (plans [int.Parse (details [0]) - 1]);
		
		if (string.Compare("y", details[2]) == 0)
			transform.parent.Find ("Scroll View").Find (details [0]).GetComponent<setStatus> ().isDone = true;
		else
			transform.parent.Find ("Scroll View").Find (details [0]).GetComponent<setStatus> ().isDone = false;
	}
	
	public void nextplan1(){
		if (int.Parse(PlayerPrefs.GetString ("handplan")) >= plans.Length)
			return;
		if (choice == planType.hand) {
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString ("handplan")).GetComponent<setStatus> ().isDone = true;
			changeFileContent(int.Parse(PlayerPrefs.GetString ("handplan")),"handplan.txt");
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString ("handplan")).GetComponent<setStatus> ().isSelected = false;
			transform.parent.Find ("Scroll View").Find ((int.Parse (PlayerPrefs.GetString ("handplan")) + 1).ToString ()).GetComponent<setStatus> ().isSelected = true;
			PlayerPrefs.SetString("plan1", plans[int.Parse (PlayerPrefs.GetString ("handplan"))]);
			PlayerPrefs.SetString ("handplan", (int.Parse(PlayerPrefs.GetString ("handplan")) + 1).ToString());
		}
	}
	
	public void nextplan2(){
		if (int.Parse(PlayerPrefs.GetString ("armplan")) >= plans.Length)
			return;
		if (choice == planType.shoudler) {
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString ("armplan")).GetComponent<setStatus> ().isDone = true;
			changeFileContent(int.Parse(PlayerPrefs.GetString ("armplan")),"armplan.txt");
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString ("armplan")).GetComponent<setStatus> ().isSelected = false;
			transform.parent.Find ("Scroll View").Find ((int.Parse (PlayerPrefs.GetString ("armplan")) + 1).ToString ()).GetComponent<setStatus> ().isSelected = true;
			PlayerPrefs.SetString("plan2", plans[int.Parse (PlayerPrefs.GetString ("armplan"))]);
			PlayerPrefs.SetString ("armplan", (int.Parse(PlayerPrefs.GetString ("armplan")) + 1).ToString());
		}
	}
	
	public void nextplan3(){
		if (int.Parse(PlayerPrefs.GetString ("bodyplan")) >= plans.Length)
			return;
		if (choice == planType.body) {
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString ("bodyplan")).GetComponent<setStatus> ().isDone = true;
			changeFileContent(int.Parse(PlayerPrefs.GetString ("bodyplan")),"bodyplan.txt");
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString ("bodyplan")).GetComponent<setStatus> ().isSelected = false;
			transform.parent.Find ("Scroll View").Find ((int.Parse (PlayerPrefs.GetString ("bodyplan")) + 1).ToString ()).GetComponent<setStatus> ().isSelected = true;
			PlayerPrefs.SetString("plan3", plans[int.Parse (PlayerPrefs.GetString ("bodyplan"))]);
			PlayerPrefs.SetString ("bodyplan", (int.Parse(PlayerPrefs.GetString ("bodyplan")) + 1).ToString());
		}
	}
	
	void changeFileContent(int num, string filename){
		Debug.Log ("here!!!");
		string[] Infos = Info.Split ('\n');
		string txt = "";
		for (int i = 0; i < Infos.Length; i++) {
			if(i == num-1){
				string temp = Infos[i];
				Debug.Log("[*]"+temp);
				Infos[i] = Infos[i].Substring(0,Infos[i].Length-1)+"y";
				Debug.Log("[**]"+Infos[i]);
				Info = Info.Replace(temp,Infos[i]);//!!!!
				FileStream fs = new FileStream (filename, FileMode.Create);//!!!!
				StreamWriter sw = new StreamWriter (fs);
				//StreamReader sr = new StreamReader (fs);
				//Debug.Log (sr.ReadLine());
				sw.AutoFlush = true;
				sw.WriteLine (Info);
				
				sw.Close ();
				fs.Close();
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		if (choice == planType.hand) {
			FileStream fs = new FileStream ("handplan.txt", FileMode.Open);
			StreamReader sr = new StreamReader (fs);
			Info = sr.ReadToEnd();
		} else if (choice == planType.shoudler) {
			FileStream fs = new FileStream ("armplan.txt", FileMode.Open);
			StreamReader sr = new StreamReader (fs);
			Info = sr.ReadToEnd();
		} else {
			FileStream fs = new FileStream ("bodyplan.txt", FileMode.Open);
			StreamReader sr = new StreamReader (fs);
			Info = sr.ReadToEnd();
		}
		//Debug.Log (Info);
		
		
		string[] Infos = Info.Split ('\n');
		plans = new string[Infos.Length];
		for (int i = 0; i < 7; i++) {
			string[] details = Infos[i].Split(':');
			Debug.Log(details[0]);
			InitDetail(details);
			
		}
		if (choice == planType.hand) {
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString("handplan")).GetComponent<setStatus> ().isSelected = true;
		} else if (choice == planType.shoudler) {
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString("armplan")).GetComponent<setStatus> ().isSelected = true;
		} else {
			transform.parent.Find ("Scroll View").Find (PlayerPrefs.GetString("bodyplan")).GetComponent<setStatus> ().isSelected = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (choice == planType.hand) {
			if (SceneManager.Global.HandTag) {
				SceneManager.Global.HandTag = false;
				nextplan1 ();
			}
		} else if (choice == planType.shoudler) {
			if (SceneManager.Global.ArmTag) {
				SceneManager.Global.ArmTag = false;
				nextplan2 ();
			}
		} else {
			if (SceneManager.Global.BodyTag) {
				SceneManager.Global.BodyTag = false;
				nextplan3 ();
			}
		}
	}
}
