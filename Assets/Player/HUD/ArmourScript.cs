using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArmourScript : MonoBehaviour {

	int ap = -1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void set(int newap){
		ap = newap;
		if (ap < 0) {
			ap = 0;
		}
		UpdateHUD ();
	}
	
	void UpdateHUD(){
		transform.GetComponent<Text> ().text = ap.ToString ();

	}
}
