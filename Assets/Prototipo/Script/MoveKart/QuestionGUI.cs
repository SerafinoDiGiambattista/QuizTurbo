using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionGUI : MonoBehaviour
{
    [SerializeField] protected GameObject[] panels;
    [SerializeField] protected GameObject canvasQuestion;
    protected TextMeshProUGUI textGUI;
    protected QuestionManager questionManager;
    private TextMeshProUGUI textmeshPro;

    private void Awake()
    {
        Instantiate(canvasQuestion);
    }

    public void SetTextQuestion(string question)
    {
        textmeshPro = canvasQuestion.GetComponent<TextMeshProUGUI>();
        //textmeshPro.SetText(question);
    }




}
