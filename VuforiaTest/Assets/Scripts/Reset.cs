using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if (gameObject.transform.position.y < -30f) {
			gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			gameObject.transform.position = new Vector3 (0, 4.0f, 0);
		}


	}
}
