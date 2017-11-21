using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public float height = 0; // public variable changed by GetPlayers

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(height*4-10, 0, height*4-10));
	}
}
