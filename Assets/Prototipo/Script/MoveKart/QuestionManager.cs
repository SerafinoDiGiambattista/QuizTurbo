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
    [SerializeField] protected int changeToCorrectAnsw = 10;
    protected RoadManager roadManager;
    protected QuestionGUI questionGUI;
    private int numCorrectAnsw = 0;
    private int correctAnswer = 0;
    private int index = 0;
    List<Dictionary<string, int>> dictionaryList = new List<Dictionary<string, int>>();

    private void Awake()
    {
        roadManager = roadObject.GetComponent<RoadManager>();
        questionGUI = gameObject.GetComponent<QuestionGUI>();
        ReadQuestions(QUESTIONS_DIR);
    }

    private void Start()
    {
      
        //foreach(string q in easyQuestions.Keys)
        //Debug.Log("  " + q);

    }

    public void ReadQuestions(string directory)
    {
        DirectoryInfo dir = new DirectoryInfo(directory);
        FileInfo[] files = dir.GetFiles("*.csv");
        
        /*foreach(FileInfo f in files)
            Debug.Log("nome file: "+f.Name);*/
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

        /*Dictionary<string, int> li = dictionaryList[0];
            foreach (string s in li.Keys)
                Debug.Log("list: " + s);  */     
    }

    public void QuestionCountPick()
    {
        if (index >= dictionaryList.Count) index = 0;
        if (roadManager.IsTutorial)
        {
            Debug.Log("question for tutorial");
            CheckResetTutorial();
        }
        else if (numCorrectAnsw < changeToCorrectAnsw && !roadManager.IsTutorial)
        {
            Debug.Log("question for road");
            PickQuestion(index);
        }
        else
        {
            Debug.Log("question from reset");
            index++;
            numCorrectAnsw = 0;
            PickQuestion(index);
        }
    }

    public void CheckResetTutorial()
    {
        if (numCorrectAnsw == 2)
        {
            roadManager.IsTutorial = false;
            Debug.Log("TUTORIAL FINITO!");
            numCorrectAnsw = 0;
            index = 0;
            roadManager.Space = 0;
            roadManager.ResetHealth();
        }
        else PickQuestion(0);
    }

    public void PickQuestion( int ind)
    {
        if (dictionaryList[ind].Count == 0)
        {
            ReadQuestions(QUESTIONS_DIR);
        }
        int rand = UnityEngine.Random.Range(0, dictionaryList[ind].Count);
        Debug.Log("Rand: " + rand);
        var question = dictionaryList[ind].ElementAt(rand);
        correctAnswer = question.Value;
        questionGUI.SetTextQuestion(question.Key);
        Debug.Log(" " + question.Key + " " + question.Value);
        dictionaryList[ind].Remove(question.Key);
    }



    public GameObject[] GetPanels
    {
        get { return panels;}
    }

    public GameObject GetCorrectAnswer()
    {
        numCorrectAnsw++;
        Debug.Log("numCorrect: " + numCorrectAnsw);
        return panels[correctAnswer];
    }

    public int NumCorrectAnsw
    {
        get { return numCorrectAnsw; }
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
