using UnityEngine;
using System.Collections;

public class ArmTrack : MonoBehaviour {
	// Use this for initialization
	public GameObject[] arms;
	public float x1, y1, z1, x2, y2, z2, x3, y3, z3;
	public Vector3[] joints;
	public Vector3 old_ab, old_ca;
	public float angle1;
	public float angle2;
	void Start () {
		
		initGameObject ();
	}
	void initGameObject(){
		old_ab = new Vector3 ();
		old_ca = new Vector3 ();
		joints = new Vector3[3];
		arms = new GameObject[3];
		arms [0] = transform.Find ("spine").transform.Find ("LeftShoulder").transform.Find ("LeftArm").gameObject;
		arms [1] = arms [0].transform.Find ("LeftForeArm").gameObject;
		arms [2] = arms [1].transform.Find ("LeftForeArmRoll").transform.Find ("LeftHand").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		x1 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().x1;
		y1 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().y1;
		z1 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().z1;
		x2 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().x2;
		y2 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().y2;
		z2 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().z2;
		x3 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().x3;
		y3 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().y3;
		z3 = (float)GameObject.Find ("PCM").GetComponent<PointCloudViewer> ().z3;
		
		joints [0] = new Vector3 (x1 * 100, y1 * 100, z1 * 100);
		joints [1] = new Vector3 (x3, y3, z3);
		joints [2] = new Vector3 (x2, y2, z2);


		Vector3 ca = joints [2] - joints [1];
		float ca_angle = Mathf.Acos(Vector3.Dot (ca, old_ca) / (ca.magnitude * old_ca.magnitude));
		Vector3 cross = Vector3.Cross (ca, old_ca); 
		Quaternion temp = new Quaternion (Mathf.Sin (ca_angle / 2) * cross.x,Mathf.Sin (ca_angle / 2) * cross.y,Mathf.Sin (ca_angle / 2) * cross.z, Mathf.Cos (ca_angle / 2));
		//Debug.LogWarning (string.Format ("CA:{0} OLD_CA:{1}", ca.ToString (), old_ca.ToString ()));

		//Debug.LogWarning (string.Format ("Angle:{0} Cross:{1} Quaternion:{2}", ca_angle.ToString (), cross.ToString (), temp.ToString ()));
		old_ca = ca;
		//arms [2].transform.localPosition = joints [0]/100;
		//arms [1].transform.localPosition = joints [1]/100;
		Vector3 rotEuler = temp.eulerAngles;
		if (rotEuler.x < 300f)
			rotEuler.z = Mathf.Clamp (-rotEuler.x, -80f, 0);
		else
			rotEuler.z = Mathf.Clamp (rotEuler.x - 360, -80f, 0);
		
		rotEuler.y = Mathf.Clamp (-rotEuler.y, -6f, 6f);
		rotEuler.x = 0;	

		//arms [0].transform.localRotation = Quaternion.Euler(rotEuler);//Quaternion.Euler(rotEuler);
		//Debug.LogWarning (rotEuler.ToString ());

		Vector3 ab = joints [1] - joints [0];
		//float angle = Mathf.Acos(Vector3.Dot (ca, ab) / (ca.magnitude * ab.magnitude));
		//float old_angle = Mathf.Acos(Vector3.Dot (old_ca, old_ab) / (old_ca.magnitude * old_ab.magnitude));


		float ab_angle = Mathf.Acos (Vector3.Dot (ab, old_ab) / (ab.magnitude * old_ab.magnitude));
		Vector3 ab_cross = Vector3.Cross (ab, old_ab); 
		Quaternion ab_temp = new Quaternion (Mathf.Sin (ab_angle / 2) * ab_cross.x,Mathf.Sin (ab_angle / 2) * ab_cross.y,Mathf.Sin (ab_angle / 2) * ab_cross.z, Mathf.Cos (ab_angle / 2));
		//Debug.LogWarning (string.Format ("ab_Angle:{0} ab_Cross:{1} ab_Quaternion:{2}", ab_angle.ToString (), ab_cross.ToString (), ab_temp.ToString ()));

		old_ab = ab;
		rotEuler = ab_temp.eulerAngles;
		float tmp = rotEuler.z;
		rotEuler.z = rotEuler.x;
		rotEuler.x = 0;
		/*if (rotEuler.x < 300f)
			rotEuler.z = Mathf.Clamp (-rotEuler.x, -80f, 0);
		else
			rotEuler.z = Mathf.Clamp (rotEuler.x - 360, -80f, 0);
		
		rotEuler.y = Mathf.Clamp (-rotEuler.y, -6f, 6f);
		rotEuler.x = 0;*/

		//arms [1].transform.localEulerAngles = rotEuler;
		arms[1].transform.Rotate(rotEuler);
		Debug.LogWarning (arms [1].transform.rotation.ToString ());
		

		/*float cos = Vector2.Dot (ab, ac)/(ab.magnitude * ac.magnitude);
		angle1 = Mathf.Acos (cos);
		angle2 = angle1 - angle2;

		Debug.LogWarning (angle2.ToString ());
		angle2 = angle1;*/
		//arms [0].transform.localPosition = new Vector3 (x1, y1, z1);
		//Debug.LogWarning (string.Format ("{0} {1} {2}", x1, y1, z1));
	}
}
