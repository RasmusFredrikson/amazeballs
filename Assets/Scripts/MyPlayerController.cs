using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyPlayerController : NetworkBehaviour {

    public float distance = 0;

    public float height;

    [SyncVar(hook = "OnIdChange")]
    private int playerId = -1;

    private GameObject canvas;
    private Image image;
    private Text text;
    public static Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };

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
          
	}

    public void changeHeight(float height)
    {
        if (isLocalPlayer)
        {
            transform.position = new Vector3(height, -10, transform.position.z); // Test code
            CmdMove(height);

            if (canvas != null)
            {
                if (height >= 1)
                {
                    text.text = "HIGH";
                }
                else if (height <= -1)
                {

                    text.text = "LOW";
                }
                else
                {
                    text.text = "";
                }
            }
            

        }
    }

    [Command]
    void CmdMove(float dist)
    {
        distance = dist;
    }

    public void setID(int id)
    {
        if (isServer)
            playerId = id;
    }

    private void OnIdChange(int pId)
    {
        if (isLocalPlayer)
        {
            if (canvas == null)
            {
                canvas = Instantiate(Resources.Load("IndicatorCanvas")) as GameObject;
                text = canvas.transform.GetChild(1).gameObject.GetComponent<Text>();
                image = canvas.transform.GetChild(0).gameObject.GetComponent<Image>();
            }
                
            if (pId != -1 && pId <= 4)
                image.color = colors[pId];
        }
    }
}
