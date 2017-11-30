using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class Reset : MonoBehaviour {

	float timeToFinishLine = 0f;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		timeToFinishLine += Time.deltaTime;
		if (gameObject.transform.position.y < -30f) {
            ResetBall();
		}

		if (Input.GetKeyDown("r"))
		{
            ReadHighscore();
        }
	}

	void OnTriggerEnter (Collider other){
		if (other.tag == "Finish") {
			Debug.Log ("You have reached the goal");
			SaveHighscore ();
            ResetBall();
		}
	}

    private void ResetBall() {
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		gameObject.transform.position = new Vector3(-3.92f, 4.0f, -4.06f);
		timeToFinishLine = 0;
    }

    void SaveHighscore() {
		string filePath = @"./Highscore.csv";  
		string delimiter = ",";   
		string[][] output;
		string timeToFinishLineStr = timeToFinishLine.ToString();
		string timeStamp = DateTime.Now.ToString();
		string[] resultRow = new string[]{ timeToFinishLineStr, timeStamp};

		if (!File.Exists (filePath)) {
			Debug.Log ("No highscore file found, creating a new one");
			output = new string[][] {
				new string[]{ "timetofinishline", "timestamp" },  
				resultRow
			}; 
		} else {
			output = new string[][] {
				resultRow
			}; 
		}

		int length = output.GetLength(0);  
		StringBuilder sb = new StringBuilder();  
		for (int index = 0; index < length; index++)  
			sb.AppendLine(string.Join(delimiter, output[index]));  

		File.AppendAllText(filePath, sb.ToString());    

	}

	void ReadHighscore(){
		List<string[]> highScores = new List<string[]>();

		using(var reader = new StreamReader(@"./Highscore.csv"))
		{ 
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				var values = line.Split(',');
				highScores.Add(values);
			}
		}
		foreach(var highScore in highScores) {
			var highScorePrint = "";
			foreach (var column in highScore) {
				highScorePrint += " : " + column;
			}
			Debug.Log(highScorePrint);
		}
	}
}
