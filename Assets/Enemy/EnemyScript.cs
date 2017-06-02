using UnityEngine;
using System.Collections;


//This is facing forward

public class EnemyScript : MonoBehaviour {

	bool alive = true;
	public int ap = 15000; 
	public int sp = 0; 
	public int score = 100;

	public Transform lite;
	public Transform explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	void dead()
	{
		alive = false;
		GameObject.Find ("SCORE").GetComponent<ScoreScript> ().addScore (score);
		Transform t = Instantiate (explosion);
		t.position = transform.position;
		t = Instantiate (lite);
		t.position = transform.position;
		Destroy (this.gameObject);
	}

	public void Hit(int ad, int sd){
		
		if (alive) {
			//#step 1: subtract shield from sd
			if (sp > 0 && sd > 0) {
				int tsp = sp; //temp sp
				sp -= sd;    //shield take hit from sd
				sd -= tsp;   //subtract the sd
			}
			
			//#step 2:  subtract shield from ad
			
			if (sp > 0 && ad > 0) {
				int tsp = sp;   //temp sp
				ad /= 10;    //shield takes 10% from ad
				sp -= ad;     //shield take hit from ad
				ad -= tsp;   //subtract the ad
				ad *= 10;    //turn ad back to normal
			}
			
			//#step 3: subtract armor from sd
			
			if (ap > 0 && sd > 0) {
				sd /= 20;    //armor takes 5% from sd
				ap -= sd;     //armor takes hit from sd
			}
			
			//#step 4: subtract armor from ad
			if (ap > 0 && ad > 0) {
				ap -= ad;
			}
			Debug.Log ("enemy hp = " + ap + " " + sp);
			if (ap <= 0) {
				dead ();
			}
			
		}
	}

	public bool getStatus(){
		return alive;
	}

	public int getAp(){
		return ap;
	}
	public int getSp(){
		return sp;
	}
}
