using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

// Leaderboard script, based on the following source: https://www.grimoirehex.com/unity-3d-local-leaderboard/

public class PlayerInfo
{
    public string name;
    public float time1;
    public float time2;
    public float time3;
    public float angle;
    public float score;

    public PlayerInfo(string name, float time1, float time2, float time3, float angle, float score)
    {
        this.name = name;
        this.time1 = time1;
        this.time2 = time2;
        this.time3 = time3;
        this.angle = angle;
        this.score = score;
    }
}

public class leaderboard : MonoBehaviour
{

    //Use TextMeshPro to display the data on the screen
    public TextMeshPro ranks;
    public TextMeshPro names;
    public TextMeshPro times;
    public TextMeshPro angles;
    public TextMeshPro scores;

    public TextMeshPro checkpointRanks;
    public TextMeshPro checkpointNames;
    public TextMeshPro checkpointTimes;


    //List To Hold "PlayerInfo" Objects
    List<PlayerInfo> collectedStats;

    private PlayerInfo latestEntry;

    private bool boardShown, boardPrevShown = true;
    private bool checkpointBoardShown, checkpointBoardPrevShown = true;

    private string filename = "leaderboardData.csv";


    // Use this for initialization
    public void Start()
    {
        collectedStats = new List<PlayerInfo>();
    }

    // Shows the leaderboard on-screen for the player
    public void ShowLeaderboard()
    {
        boardShown = true;
    }

    // Hides the leaderboard on-screen for the player
    public void HideLeaderboard()
    {
        boardShown = false;
    }

    // Shows the checkpoint leaderboard on-screen for the player
    public void ShowCheckpointLeaderboard()
    {
        checkpointBoardShown = true;
    }

    // Hides the checkpoint leaderboard on-screen for the player
    public void HideCheckpointLeaderboard()
    {
        checkpointBoardShown = false;
    }

    public void SaveFirstTime(string name, float time)
    {
        string csvLine = name + "," + time.ToString() + ",";
        System.IO.File.AppendAllText(filename, csvLine);
    }

    public void SaveSecondTime(float time)
    {
        string csvLine = time.ToString() + ",";
        System.IO.File.AppendAllText(filename, csvLine);
    }

    public void SaveTheRest(float time, float angle)
    {
        string csvLine = time.ToString() + ",";
        System.IO.File.AppendAllText(filename, csvLine);
    }

    // Saves a new complete entry (result) to the csv file
    public void SaveCompleteResult(string name, float time1, float time2, float time3, float angle)
    {
        string csvLine = name + "," + time1.ToString() + "," + time2.ToString() + "," + time3.ToString() + "," + angle.ToString();
        System.IO.File.AppendAllText(filename, csvLine + Environment.NewLine);
    }

