using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScorePlayer : MonoBehaviour
{
    [SerializeField] protected string SCORE_PATH;
    private int score = 0;
    private int bestScore = 0;
    private RoadManager roadmanager;

    private void Start()
    {
        roadmanager = GetComponent<RoadManager>();
    }

    public void OnDisable()
    {
        roadmanager = GetComponent<RoadManager>();
        ScoreSaver();
    }
    public void ScoreSaver()
    {
        score = Mathf.CeilToInt(roadmanager.Point);
        string[] lines = File.ReadAllLines(SCORE_PATH);
        foreach (string l in lines)
        {
            bestScore = int.Parse(l);
        }
        if (bestScore > score)
            WriteScoreOnFile(bestScore);
        else
            WriteScoreOnFile(score);

    }
    public void WriteScoreOnFile(int s)
    {
        File.WriteAllText(SCORE_PATH, s.ToString());
    }
}
