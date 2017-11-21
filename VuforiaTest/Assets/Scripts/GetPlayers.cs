using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayers : MonoBehaviour {

    MyPlayerController player1;
    MyPlayerController player2;

    public GameObject board;

    public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player1 == null)
        {
            player1 = GameObject.Find("Player(Clone)").GetComponent<MyPlayerController>();
        } else
        {
            Debug.Log("player1 is found" + player1.distance.ToString());
            text.text = player1.distance.ToString();

            //board.transform.rotation = Quaternion.Euler(new Vector3(player1.distance * 20 - 10, 0, player1.distance * 20 - 10));
            board.GetComponent<Board>().height = player1.distance;
        }

        if (player2 == null)
        {
            
        } else
        {
            Debug.Log("player1 is found" + player2.distance.ToString());
        }
	}
}
