using UnityEngine;
using System.Collections;

public class Level1Script : MonoBehaviour {

	enum Phase{start,phase1,phase2,phase3,miniboss,boss}
	Phase phase;

	public Transform enemymissileplane;
	public Transform satellite;
	public Transform frigate;

	Color c;
	Renderer r;
	TextMesh t;

	float fadeSpeed = 0.01f;
	bool fadeIn = false;
	bool fadeOut = false;

	// Use this for initialization
	void Start () {
		r = transform.GetComponent<Renderer> ();	//Get the renderer component
		c = r.material.color;						//Get a color thing to use
		c.a = 0;									//set alpha to 0
		r.material.color = c;						//apply changes to color
		t = transform.GetComponent<TextMesh> ();
		t.text = "Stage 1\nDemo";

		phase = Phase.start;
		StartCoroutine (Level ());

	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn) {
			c.a+=fadeSpeed;
			r.material.color = c;
			if (c.a >= 1.0f){
				fadeIn=false;
				fadeOut=true;
			}
		} else if (fadeOut) {
			c.a-=fadeSpeed;
			r.material.color = c;
			if (c.a <= 0.0f){
				fadeOut=false;
			}
		}

	}

	//max sides 3.25f
	//max height 4.6f

	//~8 seconds if spawned from 10 for something moving at 0.01f
	//~1 second at 6 for something moving at 0.01f

	IEnumerator Level(){
		if (phase == Phase.start) {
			yield return new WaitForSeconds (1);
			fadeIn = true;

			phase = Phase.phase1;
		} 
		if (phase == Phase.phase1) {
			yield return new WaitForSeconds (5);
			
			Transform t1 = Instantiate (satellite);
			t1.position = new Vector2 (4.5f, 4);

			yield return new WaitForSeconds (1.6f);

			t1 = Instantiate (satellite);
			t1.position = new Vector2 (-4.5f, 3);
			t1.GetComponentInChildren<EnemySatelliteScript>().MoveRight();
			
			yield return new WaitForSeconds (13);
			
			t1 = Instantiate (enemymissileplane);
			t1.position = new Vector2 (0, 6);
			
			yield return new WaitForSeconds (1);
			
			t1 = Instantiate (enemymissileplane);
			t1.position = new Vector2 (2, 6);
			t1 = Instantiate (enemymissileplane);
			t1.position = new Vector2 (-2, 6);

			yield return new WaitForSeconds (6);

			t.text = "You have\nfinished\nthe demo";
			fadeIn = true;
		}
	}

}
