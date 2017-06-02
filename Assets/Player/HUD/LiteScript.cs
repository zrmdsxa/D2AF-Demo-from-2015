using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LiteScript : MonoBehaviour {

	int lite=0;
	bool alive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void set(int newlite){
		lite = newlite;
		UpdateHUD ();
	}

	void UpdateHUD(){
		transform.GetComponent<Text> ().text = lite.ToString ();
	}


}
