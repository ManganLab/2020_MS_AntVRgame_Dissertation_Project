using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

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

    public void SaveThirdTime(float time)
    {
        string csvLine = time.ToString() + ",";
        System.IO.File.AppendAllText(filename, csvLine);
    }

    public void SaveAngle(float angle)
    {
        string csvLine = angle.ToString();
        System.IO.File.AppendAllText(filename, csvLine + Environment.NewLine);
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
        if (checkpointBoardShown && !checkpointBoardPrevShown)
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
        else if (checkpointBoardPrevShown && !checkpointBoardShown)
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

    //Sorting the stats by their overall score
    public void SortStatsByScore()
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
                    //Temporary variable to hold smaller score
                    PlayerInfo tempInfo = collectedStats[j + 1];

                    // Replace small score with bigger score
                    collectedStats[j + 1] = collectedStats[j];

                    //Set small score closer to the end of the list by placing it at "i" rather than "i-1" 
                    collectedStats[j] = tempInfo;
                }
            }
        }
    }

    //Sorting the stats by the time at checkpoint 1, 2 or 3
    public void SortStatsByTime(int checkpoint)
    {
        latestEntry = collectedStats.Last();

        //Bubble sort requires nxn passes for a list of n elements
        for (int i = 1; i < collectedStats.Count; i++)
        {
            //Start at the beginning of the list and compare the score with the one below it
            for (int j = 0; j < collectedStats.Count - i; j++)
            {
                if (checkpoint == 1)
                {
                    //If The Current Time Is Lower Than The Score Above It , Swap
                    if (collectedStats[j].time1 > collectedStats[j + 1].time1)
                    {
                        //Temporary variable to hold smaller time
                        PlayerInfo tempInfo = collectedStats[j + 1];

                        // Replace smaller time with bigger time
                        collectedStats[j + 1] = collectedStats[j];

                        //Set smaller time closer to the end of the list by placing it at "i" rather than "i-1" 
                        collectedStats[j] = tempInfo;
                    }
                }
                else if (checkpoint == 2)
                {
                    //If The Current Time Is Lower Than The Score Above It , Swap
                    if (collectedStats[j].time2 > collectedStats[j + 1].time2)
                    {
                        //Temporary variable to hold smaller time
                        PlayerInfo tempInfo = collectedStats[j + 1];

                        // Replace smaller time with bigger time
                        collectedStats[j + 1] = collectedStats[j];

                        //Set smaller time closer to the end of the list by placing it at "i" rather than "i-1" 
                        collectedStats[j] = tempInfo;
                    }
                }
                else if (checkpoint == 3)
                {
                    //If The Current Time Is Lower Than The Score Above It , Swap
                    if (collectedStats[j].time3 > collectedStats[j + 1].time3)
                    {
                        //Temporary variable to hold smaller time
                        PlayerInfo tempInfo = collectedStats[j + 1];

                        // Replace smaller time with bigger time
                        collectedStats[j + 1] = collectedStats[j];

                        //Set smaller time closer to the end of the list by placing it at "i" rather than "i-1" 
                        collectedStats[j] = tempInfo;
                    }
                }
            }
        }

        UpdateCheckpointLeaderBoardVisual(checkpoint);
    }

    public void UpdateLeaderBoardVisual()
    {
        int latestIndex = collectedStats.IndexOf(latestEntry);
        List<int> indexesToTry = new List<int>();
        indexesToTry.Add(0);
        if (latestIndex > 0)
            indexesToTry.Add(latestIndex - 1);
        indexesToTry.Add(latestIndex);
        if (latestIndex < collectedStats.Count - 2)
            indexesToTry.Add(latestIndex + 1);
        indexesToTry.Add(collectedStats.Count - 1);
        int previousIndex = 0;
        //Loop through the indexes in the list (ignoring duplicates) and display their stats
        foreach (int index in indexesToTry.Distinct())
        {
            if (index - previousIndex > 1)
            {
                ranks.text += "...\n";
                names.text += "...\n";
                times.text += "...\n";
                angles.text += "...\n";
                scores.text += "...\n";

            }
            //Make current attempt red, so it is easily identifiable
            if (collectedStats[index] == latestEntry)
            {
                ranks.text += "<color=red>";
                names.text += "<color=red>";
                times.text += "<color=red>";
                angles.text += "<color=red>";
                scores.text += "<color=red>";
            }

            ranks.text += (index + 1).ToString() + "\n";
            names.text += collectedStats[index].name + "\n";
            times.text += collectedStats[index].time3.ToString() + "\n";
            angles.text += collectedStats[index].angle.ToString() + "\n";
            scores.text += collectedStats[index].score.ToString() + "\n";

            if (collectedStats[index] == latestEntry)
            {
                ranks.text += "</color>";
                names.text += "</color>";
                times.text += "</color>";
                angles.text += "</color>";
                scores.text += "</color>";
            }
            previousIndex = index;
        }
    }

    public void UpdateCheckpointLeaderBoardVisual(int checkpoint)
    {
        int latestIndex = collectedStats.IndexOf(latestEntry);
        List<int> indexesToTry = new List<int>();
        indexesToTry.Add(0);
        if (latestIndex > 0)
            indexesToTry.Add(latestIndex - 1);
        indexesToTry.Add(latestIndex);
        if (latestIndex < collectedStats.Count - 2)
            indexesToTry.Add(latestIndex + 1);
        indexesToTry.Add(collectedStats.Count - 1);
        int previousIndex = 0;
        //Loop through the indexes in the list (ignoring duplicates) and display their stats
        foreach (int index in indexesToTry.Distinct())
        {
            if (index - previousIndex > 1)
            {
                checkpointRanks.text += "...\n";
                checkpointNames.text += "...\n";
                checkpointTimes.text += "...\n";

            }
            //Make current attempt red, so it is easily identifiable
            if (collectedStats[index] == latestEntry)
            {
                checkpointRanks.text += "<color=red>";
                checkpointNames.text += "<color=red>";
                checkpointTimes.text += "<color=red>";
            }

            checkpointRanks.text += (index + 1).ToString() + "\n";
            checkpointNames.text += collectedStats[index].name + "\n";
            if (checkpoint == 1)
                checkpointTimes.text += collectedStats[index].time1.ToString() + "\n";
            else if (checkpoint == 2)
                checkpointTimes.text += collectedStats[index].time2.ToString() + "\n";
            else
                checkpointTimes.text += collectedStats[index].time3.ToString() + "\n";

            if (collectedStats[index] == latestEntry)
            {
                checkpointRanks.text += "</color>";
                checkpointNames.text += "</color>";
                checkpointTimes.text += "</color>";
            }
            previousIndex = index;
        }
    }

    private float calculateScore(float time, float angle)
    {
        float score = (1 / time + 1 / angle) * 100;
        score = Mathf.Round(score * 100.0f) / 100.0f;
        return score;
    }

    public void LoadLeaderBoardStats()
    {
        ClearPrefs();
        string[] lines = System.IO.File.ReadAllLines(filename);

        //Results stored in csv file in chronological order (last line will be the current attempt)
        for (int i = 0; i < lines.Length; i++)
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
        SortStatsByScore();

        //Update LeaderBoard on screen
        UpdateLeaderBoardVisual();
    }

    public void LoadCheckpointLeaderBoardStats(int checkpoint)
    {
        ClearCheckpointPrefs();
        string[] lines = System.IO.File.ReadAllLines(filename);

        //Results stored in csv file in chronological order (last line will be the current attempt)
        for (int i = 0; i < lines.Length; i++)
        {
            String[] lineData = lines[i].Split(',');
            //Name, time (in seconds) and size of angle (in degrees) they were off by when looking for their home
            String name = lineData[0];
            float time1 = float.Parse(lineData[1]);
            float time2, time3, angle, score;
            time2 = time3 = angle = score = 0f;
            if (checkpoint > 1)
            {
                time2 = float.Parse(lineData[2]);

                if (checkpoint > 2)
                {
                    time3 = float.Parse(lineData[3]);
                }
            }
            PlayerInfo loadedInfo = new PlayerInfo(name, time1, time2, time3, angle, score);
            collectedStats.Add(loadedInfo);
        }
        SortStatsByTime(checkpoint);
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

    public void ClearCheckpointPrefs()
    {
        //Use This To Delete All Names And Scores From The LeaderBoard
        PlayerPrefs.DeleteAll();
        collectedStats.Clear();

        //Clear Current Displayed Checkpoint LeaderBoard
        checkpointRanks.text = "";
        checkpointNames.text = "";
        checkpointTimes.text = "";
    }

    void OnApplicationQuit()
    {
        string[] lines = System.IO.File.ReadAllLines(filename);
        int deleteLines = 0;

        //If last char of last line is a comma (ie: line is not complete), delete last line
        if (lines.Last().Last() == ',')
            deleteLines = 1;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < (lines.Length - deleteLines); i++)
            sb.AppendLine(lines[i].Replace("Cur. Attempt", "Prev. Attempt"));

        System.IO.File.WriteAllText(filename, sb.ToString());
    }
}
