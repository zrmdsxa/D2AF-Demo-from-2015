using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	int score=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addScore(int s){
		score += s;
		UpdateHUD ();
	}

	void UpdateHUD(){
		transform.GetComponent<Text> ().text = score.ToString ();
	}
}
