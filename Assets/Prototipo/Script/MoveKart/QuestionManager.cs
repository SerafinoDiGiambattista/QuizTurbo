using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] protected GameObject[] panels; //0=False 1=True
    [SerializeField] protected string QUESTIONS_DIR;
    [SerializeField] protected GameObject roadObject;
    [SerializeField] protected int changeToCorrectAnsw = 1;
    [SerializeField] protected int changeTutorial = 1;
    protected TutorialManager tutorialManager;
    protected RoadManager roadManager;
    protected QuestionGUI questionGUI;
    private int numCorrectAnsw = 0;
    private int correctAnswer = 0;
    private int index = 0;
    List<Dictionary<string, int>> dictionaryList = new List<Dictionary<string, int>>();

    private void Awake()
    {
        tutorialManager = roadObject.GetComponent<TutorialManager>();
        roadManager = roadObject.GetComponent<RoadManager>();
        questionGUI = gameObject.GetComponent<QuestionGUI>();
        ReadQuestions(QUESTIONS_DIR);
    }

    private void Start()
    {
        PickQuestion(0);
    }
    public void ReadQuestions(string directory)
    {
        DirectoryInfo dir = new DirectoryInfo(directory);
        FileInfo[] files = dir.GetFiles("*.csv");
        
        foreach (FileInfo f in files)
        {
            Dictionary<string, int> questions = new Dictionary<string, int>();
            string path = (directory + "/" + f.Name).ToString();
            //Debug.Log("String: " + path);
            string[] lines = File.ReadAllLines(path);
            foreach (string l in lines)
            {
                string[] items = l.Split(';');
                string param0 = items[0].Trim();
               
                int param1 = int.Parse(items[1].Trim());
                //Debug.Log("param0: " + param0+" "+ param1);
                questions.Add(param0, param1);
            }
            dictionaryList.Add(questions);
        }    
    }

    public void QuestionCountPick()
    {
        if (index >= dictionaryList.Count) index = 0;
        
        if (!tutorialManager.GetTutorial)
        {
            if (numCorrectAnsw < changeToCorrectAnsw)
            {
                PickQuestion(index);
            }
            else
            {
                Debug.Log("index : "+ index);
                index++;
                numCorrectAnsw = 0;
                PickQuestion(index);
            }
        }
    }

    public void CheckResetTutorial()
    {
       
        if (numCorrectAnsw == changeTutorial)
        { 
            tutorialManager.FalseTutorial();
            numCorrectAnsw = 0;
            index = 0;
            roadManager.Point = 0;
            roadManager.ResetHealth();
            roadManager.ResetScoreMultiplier();
        }
        else PickQuestion(0);
    }

    public void PickQuestion( int ind)
    {
        //Debug.Log(ind);
        if (dictionaryList[ind].Count == 0)
        {
            ReadQuestions(QUESTIONS_DIR);
        }
        int rand = UnityEngine.Random.Range(0, dictionaryList[ind].Count);
        //Debug.Log("Rand: " + rand);
        var question = dictionaryList[ind].ElementAt(rand);
        correctAnswer = question.Value;
        questionGUI.SetTextQuestion(question.Key);
        //Debug.Log(" " + question.Key + " " + question.Value);
        dictionaryList[ind].Remove(question.Key);
    }



    public GameObject[] GetPanels
    {
        get { return panels;}
    }

    public GameObject GetCorrectAnswer()
    {
        
        return panels[correctAnswer];
    }

    public void IncrementCorrectAnsw()
    {
      
        numCorrectAnsw++;
        if(tutorialManager.GetTutorial) CheckResetTutorial();

    }

    public void ActivateCanvasQuestion()
    {
        QuestionCountPick();
        questionGUI.CanvasQuestion.SetActive(true);
    }

    public void DeactivateCanvasQuestion()
    {
        questionGUI.CanvasQuestion.SetActive(false);
    }
}
