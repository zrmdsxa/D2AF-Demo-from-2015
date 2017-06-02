using UnityEngine;
using System.Collections;

public class EnemySatelliteScript : MonoBehaviour {
	public int ad=200;
	public int sd=1000;
	bool moveRight = false;
	public float speed = 0.01f;
	public Transform laser;
	public float cooldown = 1f;
	float currentcooldown =1;

	Transform player;
	Transform pivot;
	
	// Use this for initialization
	void Start () {
		pivot = transform.FindChild("LaserPivot");
		player = GameObject.Find ("D2AssaultFighter").transform;
	}
	
	// Update is called once per frame
	void Update () {

		if (currentcooldown > 0) {
			currentcooldown -= Time.deltaTime;
		}
		if (moveRight) {
			transform.position += transform.right * speed;
			if (transform.position.x > 4.3f){
				Destroy (this.gameObject);
			}
		} else {
			transform.position -= transform.right * speed;
			if (transform.position.x < -4.3f){
				Destroy (this.gameObject);
			}
		}



		AimAtPlayer ();
	}

	void AimAtPlayer(){
		float angle = Mathf.Atan2 (player.position.y - pivot.position.y, player.position.x - pivot.position.x) * 180 / Mathf.PI;
		if (angle > -150f && angle < -30f) {
			Quaternion current = pivot.rotation;
			pivot.LookAt(player.transform);
			pivot.rotation = Quaternion.Lerp(current,pivot.rotation,0.1f);
			if (currentcooldown <= 0.0f){
				FireAtPlayer ();
			}
		}
	}

	void FireAtPlayer(){
		RaycastHit hit;
		if (Physics.Raycast (pivot.position + pivot.forward * 0.28f, pivot.forward,out hit, 10f)) {
			//Debug.DrawLine (pivot.position + pivot.forward * 0.28f, hit.point);
			currentcooldown = cooldown;
			Transform t = Instantiate (laser);
			t.position = pivot.position + pivot.forward*0.78f;
			t.rotation = pivot.rotation;
			//t.GetComponentInChildren<EnemyBulletScript>().setDamage(ad,sd);

		}
	}

	public void MoveRight(){
		moveRight = true;
	}
}
