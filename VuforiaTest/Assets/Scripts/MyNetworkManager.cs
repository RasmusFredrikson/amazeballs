using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// Source https://stackoverflow.com/questions/40919858/different-scenes-for-server-and-clients-using-unity-networking-hlapi

public class MyNetworkManager : NetworkManager {

	public override void OnClientSceneChanged(NetworkConnection conn) {
		SceneManager.LoadScene ("PhoneScene", LoadSceneMode.Additive);
		ClientScene.Ready (conn);
		ClientScene.AddPlayer (conn, 0); // TODO next available number
	}

	public override void OnServerSceneChanged(string scenename) {
		SceneManager.LoadScene ("GameScene", LoadSceneMode.Additive);
	}


}
