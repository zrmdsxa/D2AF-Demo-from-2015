using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//To be used with D2 Assault Fighter

//This is facing forward

public class PlayerScript : MonoBehaviour
{

	bool alive = true;
	float topbound = 4.6f;
	float bottombound = -4.4f;
	float leftbound = -3.25f;
	float rightbound = 3.25f;

	//ap and sp of ship
	
	public int startap = 23000;
	public int startsp = 13000;
	int ap;
	int sp;
	int lite=0;
	public float speed = 0.06f;
	public float mspregen = 0.02f;
	float spregen = 0;

	//The hud 
	public Transform hud;

	Transform apCounter;
	Transform spCounter;
	Transform liteCounter;
	Transform scoreCounter;
	Transform apBar;
	Transform spBar;
	ArmourScript armourScript;
	ShieldScript shieldScript;
	LiteScript liteScript;
	ScoreScript scoreScript;


	//the bullet D2AF fires
	public Transform bullet;
	public float maincooldown = 0.1f;
	float maincurrentcooldown = 0;		//change this value

	Transform mainDamage;
	public int mainadbase = 900;
	public int mainsdbase = 500;
	int mainad;
	int mainsd;

	public Transform explosion;

	NetworkView nv;

	// Use this for initialization
	void Start ()
	{
		nv = transform.GetComponent<NetworkView> ();


		//set up hud

		if (nv.isMine || !(Network.isClient || Network.isServer)) {
			Transform h = Instantiate (hud);
			apCounter = h.FindChild("ARMOUR");
			spCounter = h.FindChild("SHIELD");
			liteCounter = h.FindChild("LITE");
			scoreCounter = h.FindChild("SCORE");
			mainDamage = h.FindChild("MAINDAMAGE");
			apBar = h.FindChild("ArmourBar");
			spBar = h.FindChild("ShieldBar");
			armourScript = apCounter.GetComponent<ArmourScript> ();
			shieldScript = spCounter.GetComponent<ShieldScript> ();
			liteScript = liteCounter.GetComponent<LiteScript> ();
			scoreScript = scoreCounter.GetComponent<ScoreScript> ();

			mainad = mainadbase;
			mainsd = mainsdbase;

			ap = startap;
			sp = startsp;
			armourScript.set (ap);
			shieldScript.set (sp);

			transform.GetComponent<NetworkInterpolateScript> ().enabled = false;
		}
		else {
			transform.GetComponent<NetworkInterpolateScript> ().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawRay (transform.position, transform.forward);
		Debug.DrawRay (transform.position, -transform.forward);
		Debug.DrawRay (transform.position, transform.right);
		Debug.DrawRay (transform.position, -transform.right);

		if (transform.GetComponent<NetworkView>().isMine || !(Network.isClient || Network.isServer)) {
			//fire main weapon button
			if (Input.GetKey (KeyCode.Z)) {
				if (maincurrentcooldown <= 0) {
					Transform a = Instantiate (bullet);
					a.position = transform.position + transform.forward * 0.58f + transform.right * 0.022f;
					a.GetComponent<BulletScript>().setDamage(mainad,mainsd);
					Transform b = Instantiate (bullet);
					b.position = transform.position + transform.forward * 0.58f + transform.right * -0.022f;
					b.GetComponent<BulletScript>().setDamage(mainad,mainsd);
					maincurrentcooldown = maincooldown;
				}
			}
			/*
			if (Input.GetKey (KeyCode.X)) {

			}*/


						//Movement
			if (Input.GetKey (KeyCode.LeftShift)) {
				if (Input.GetKey (KeyCode.RightArrow)) {
					transform.position += transform.right * speed / 2;
				}
				if (Input.GetKey (KeyCode.LeftArrow)) {
					transform.position += -transform.right * speed / 2;
				}
				if (Input.GetKey (KeyCode.UpArrow)) {
					transform.position += transform.forward * speed / 2;
				}
				if (Input.GetKey (KeyCode.DownArrow)) {
					transform.position += -transform.forward * speed / 2;
				}
			} else {
				if (Input.GetKey (KeyCode.RightArrow)) {
					transform.position += transform.right * speed;
				}
				if (Input.GetKey (KeyCode.LeftArrow)) {
					transform.position += -transform.right * speed;
				}
				if (Input.GetKey (KeyCode.UpArrow)) {
					transform.position += transform.forward * speed;
				}
				if (Input.GetKey (KeyCode.DownArrow)) {
					transform.position += -transform.forward * speed;
				}
			}
			//cooldowns

			//main weapon cooldown
			if (maincurrentcooldown > 0) {
				maincurrentcooldown -= Time.deltaTime;
			}

			//shield regen cooldown
			if (spregen <= 0 && sp < startsp) {
				sp += 1;
				setSP ();
				spregen = mspregen;
			} else {
				if (spregen > 0) {
					spregen -= Time.deltaTime;
				}
			}

			//Boundaries

			transform.position = new Vector3(Mathf.Clamp (transform.position.x,leftbound,rightbound),Mathf.Clamp (transform.position.y,bottombound,topbound),0);
			                                 /*
			if (transform.position.x > rightbound) {
				transform.position = new Vector3 (rightbound, transform.position.y, transform.position.z);
			}
			if (transform.position.x < leftbound) {
				transform.position = new Vector3 (leftbound, transform.position.y, transform.position.z);
			}
			if (transform.position.y < bottombound) {
				transform.position = new Vector3 (transform.position.x, bottombound, transform.position.z);
			}
			if (transform.position.y > topbound) {
				transform.position = new Vector3 (transform.position.x, topbound, transform.position.z);
			}*/
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log ("Player hit");

		if (collision.transform.tag == "PickupLite") {
			if (collision.transform.GetComponent<LiteObjectScript> ().getStatus ()) {
				collision.transform.GetComponent<LiteObjectScript> ().pickUp ();
				scoreScript.addScore (1000);
				addLite (1);
				Destroy (collision.gameObject);
			}
		}
		if (collision.transform.tag == "Enemy") {
			EnemyScript es = collision.transform.GetComponent<EnemyScript> ();
			if (es.getStatus ()) {
				Hit (es.getAp ()/2, es.getSp ());
				es.Hit (startap, startsp);
			}
		}
	}
	
	public void Hit (int ad, int sd)
	{
		
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

			if (sp < 0){
				sp=0;
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


			if (ap <= 0) {
				dead ();
			}
			setAP ();
			setSP ();
		}
	}

	void setAP ()
	{
		armourScript.set (ap);
		apBar.GetComponent<Image> ().fillAmount = ((float) ap/(float)startap);
	}

	void setSP ()
	{
		shieldScript.set(sp);
		spBar.GetComponent<Image> ().fillAmount = ((float) sp/(float)startsp);
	}
	void addLite(int l){
		lite += l;
		mainad = (int)(mainadbase+(lite*1.5f));
		mainsd = (int)(mainsdbase + (lite * 1));
		mainDamage.GetComponent<Text> ().text = "("+mainad+"/"+mainsd+")";
		setLite ();
	}
	void setLite()
	{
		liteScript.set (lite);
	}

	void dead ()
	{
		Transform t = Instantiate (explosion);
		t.position = transform.position;
		Destroy (this.gameObject);
	}

}
