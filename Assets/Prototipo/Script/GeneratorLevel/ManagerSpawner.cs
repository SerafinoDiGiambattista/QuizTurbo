using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ManagerSpawner : MonoBehaviour
{
    [SerializeField] string fileInput;

    [SerializeField] GameObject road;
    [SerializeField] GameObject canvasQuestion;
    private TextMeshProUGUI textmeshPro;
    Vector3 nextSpawnPoint = new Vector3 (0,0,0);
    public float zpos = 30f;
    public int index = 5;
    private int count = 0;

    public void SpawnSegment()
    {
        count++;
        
      
        GameObject temp = Instantiate(road, nextSpawnPoint, Quaternion.identity);
        if (count == 3)
        {
            temp.GetComponent<SlideRoad>().SpawnObstacle(false);       

        }
        else if (count == 4)
        {
            //temp.GetComponent<SlideRoad>().SetActivateCanvas(false);

            count = 0;
        }
        else
         temp.GetComponent<SlideRoad>().SpawnObstacle(true);
        //temp.GetComponent<SlideRoad>().SpawnPanel();
    }

    public void SpwanQuestion(Boolean pass)
    {
        if (pass) {
            ReadCSV();
            canvasQuestion.SetActive(pass);
        }
        else
        {
            canvasQuestion.SetActive(pass);
        }
    }
     
    

    public float StartSpwan(float position, Boolean spawnItems)
    {
        position += zpos;
        nextSpawnPoint = new Vector3(0, 0, position);
        GameObject temp = Instantiate(road, nextSpawnPoint, Quaternion.identity);
        if (spawnItems) {
            //temp.GetComponent<SlideRoad>().SpawnObstacle(false);
            temp.GetComponent<SlideRoad>().SpawnObstacle(true);
            //temp.GetComponent<SlideRoad>().SpawnPanel();
        }
        return position;
    }

    private void ReadCSV()
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
            // Debug.Log(tabellaDomande[i]+ " risposta: "+ tabellaRisposte[i]);  
            i++;

        }

        int sizeTabella = i;
        //Debug.Log("i: " + i);

        int rand = UnityEngine.Random.Range(0, sizeTabella); // 0 è compreso, sizeTabella non è compreso
        //Debug.Log("random number: " + rand);
        while (!tabellaDomande.ContainsKey(rand))
            rand = UnityEngine.Random.Range(0, sizeTabella);

        string domanda = tabellaDomande[rand];
        tabellaDomande.Remove(rand);

        textmeshPro.SetText(domanda, true);
        //Debug.Log("Valore di text : "+textmeshPro);

        //int risposta = tabellaRisposte[rand];
        //tabellaRisposte.Remove(rand);    

        // yield return new WaitForSeconds(0.5f);



    }

    private void Start()
    {    
        canvasQuestion = Instantiate(canvasQuestion);
         textmeshPro = (TextMeshProUGUI)canvasQuestion.GetComponentInChildren(typeof(TextMeshProUGUI));
        float position = 0;
        //canvasQuestion.SetActive(true);
        //disattivate il primo trigger
        Instantiate(road, nextSpawnPoint, Quaternion.identity);
        
        
        for (int i=0; i<index; i++)
        {
            if (i < index/2)
            {
                position=StartSpwan(position,false);
            }
            else
            {
                position = StartSpwan(position,true);
              
            }
        }
        
        
    }

 


  
}

