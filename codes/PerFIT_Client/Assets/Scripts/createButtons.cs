using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class createButtons : MonoBehaviour {
	public enum playType {hand, body, arm};
	public playType choice = 0;

	public void createButton(){

	}

	
	void Start () {
		List<string> lst = new List<string> ();
		string ret = "";
		Connection.getInstance ().Login (PlayerPrefs.GetString ("User"), PlayerPrefs.GetString ("Password"),out ret);
		if (choice == playType.hand) {
			Connection.getInstance ().CheckMovie ("hand", out lst);
		} else if (choice == playType.arm) {
			Connection.getInstance ().CheckMovie ("arm", out lst);
		} else {
			Connection.getInstance ().CheckMovie ("body", out lst);
		}
		Debug.Log (lst.ToString ());
		for (int i = 0; i < lst.Count; i++) {
			GameObject sample = Resources.Load("Prefabs/IconButton-2") as GameObject;
			GameObject button = Instantiate(sample);

			button.transform.parent = this.transform;
			button.transform.localScale = new Vector3(1,1,1);
			if (choice == playType.hand) {

				button.GetComponent<getFileFromServer>().choice = (getFileFromServer.playType)0;
			} else if (choice == playType.arm) {
				button.GetComponent<getFileFromServer>().choice = (getFileFromServer.playType)1;
			} else {
				button.GetComponent<getFileFromServer>().choice = (getFileFromServer.playType)2;
			}
			button.transform.Find ("Title").GetComponent<UILabel>().text = lst[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
