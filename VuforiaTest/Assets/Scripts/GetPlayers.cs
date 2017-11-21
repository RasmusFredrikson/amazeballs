using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayers : MonoBehaviour {

    public MyPlayerController[] players;

    private GameObject board;

    private Text text;

	// Use this for initialization
	void Start () {
        board = GameObject.Find("Board");
        if (board == null)
        {
            Debug.LogError("'Board' missing from scene");
        }

        GameObject UI = GameObject.Find("Debug UI");
        if (UI != null)
        {
            text = UI.transform.GetChild(1).gameObject.GetComponent<Text>();
        }
        if (text == null)
        {
            Debug.LogError("Debug Text missing from 'Debug UI' or scene");
        }

        players = new MyPlayerController[4];
        //players[0] = null;
        players[1] = null;
        players[2] = null;
        players[3] = null;

    }

    // Update is called once per frame
    void Update () {
        text.text = Network.player.ipAddress;


        if (players[0] == null)
        {
            text.text = "\nNo player 1";
        } else
        {
            text.text = "\nP1: " + players[0].distance.ToString() + " ";

            board.GetComponent<Board>().height = players[0].distance;
        }

        if (players[1] == null)
        {
            text.text = text.text + "\nNo player 2";
        } else
        {
            text.text = text.text + "\nP2: " + players[1].distance.ToString();
        }
	}
}
