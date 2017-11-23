using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompareHeight : MonoBehaviour {

	private GameObject cameraLocation;
	private GameObject targetLocation;
	private Text text;

    public MyPlayerController player; // Local player

    protected Vector3 diff = Vector3.zero;

	protected float height;

    public float initHeight = 4f;
    public float maxHeight = 10f;
    public float minHeight = 1f;

	// Use this for initialization
	void Start () {
        height = initHeight;

        cameraLocation = GameObject.Find("ARCamera");
        targetLocation = GameObject.Find("ImageTarget");

        GameObject UI = GameObject.Find("Debug UI");
        if (UI != null)
        {
            text = UI.transform.GetChild(1).gameObject.GetComponent<Text>();
        }

        if (cameraLocation == null || targetLocation == null)
        {
            Debug.LogError("ARCamera or ImageTarget missing from scene");
        }
        if (text == null)
        {
            Debug.LogError("Debug Text missing from 'Debug UI' or scene");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (targetLocation.transform.position.magnitude > 0) { // TODO Check if target is tracked

			diff = cameraLocation.transform.position - targetLocation.transform.position;
			height = diff.magnitude;

			text.text = height.ToString () + " , Active";
		} else {
			text.text = height.ToString() + " , Not active";
		}

        if (player == null)
        {
            Debug.LogError("Player is missing");
        } else
        {
            // Clamp height
            height = height < minHeight ? minHeight : height;
            height = height > maxHeight ? maxHeight : height;

            // Set the height on the player
            player.height = height;
	    }
    }
}
