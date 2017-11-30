using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public float[] height; // public variable changed by GetPlayers

    // Use this for initialization
    void Start() {
        height = new float[4];

        for (int i = 0; i < 4; i++)
        {
            height[i] = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		//        for (int i = 0; i < 4; i++)
		//        {
		//            Debug.LogError("Height" + i + " : " + height[i]);
		//        }

		if (Input.GetKeyDown("w"))
		{
            gameObject.transform.Rotate(new Vector3(-5, 0, 0));
		}

		else if (Input.GetKeyDown("s"))
		{
			gameObject.transform.Rotate(new Vector3(5, 0, 0));
		}

		else if (Input.GetKeyDown("a"))
		{
			gameObject.transform.Rotate(new Vector3(0, 0, -5));
		}

		else if (Input.GetKeyDown("d"))
		{
			gameObject.transform.Rotate(new Vector3(0, 0, 5));
		}

		//gameObject.transform.rotation = Quaternion.Euler(new Vector3(height[0]*45 - height[2]*45, 0, height[1]*45 - height[3]*45));
    }
}
