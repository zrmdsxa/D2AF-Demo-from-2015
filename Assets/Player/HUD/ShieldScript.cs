using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldScript : MonoBehaviour {

	int sp = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void set(int newsp){
		sp = newsp;
		if (sp < 0) {
			sp = 0;
		}
		UpdateHUD ();
	}

	void UpdateHUD(){
		transform.GetComponent<Text> ().text = sp.ToString ();
	}
}
