using UnityEngine;
using System.Collections;
using Client.CS;

public class PanelButtonManager : MonoBehaviour {
	public void submit1(){
		string username = PlayerPrefs.GetString ("User");
		string npassword = transform.Find ("password").Find("password").gameObject.GetComponent<UIInput>().value;
		string ret = "";

		//waiting to be deleted
		Connection.getInstance ().Login (username, PlayerPrefs.GetString ("Password"), out ret);
		if (npassword.Length >= 3)
			Connection.getInstance ().ModifyPass (username, npassword, out ret);
		else 
			ret = "failure";

		if (ret != "failure") {
			PlayerPrefs.SetString("Password", npassword);
		}
		Debug.Log (ret);
	}

	public void submit2(){
		string username = PlayerPrefs.GetString ("User");
		string weight = transform.Find ("weight").Find ("weight").gameObject.GetComponent<UIInput>().value;
		string height = transform.Find ("height").Find ("height").gameObject.GetComponent<UIInput>().value;
		string ret = "";
		Debug.Log (PlayerPrefs.GetString ("Password"));
		Connection.getInstance ().Login (username, PlayerPrefs.GetString ("Password"), out ret);
		Connection.getInstance ().ModifyInfo (username, double.Parse(height), double.Parse(weight), out ret);

		if (ret != "failure") {
			PlayerPrefs.SetString("weight", weight);
			PlayerPrefs.SetString("height", height);
		}
		Debug.Log (ret);
	}

	public void LoginButtonPressed(){	
		transform.Find ("Warning").gameObject.SetActive(false);
		//Debug.Log(transform.Find ("UserName").gameObject.GetComponent<UIInput>().value);
		//Debug.Log(transform.Find ("PassWord").gameObject.GetComponent<UIInput>().value);
		string username = transform.Find ("UserName").gameObject.GetComponent<UIInput> ().value;
		string password = transform.Find ("PassWord").gameObject.GetComponent<UIInput>().value;
		string ret = "";
		if (username.Length >= 3 && password.Length >= 3)
			Connection.getInstance ().Login (username, password, out ret);
		else
			ret = "failure";

		Debug.Log ("ret:" + ret);
		if (ret == "failure") {
			transform.Find ("UserName").gameObject.GetComponent<UIInput> ().value = "";
			transform.Find ("PassWord").gameObject.GetComponent<UIInput> ().value = "";
			transform.Find ("Warning").gameObject.SetActive (true);
		} else if (ret == "success"){
			PlayerPrefs.SetString("User", username);
			PlayerPrefs.SetString("Password", password);
			Debug.Log ("GoTo MainMenu...");
			Connection.getInstance().RecvFile("armplan.txt", out ret);
			Connection.getInstance().RecvFile("bodyplan.txt", out ret);
			Connection.getInstance().RecvFile("handplan.txt", out ret);
			Connection.getInstance().RecvFile("shoulder_history.txt", out ret);
			Connection.getInstance().RecvFile("righthand_history.txt", out ret);
			Connection.getInstance().RecvFile("lefthand_history.txt", out ret);
			Connection.getInstance().RecvFile("pushup_history.txt", out ret);
			Application.LoadLevel(1);
		}
	}

	public void RegisterButtonPressed(){
		transform.Find ("Warning").gameObject.SetActive(false);
		//Debug.Log(transform.Find ("UserName").gameObject.GetComponent<UIInput>().value);
		//Debug.Log(transform.Find ("PassWord").gameObject.GetComponent<UIInput>().value);
		string username = transform.Find ("UserName").gameObject.GetComponent<UIInput> ().value;
		string password = transform.Find ("PassWord").gameObject.GetComponent<UIInput>().value;
		string ret = "";
		if (username.Length >= 3 && password.Length >= 3)
			Connection.getInstance().Register (username, password, out ret);
		else
			ret = "failure";
		Debug.Log ("ret:" + ret);
		if (ret == "failure") {
			transform.Find ("UserName").gameObject.GetComponent<UIInput> ().value = "";
			transform.Find ("PassWord").gameObject.GetComponent<UIInput> ().value = "";
			transform.Find ("Warning").gameObject.SetActive (true);
		} else if (ret == "success"){
			PlayerPrefs.SetString("User", username);
			PlayerPrefs.SetString("Password", password);
			Debug.Log ("GoTo MainMenu...");
			Application.LoadLevel(1);
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
