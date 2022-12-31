using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;


public class CSVReader : MonoBehaviour
{
    public TextMeshProUGUI  textmeshPro;
    public string fileInput;

    public void Start()
    {
            ReadCSV();
    }
    
    public void ReadCSV()
    {
        StreamReader strReader = new StreamReader(fileInput);

        Dictionary<int, string> tabellaDomande = new Dictionary<int, string>();
        Dictionary<int, int> tabellaRisposte = new Dictionary<int, int>();

        bool endOdFile = false;
        int i = 0;
        
        while (!endOdFile)
        {
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOdFile = true;
                break;
            }
            string[] row = data_String.Split(';');
            tabellaDomande.Add(i, row[0]);
            tabellaRisposte.Add(i, int.Parse(row[1]));
            //Debug.Log(tabellaDomande[i]+ " risposta: "+ tabellaRisposte[i]);  
            i++;
             
        }

        int sizeTabella = i;
        Debug.Log("i: " + i);

        int rand = UnityEngine.Random.Range(0, sizeTabella); // 0 è compreso, sizeTabella non è compreso
        //Debug.Log("random number: " + rand);
        while(!tabellaDomande.ContainsKey(rand))
            rand = UnityEngine.Random.Range(0, sizeTabella);

        string domanda = tabellaDomande[rand];
        tabellaDomande.Remove(rand);

        textmeshPro.SetText(domanda);
        Debug.Log(domanda);

        //int risposta = tabellaRisposte[rand];
        //tabellaRisposte.Remove(rand);    

    }
}
