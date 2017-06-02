using UnityEngine;
using System.Collections;

//This is facing up

public class BulletScript : MonoBehaviour {
	public float life = 2.00f;
	public int ad = 900;
	public int sd = 500;
	public Transform sparks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y >= 5.5f) {
			Destroy (this.gameObject);
		}
		if (life < 0) {
			Destroy (this.gameObject);
		}
		if (life > 0) {
			life-= Time.deltaTime;
			transform.position += transform.up*0.5f;
		}



	}

	public void setDamage(int newad, int newsd){
		ad = newad;
		sd = newsd;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "PickupLite") {
		} 
		else if (collision.transform.tag == "Enemy") {

			Debug.Log ("Bullet hit Enemy");
			collision.transform.GetComponent<EnemyScript>().Hit(ad,sd);
			DestroyBullet();
		} 
		/*else {
			DestroyBullet();
		}*/
	}

	void DestroyBullet(){
		Transform t = Instantiate (sparks);
		t.position = transform.position;
		Destroy (this.gameObject);
	}
}
