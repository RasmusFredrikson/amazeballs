using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public float[] height; // public variable changed by GetPlayers

    private Rigidbody rb;
    private GameObject indicators;
    private GameObject selectedBoard;

    private int mode = 1;
    public int setMode = 4;

    private int boardLevel = 0;

    private float ropeLength = 2f;

    private BoardConstants boardConstants; 
        
    // Use this for initialization
    void Start() {
        height = new float[4];

        for (int i = 0; i < 4; i++)
        {
            height[i] = 0;
        }

        indicators = Instantiate(Resources.Load("IndicatorHolder"), transform, false) as GameObject;
        indicators.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y, 1f / transform.localScale.z);

        Renderer rend1 = indicators.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        rend1.material.SetColor("_Color", Color.red);
        Renderer rend2 = indicators.transform.GetChild(1).gameObject.GetComponent<Renderer>();
        rend2.material.SetColor("_Color", Color.blue);
        Renderer rend3 = indicators.transform.GetChild(2).gameObject.GetComponent<Renderer>();
        rend3.material.SetColor("_Color", Color.green);
        indicators.SetActive(false);

        changeBoard();

        for (int i = 1; i < setMode; i++)
        {
            changeMode();
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
            changeMode();
        }

        if (Input.GetKeyDown("b"))
        {
            changeBoard();    
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
            case 4:
                holdCornerMiddle();
                break;
        }

        if (mode == 1 || mode == 4)
        {
            for(int i = 0; i < 3; i++)
            {
                if (height[i] <= -1)
                {
                    SetText(indicators.transform.GetChild(i).gameObject, "Low");
                }else if (height[i] >= 1)
                {
                    SetText(indicators.transform.GetChild(i).gameObject, "High");
                }else
                {
                    SetText(indicators.transform.GetChild(i).gameObject, "");
                }
            }
        }
        
        
    }

    void changeBoard()
    {
        boardLevel = (boardLevel + 1) % 4;
        Destroy(selectedBoard);

        switch (boardLevel)
        {
            case 0:
                selectedBoard = Instantiate(Resources.Load("SimpleBoard"), transform, false) as GameObject;
                break;
            case 1:
                selectedBoard = Instantiate(Resources.Load("SpiralBoard"), transform, false) as GameObject;
                break;
            case 2:
                selectedBoard = Instantiate(Resources.Load("ComplicatedBoard"), transform, false) as GameObject;
                break;
            case 3:
                selectedBoard = Instantiate(Resources.Load("LavaBoard"), transform, false) as GameObject;
                break;
        }

        boardConstants = selectedBoard.GetComponent<BoardConstants>() as BoardConstants;
        GameObject.Find("Ball").transform.position = boardConstants.spawnPosition;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        indicators.transform.GetChild(0).position = new Vector3(-(boardConstants.width + 1f), 0, boardConstants.length + 1f);
        indicators.transform.GetChild(1).position = new Vector3(boardConstants.width + 1f, 0, boardConstants.length + 1f);
        indicators.transform.GetChild(2).position = new Vector3(1f, 0, - (boardConstants.length + 1f));
    }

    void changeMode()
    {
        // Remove mode specifics
        switch (mode)
        {
            case 1:
                indicators.SetActive(false);
                break;
            case 2:
                Destroy(gameObject.GetComponent<Rigidbody>());
                rb = null;
                break;
            case 4:
                indicators.SetActive(false);
                break;
        }
        mode = mode + 1;
        mode = mode % 5;

        // Add mode specifc
        switch (mode)
        {
            case 1:
                indicators.SetActive(true);
                indicators.transform.GetChild(0).position = new Vector3(boardConstants.width, 0, boardConstants.length);
                indicators.transform.GetChild(1).position = new Vector3(boardConstants.width, 0, -boardConstants.length);
                indicators.transform.GetChild(2).position = new Vector3(-boardConstants.width, 0, -boardConstants.length);
                break;
            case 2:
                rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.mass = 100;
                break;
            case 4:
                indicators.SetActive(true);
                indicators.transform.GetChild(0).position = new Vector3(-boardConstants.width, 0, boardConstants.length);
                indicators.transform.GetChild(1).position = new Vector3(boardConstants.width, 0, boardConstants.length);
                indicators.transform.GetChild(2).position = new Vector3(0, 0, -boardConstants.length);
                break;
        }
    }
    
    void keybind() {

        if (Input.GetKey("w"))
		    {
             gameObject.transform.Rotate(new Vector3(-5, 0, 0)* Time.deltaTime);
		    }
        else if (Input.GetKey("s"))
        {
        	gameObject.transform.Rotate(new Vector3(5, 0, 0) * Time.deltaTime);
        }
        else if (Input.GetKey("a"))
        {
        	gameObject.transform.Rotate(new Vector3(0, 0, -5)* Time.deltaTime);
        }
        else if (Input.GetKey("d"))
        {
        	gameObject.transform.Rotate(new Vector3(0, 0, 5)* Time.deltaTime);
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
        Vector3 point1 = new Vector3(boardConstants.width, height[0] * 10, boardConstants.length);
        Vector3 point2 = new Vector3(boardConstants.width, height[1] * 10, -boardConstants.length);
        Vector3 point3 = new Vector3(-boardConstants.width, height[2] * 10, -boardConstants.length);

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

    // 3 Players
    void holdCornerMiddle()
    {
        Vector3 point1 = new Vector3(-boardConstants.width, height[0] * 10, boardConstants.length);
        Vector3 point2 = new Vector3(boardConstants.width, height[1] * 10, boardConstants.length);
        Vector3 point3 = new Vector3(0, height[2] * 10, -boardConstants.length);

        Vector3 line1 = point3 - point1;
        Vector3 line2 = point3 - point2;
        Vector3 normal = Vector3.Cross(line1, line2);
        Vector3 forward =  ((point1 + point2)  - point3).normalized;
        if (Vector3.Dot(normal, Vector3.up) < 0)
        {
            normal = -normal;
        }

        Vector3 pos = (point3 + ((point1 + point2) / 2f)) / 2f;

        gameObject.transform.position = Vector3.MoveTowards(transform.position, pos, 1 * Time.deltaTime);

        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forward, normal), 150 * Time.deltaTime);
    }

    // 4 Players
    void addForces()
    {
        Vector3 point1 = transform.position + transform.forward * 10 + transform.right * 10;
        Vector3 point2 = transform.position + transform.forward * 10 + transform.right * -10;
        Vector3 point3 = transform.position + transform.forward * -10 + transform.right * -10;
        Vector3 point4 = transform.position + transform.forward * -10 + transform.right * 10;


        Vector3 force1 = new Vector3(boardConstants.width + 2, 4, boardConstants.length + 2) - point1;
        Vector3 force2 = new Vector3(-boardConstants.width + 2, 4, boardConstants.length + 2) - point2;
        Vector3 force3 = new Vector3(-boardConstants.width + 2, 4, -boardConstants.length + 2) - point3;
        Vector3 force4 = new Vector3(boardConstants.width + 2, 4,-boardConstants.length + 2) - point4;

        force1 = force1.normalized * (force1.magnitude > ropeLength ? Mathf.Pow(force1.magnitude, 2): 0);
        force2 = force2.normalized * (force2.magnitude > ropeLength ? Mathf.Pow(force2.magnitude, 2) : 0);
        force3 = force3.normalized * (force3.magnitude > ropeLength ? Mathf.Pow(force3.magnitude, 2) : 0);
        force4 = force4.normalized * (force4.magnitude > ropeLength ? Mathf.Pow(force4.magnitude , 2): 0);

        rb.AddForceAtPosition(force1, point1, ForceMode.VelocityChange);
        rb.AddForceAtPosition(force2, point2, ForceMode.VelocityChange);
        rb.AddForceAtPosition(force3, point3, ForceMode.VelocityChange);
        rb.AddForceAtPosition(force4, point4, ForceMode.VelocityChange);
    }

    void SetText(GameObject indicator, string message)
    {
        Transform textTransform = indicator.transform.GetChild(0).GetChild(0);

        textTransform.gameObject.GetComponent<Text>().text = message;
        textTransform.rotation = Quaternion.LookRotation(indicator.transform.position - UnityEngine.Camera.main.transform.position);
        textTransform.position = indicator.transform.position + textTransform.up*2;
    }
}
