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
        for (int i = 0; i < 4; i++)
        {
            Debug.LogError("Height" + i + " : " + height[i]);
        }

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(height[0] - height[2], 0, height[1] - height[3]));
    }
}
