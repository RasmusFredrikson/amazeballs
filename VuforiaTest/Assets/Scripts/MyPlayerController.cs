using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyPlayerController : NetworkBehaviour {

    public float distance = 0;

    public float height;

	// Use this for initialization
	void Start () {
        height = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Not local");
        if (isLocalPlayer)
        {
            Debug.Log("Local");
            transform.position = new Vector3(height, -10, transform.position.z); // Test code
            CmdMove(height);
        }

        
	}

    [Command]
    void CmdMove(float dist)
    {
        distance = dist;
    }
}
