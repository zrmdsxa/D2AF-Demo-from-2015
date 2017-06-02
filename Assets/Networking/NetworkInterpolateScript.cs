using UnityEngine;
using System.Collections;

public class NetworkInterpolateScript : MonoBehaviour {

	public float interpTime = 0.3f;
	internal struct State
	{
		internal Vector3 pos;
		internal Quaternion rot;
	}

	State s;

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// Always send transform (depending on reliability of the network view)
		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		// When receiving, buffer the information
		else
		{
			// Receive latest state information
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			s.pos = pos;
			s.rot = rot;
		}
	}

	// This only runs where the component is enabled, which is only on remote peers (server/clients)
	void Update () {
		transform.position = Vector3.Lerp (transform.position, s.pos, interpTime);
		transform.rotation = Quaternion.Slerp (transform.rotation, s.rot, interpTime);
	}
}
