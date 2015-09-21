using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class Onclick : MonoBehaviour {
	void OnClick(){
		SceneManager.Global.RecordName = transform.Find ("Title").GetComponent<UILabel>().text;
		SceneManager.Global.RunMode = MCTTypes.RunModes.PlayFromFile;
		
		Application.LoadLevel (SceneManager.Global.SceneNum);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
