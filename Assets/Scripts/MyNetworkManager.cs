using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using System;

// Source https://stackoverflow.com/questions/40919858/different-scenes-for-server-and-clients-using-unity-networking-hlapi

public class MyNetworkManager : NetworkManager {

	public override void OnClientSceneChanged(NetworkConnection conn) {

        Debug.Log("Client Scene");

		SceneManager.LoadScene ("Scenes/PhoneScene", LoadSceneMode.Additive);
		ClientScene.Ready (conn);

        ClientScene.AddPlayer (conn, 0);
	}

	public override void OnServerSceneChanged(string scenename) {
        Debug.Log("Server Scene");
		SceneManager.LoadScene ("Scenes/GameScene", LoadSceneMode.Additive);
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.LogError("Added player" + playerControllerId.ToString());

        Debug.LogError(conn.connectionId);

        MyPlayerController player = conn.playerControllers[0].gameObject.GetComponent<MyPlayerController>();

        int playerID = conn.connectionId - 1;

        GameObject shs = GameObject.Find("ScriptHolderServer");
        if (shs == null || shs.GetComponent<GetPlayers>() == null)
        {
            Debug.LogError("GetPlayer not found on ScriptHolderServer");
        }else
        {
            shs.GetComponent<GetPlayers>().players[playerID] = player;
            player.setID(playerID);
        }

        

        Debug.LogError(player.ToString());
    }

}
