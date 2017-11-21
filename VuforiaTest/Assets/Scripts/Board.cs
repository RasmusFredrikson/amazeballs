using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public float height = 0;

    public float ydir = 0;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
        

		Debug.Log (height);
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(height*4-10, 0, height*4-10));

        //gameObject.transform.Rotate(new Vector3(height, 0, 0));
	}
}
