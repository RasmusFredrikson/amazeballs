using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public float height = 0;

	public float[] corners; // LEFT RIGHT UP DOWN

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(new Vector3(height/10,0, 0));


	}
}
