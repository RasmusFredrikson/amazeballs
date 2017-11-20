using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public float height = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (height);
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(height*20-10, 0, height*20-10));
	}
}
