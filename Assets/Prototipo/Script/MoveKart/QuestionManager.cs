using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] protected GameObject panelTrue;
    [SerializeField] protected GameObject panelFalse;
    [SerializeField] protected string EASY_QUESTIONS;
    [SerializeField] protected string MEDIUM_QUESTIONS;
    [SerializeField] protected string HARD_QUESTIONS;
    [SerializeField] protected GameObject roadObject;
    protected Dictionary<string, int> easyQuestions = new Dictionary<string, int>();
    protected RoadManager roadManager;
    protected QuestionGUI questionGUI;
    private bool easy;
    private bool medium;
    private bool hard;

    private void Awake()
    {
        roadManager = roadObject.GetComponent<RoadManager>();
        questionGUI = gameObject.GetComponent<QuestionGUI>();
        ReadQuestions(EASY_QUESTIONS, easyQuestions);
    }

    private void Start()
    {
        easy = roadManager.GetEasy;
        medium = roadManager.GetMedium;
        hard = roadManager.GetHard;

        //foreach(string q in easyQuestions.Keys)
        //Debug.Log("  " + q);

        PickQuestion();
    }

    public void ReadQuestions(string fileInput, Dictionary<string, int> questions)
    {
        Dictionary<string, int> questionTable = new Dictionary<string, int>();
        string[] lines = File.ReadAllLines(fileInput);
        foreach (string l in lines)
        {
            string[] items = l.Split(';');
            string param0 = items[0].Trim();
            int param1 = int.Parse(items[1].Trim());
            questions.Add(param0, param1);
        }
    }

    public void PickQuestion()
    {
        if (easyQuestions.Count == 0)
        {
            ReadQuestions(EASY_QUESTIONS, easyQuestions);
        }
        int rand = UnityEngine.Random.Range(0, easyQuestions.Count);
        var question = easyQuestions.ElementAt(rand);
        questionGUI.SetTextQuestion(question.Key);
        //Debug.Log(" " + question.Key + " " + question.Value);
        easyQuestions.Remove(question.Key);        

    }

    public GameObject GetPanelTrue
    {
        get { return panelTrue;}
    }

    public GameObject GetPanelFalse
    {
        get { return panelFalse; }
    }
}
