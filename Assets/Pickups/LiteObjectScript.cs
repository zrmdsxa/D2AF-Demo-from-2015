using UnityEngine;
using System.Collections;

//This is facing up

public class LiteObjectScript : MonoBehaviour {

	bool alive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Lite falls off the screen - destroy
		if (transform.position.y < -6) {
			Destroy (this.gameObject);
		}

		//move down
		transform.position += transform.up*-0.02f;

	}

	public bool getStatus(){
		return alive;
	}

	public void pickUp(){
		alive = false;
	}
}
