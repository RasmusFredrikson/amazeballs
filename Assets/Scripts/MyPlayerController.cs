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
        
        if (isLocalPlayer)
        {
            // Add to compareHeight
            GameObject shc = GameObject.Find("ScriptHolderClient");
            if (shc == null || shc.GetComponent<CompareHeight>() == null)
            {
                Debug.LogError("CompareHeight not found on ScriptHolderClient");
            }else
            {
                shc.GetComponent<CompareHeight>().player = this;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
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
