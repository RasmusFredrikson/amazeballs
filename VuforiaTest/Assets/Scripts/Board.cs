using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public float[] height; // public variable changed by GetPlayers

    private Rigidbody rb;

    public int mode = 1;

    private float ropeLength = 2f;

    // Use this for initialization
    void Start() {
        height = new float[4];

        for (int i = 0; i < 4; i++)
        {
            height[i] = 0;
        }

        if (mode == 2)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = 100;
        }
            
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; i++)
        {
            //Debug.LogError("Height" + i + " : " + height[i]);
        }
        
        if (Input.GetKeyDown("m")) 
        {
            mode = mode + 1;
            mode = mode % 4;
        }

        switch (mode)
        {
            case 0:
                rotateSides();
                break;
            case 1:
                holdCorner();
                break;
            case 2:
                //addForces();
                break;
            case 3:
                keybind();
                break;
        }
        
        
    }
    
    void keybind() {
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
    }

    // 2 Players or more
    void rotateSides()
    {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(height[0]*45 - height[2]*45, 0, height[1]*45- height[3] * 45));
    }

    // 3 Players
    void holdCorner()
    {
        Vector3 point1 = new Vector3(10, height[0]*10, 10);
        Vector3 point2 = new Vector3(10, height[1]*10, -10);
        Vector3 point3 = new Vector3(-10, height[2]*10, -10);

        Vector3 line1 = point1 - point2;
        Vector3 line2 = point1 - point3;
        Vector3 normal = Vector3.Cross(line1, line2);
        if (Vector3.Dot(normal, Vector3.up) < 0)
        {
            normal = -normal;
        }

        Vector3 pos = (point1 + point3) / 2f;

        gameObject.transform.position = Vector3.MoveTowards(transform.position, pos,  1 *Time.deltaTime);

        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(line1, normal), 150*Time.deltaTime);
    }

    // 4 Players
    void addForces()
    {
        Vector3 point1 = transform.position + transform.forward * 10 + transform.right * 10;
        Vector3 point2 = transform.position + transform.forward * 10 + transform.right * -10;
        Vector3 point3 = transform.position + transform.forward * -10 + transform.right * -10;
        Vector3 point4 = transform.position + transform.forward * -10 + transform.right * 10;


        Vector3 force1 = new Vector3(12, 4, 12) - point1;
        Vector3 force2 = new Vector3(-12, 4, 12) -point2;
        Vector3 force3 = new Vector3(-12, 4, -12) -point3;
        Vector3 force4 = new Vector3(12, 4,- 12) -point4;

        force1 = force1.normalized * (force1.magnitude > ropeLength ? Mathf.Pow(force1.magnitude, 2): 0);
        force2 = force2.normalized * (force2.magnitude > ropeLength ? Mathf.Pow(force2.magnitude, 2) : 0);
        force3 = force3.normalized * (force3.magnitude > ropeLength ? Mathf.Pow(force3.magnitude, 2) : 0);
        force4 = force4.normalized * (force4.magnitude > ropeLength ? Mathf.Pow(force4.magnitude , 2): 0);

        rb.AddForceAtPosition(force1, point1, ForceMode.VelocityChange);
        rb.AddForceAtPosition(force2, point2, ForceMode.VelocityChange);
        rb.AddForceAtPosition(force3, point3, ForceMode.VelocityChange);
        rb.AddForceAtPosition(force4, point4, ForceMode.VelocityChange);
    }
}
