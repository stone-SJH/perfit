using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class SceneManager : MonoBehaviour {
	private int ArmScene = 3;
	private int HandScene = 2;
	private int BodyScene = 4;
	public class Global
	{
		public static  MCTTypes.RunModes RunMode = MCTTypes.RunModes.RecordToFile;
		public static int SceneNum = 0;
		public static string Handplan = PlayerPrefs.GetString("plan1");
		public static string Armplan = PlayerPrefs.GetString("plan2");
		public static string Bodyplan = PlayerPrefs.GetString("plan3");
		public static bool HandTag = false;
		public static bool ArmTag = false;
		public static bool BodyTag = false;
		public static string RecordName = "";
	}

	public void GoBack()
	{
		Application.LoadLevel (1);
	}

	public void BeginArmRecording()
	{
		Global.RunMode = MCTTypes.RunModes.RecordToFile;
		Application.LoadLevel (ArmScene);
		Global.SceneNum = ArmScene;
	}

	public void BeginHandRecording()
	{
		Global.RunMode = MCTTypes.RunModes.RecordToFile;
		Application.LoadLevel (HandScene);
		Global.SceneNum = HandScene;
	}

	public void BeginAction(int SceneNum)
	{
		Global.RunMode = MCTTypes.RunModes.LiveStream;
//		GameObject ReverseButton2 = (GameObject)Instantiate (ReverseButton, Vector3.zero, Quaternion.identity);
//		DontDestroyOnLoad(ReverseButton);
		Application.LoadLevel (SceneNum);
	}

	public void BeginReplay()
	{
		Global.RunMode = MCTTypes.RunModes.PlayFromFile;

		Application.LoadLevel (Global.SceneNum);
		Debug.Log("replay!");
	}

	public void ChangetoHand()
	{
		Global.SceneNum = HandScene;
		Debug.LogWarning (Global.SceneNum.ToString ());
	}

	public void ChangetoArm()
	{
		Global.SceneNum = ArmScene;
	}

	public void ChangetoBody()
	{
		Global.SceneNum = BodyScene;
	}

	public void BeginRecording()
	{
		Global.RunMode = MCTTypes.RunModes.RecordToFile;
		Debug.LogWarning (Global.SceneNum.ToString ());
		Application.LoadLevel (Global.SceneNum);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
