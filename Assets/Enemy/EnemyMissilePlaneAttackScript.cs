using UnityEngine;
using System.Collections;

public class EnemyMissilePlaneAttackScript : MonoBehaviour {
	
	//gun
	public int ad=1;
	public int sd=0;
	//missile
	public Transform missile;
	public int ad2=1;
	public int sd2=0;

	bool fired = false;
	public float speed = 0.01f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!fired) {
			if (transform.position.y < 5.0f) {
				//attack
				StartCoroutine(Attack ());
				fired = true;
			}
		}

		//move down
		transform.position += transform.forward*speed;

	}

	IEnumerator Attack(){
		Transform t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *-0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *-0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *-0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *0.2f;
		yield return new WaitForSeconds(0.6f);
		t = Instantiate (missile);
		t.transform.position = transform.position + transform.right *-0.2f;

	}


}
