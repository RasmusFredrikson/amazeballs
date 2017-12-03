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
    }

    // Update is called once per frame
    void Update () {
        text.text = Network.player.ipAddress;

        for (int i = 0; i < 4 || i < players.Length; i++)
        {
            if (players[i] == null)
            {
                text.text = text.text + "\nNo player " + (i + 1).ToString();
            }
            else
            {
                text.text = text.text +  "\nP" + (i + 1).ToString() + ": " + players[i].distance.ToString() + " ";

                board.GetComponent<Board>().height[i] = players[i].distance;
            }


        }
    }
}