    // Update is called once per frame
    void Update()
    {
        // Enable/Disable leaderboard
        if (boardShown && !boardPrevShown)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Leaderboard"))
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
            boardPrevShown = true;
            print("Leaderboard enabled");
        }
        else if (boardPrevShown && !boardShown)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Leaderboard"))
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
            boardPrevShown = false;
            print("Leaderboard disabled");
        }
        // Enable/Disable checkpoint leaderboard
        if (boardShown && !boardPrevShown)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint Leaderboard"))
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
            checkpointBoardPrevShown = true;
            print("Leaderboard enabled");
        }
        else if (boardPrevShown && !boardShown)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint Leaderboard"))
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
            checkpointBoardPrevShown = false;
            print("Leaderboard disabled");
        }
    }

    /*public void SubmitButton()
    {
        //Create Object Using Values From InputFields, This Is Done So That A Name And Score Can Easily Be Moved/Sorted At The Same Time
        PlayerInfo stats = new PlayerInfo(name.text, int.Parse(score.text));//Depending On How You Obtain The Score, It May Be Necessary To Parse To Integer

        //Add The New Player Info To The List
        collectedStats.Add(stats);

        //Clear InputFields Now That The Object Has Been Created
        name.text = "";
        score.text = "";

        //Start Sorting Method To Place Object In Correct Index Of List
        SortStats();
    }*/

    public void SortStats()
    {
        latestEntry = collectedStats.Last();
        //Bubble sort requires nxn passes for a list of n elements
        for (int i = 1; i < collectedStats.Count; i++)
        {
            //Start at the beginning of the list and compare the score with the one below it
            for (int j = 0; j < collectedStats.Count - i; j++)
            {
                //If The Current Score Is Higher Than The Score Above It , Swap
                if (collectedStats[j].score < collectedStats[j + 1].score)
                {
                    //Temporary variable to hold small score
                    PlayerInfo tempInfo = collectedStats[j + 1];

                    // Replace small score with big score
                    collectedStats[j + 1] = collectedStats[j];

                    //Set small score closer to the end of the list by placing it at "i" rather than "i-1" 
                    collectedStats[j] = tempInfo;
                }
            }
        }

        //Update PlayerPref That Stores Leaderboard Values
        UpdateLeaderBoardVisual();
    }

    /*public void UpdatePlayerPrefsString()
    {
        //Start With A Blank String
        string stats = "";

        //Add Each Name And Score From The Collection To The String
        for (int i = 0; i < collectedStats.Count; i++)
        {
            //Be Sure To Add A Comma To Both The Name And Score, It Will Be Used To Separate The String Later
            stats += collectedStats[i].name + ",";
            stats += collectedStats[i].time + ",";
            stats += collectedStats[i].angle + ",";
            stats += collectedStats[i].score + ",";
        }

        //Add The String To The PlayerPrefs, This Allows The Information To Be Saved Even When The Game Is Turned Off
        PlayerPrefs.SetString("LeaderBoards", stats);

        //Now Update The On Screen LeaderBoard
        UpdateLeaderBoardVisual();
    }*/

    public void UpdateLeaderBoardVisual()
    {
        //Clear Current Displayed LeaderBoard
        ranks.text = "";
        names.text = "";
        times.text = "";
        angles.text = "";
        scores.text = "";
        print("Here!");

        //Simply Loop Through The List And Add The Data To The Display Text
        for (int i = 0; i < collectedStats.Count; i++)
        {
            //Display up to 6 rows (plus the headers)
            if (i >= 6)
                break;

            //Make latest entry red, so it is easily identifiable
            if (collectedStats[i] == latestEntry)
            {
                ranks.text += "<color=red>";
                names.text += "<color=red>";
                times.text += "<color=red>";
                angles.text += "<color=red>";
                scores.text += "<color=red>";
            }

            ranks.text += (i + 1).ToString() + "\n";
            names.text += collectedStats[i].name + "\n";
            times.text += collectedStats[i].time3.ToString() + "\n";
            angles.text += collectedStats[i].angle.ToString() + "\n";
            scores.text += collectedStats[i].score.ToString() + "\n";

            if (collectedStats[i] == latestEntry)
            {
                ranks.text += "</color>";
                names.text += "</color>";
                times.text += "</color>";
                angles.text += "</color>";
                scores.text += "</color>";
            }
        }
    }

    private float calculateScore(float time, float angle)
    {
        if (angle < 1)
            angle = 1;
        float score = (1 / time + 1 / angle) * 100;
        return score;
    }

    public void LoadLeaderBoardStats()
    {
        ClearPrefs();
        String fileData = System.IO.File.ReadAllText(filename);
        String[] lines = fileData.Split('\n');

        //Results stored in csv file in chronological order (last line will be blank, ready to store new data)
        for (int i = 0; i < lines.Length - 1; i++)
        {
            String[] lineData = lines[i].Split(',');
            //Name, time (in seconds) and size of angle (in degrees) they were off by when looking for their home
            String name = lineData[0];
            float time1 = float.Parse(lineData[1]);
            float time2 = float.Parse(lineData[2]);
            float time3 = float.Parse(lineData[3]);
            float angle = float.Parse(lineData[4]);
            //Score is calculated from time and angle (the less the better, for both)
            float score = calculateScore(time3, angle);
            PlayerInfo loadedInfo = new PlayerInfo(name, time1, time2, time3, angle, score);
            collectedStats.Add(loadedInfo);
        }

        //Sort stats by score (and then display them on the screen)
        SortStats();
    }

    public void ClearPrefs()
    {
        //Use This To Delete All Names And Scores From The LeaderBoard
        PlayerPrefs.DeleteAll();
        collectedStats.Clear();

        //Clear Current Displayed LeaderBoard
        ranks.text = "";
        names.text = "";
        times.text = "";
        angles.text = "";
        scores.text = "";
    }
}
