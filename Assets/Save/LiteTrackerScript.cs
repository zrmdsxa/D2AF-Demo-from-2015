using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LiteTrackerScript : MonoBehaviour {

	int lite;
	public static LiteTrackerScript lts;
	
	void Awake () {
		if (lts == null) {
			DontDestroyOnLoad (this);
			lts = this;
		} else if (lts != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		Load ();
		Save ();
		Debug.Log (lite);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/Game.data");
		Debug.Log (Application.persistentDataPath);
		GameData data = new GameData ();
		data.lite = lite;
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/Game.data")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/Game.data", FileMode.Open);

			//Deserialize does not know what data it is. Add a cast
			GameData data = (GameData)bf.Deserialize (file);
			file.Close ();

			lite = data.lite;
		} else {
			lite=0;
		}
	}
}

//saveable data
[Serializable]
class GameData{
	public int lite;
}