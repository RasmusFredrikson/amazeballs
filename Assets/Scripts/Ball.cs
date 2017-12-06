using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class Ball : MonoBehaviour
{

    private float timeToFinishLine = 0f;
    private Text timeText;
    private Text highscoreText;

    // Use this for initialization
    void Start()
    {
        GameObject highscoreCanvas = Instantiate(Resources.Load("HighscoreCanvas")) as GameObject;
        timeText = highscoreCanvas.transform.GetChild(0).gameObject.GetComponent<Text>();
        highscoreText = highscoreCanvas.transform.GetChild(1).gameObject.GetComponent<Text>();
        timeText.text = "Time: " + timeToFinishLine.ToString();
        highscoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0){
            timeToFinishLine += Time.deltaTime;
            timeText.text = "Time: " + timeToFinishLine.ToString();
        }
        gameObject.transform.Rotate(1, 1, 1); //Keeps the ball from getting stuck
        if (gameObject.transform.position.y < -30f)
        {
            ResetBall();
        }

        if (Input.GetKeyDown("p"))
        {
            Time.timeScale = (Time.timeScale + 1) % 2;
            if (Time.timeScale == 0)
                ReadHighscore(false);
            else
                highscoreText.text = "";
        }
        if (Input.GetKeyDown("r"))
        {
            Time.timeScale = 1;
            ResetBall();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Time.timeScale = 0;
            string timeToFinishLineStr = timeToFinishLine.ToString();
            timeText.text = "Time: " + timeToFinishLineStr;
            SaveHighscore(timeToFinishLineStr);
            ReadHighscore(true);
        }
    }

    private void ResetBall()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = new Vector3(-3.92f, 4.0f, -4.06f);
        timeToFinishLine = 0;
        highscoreText.text = "";
    }

    void SaveHighscore(string timeToFinishLineStr)
    {
        string filePath = @"./Highscore.csv";
        string delimiter = ",";
        string[][] output;
        string timeStamp = DateTime.Now.ToString();
        string[] resultRow = new string[] { timeToFinishLineStr, timeStamp };

        if (!File.Exists(filePath))
        {
            Debug.Log("No highscore file found, creating a new one");
            output = new string[][] {
                new string[]{ "timetofinishline", "timestamp" },
                resultRow
            };
        }
        else
        {
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

    void ReadHighscore(Boolean reachedFinishedLine)
    {
        List<float> highScores = new List<float>();

        using (var reader = new StreamReader(@"./Highscore.csv"))
        {
            reader.ReadLine(); //Remove the header
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                highScores.Add(float.Parse(values[0]));
            }
        }

        highScores.Sort();
        if (reachedFinishedLine)
        {
            highscoreText.text = "You didn't reach a highscore :( \n";
            for (int i = 0; i < highScores.Count && i <= 10; i++)
            {
                if (timeToFinishLine < highScores[i])
                {
                    highscoreText.text = "You finished in " + i + " place. Congratz!\n";
                    break;
                }
            }
            highscoreText.text += "Press 'R' to play again. \n\nBest times \n";
        } else 
            highscoreText.text += "Paused. \n\nBest times \n";

        for (int i = 0; i < highScores.Count && i <= 10; i++)
        {
            highscoreText.text += Math.Round(highScores[i], 2) + "s\n";
        }
    }

    void compare() {
        
    }
}
