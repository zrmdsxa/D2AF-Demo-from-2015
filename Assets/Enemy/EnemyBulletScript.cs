using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {


	public int ad= 600;
	public int sd = 500;
	public float speed=0.2f;
	public Transform explosion;
	public bool active = false;
	bool alive = true;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position += transform.forward * speed;

	}

	public void setActive(){
		active = true;
	}

	public void setDamage(int newad, int newsd){
		ad = newad;
		sd = newsd;
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("EnemyMissile hit");
		if (collision.transform.tag == "Player") {
			collision.transform.GetComponent<PlayerScript> ().Hit (ad, sd);
			explode ();
		}
	}

	void explode(){
		alive = false;
		Transform t = Instantiate (explosion);
		t.position = transform.position;
		Destroy (this.gameObject);
	}
}
