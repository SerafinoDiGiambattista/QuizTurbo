using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionGUI : MonoBehaviour
{
    [SerializeField] protected GameObject canvasQuestion;
    protected QuestionManager questionManager;
    protected TextMeshProUGUI textmeshPro;

    private void Awake()
    {
        //canvasQuestion.SetActive(false);
        canvasQuestion = Instantiate(canvasQuestion);
        canvasQuestion.SetActive(false);
        textmeshPro = canvasQuestion.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTextQuestion(string question)
    {
        textmeshPro.SetText(question);
    }

    public GameObject CanvasQuestion
    {
        get { return canvasQuestion;  }
    }
}
