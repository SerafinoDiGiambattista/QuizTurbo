
using System;
using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.UI.Image;
using Unity.MLAgents;

public class SlideRoad : MonoBehaviour
{
    // GroundSpawner groundSpawner;
    ManagerSpawner roadSpawner;
    public float modspeed= 10f;  
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject panelTrue;
    [SerializeField] GameObject panelFalse;
    [SerializeField] GameObject canvasDomanda;
    public TextMeshProUGUI textmeshPro;
    public string fileInput;
    private int count = 0;

    private void Start()
    {
        roadSpawner = GameObject.FindObjectOfType<ManagerSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        //chiedere a caturano perchè succede ??
        SetActivateCanvas(true);
                
            if (other.gameObject.CompareTag("Ciccio"))
            {   
                roadSpawner.SpawnSegment();
                Destroy(gameObject, 1);
                
            
                
                   // ReadCSV();
                    
             
                
            }
        
       
    }


    public void SpawnObstacle(Boolean spawOb)
    {
        float[] floatposition = {-3.0f, 0.0f, 3.0f };
        // Choose a random point to spawn the obstacle
        //int obstacleSpawnIndex = UnityEngine.Random.Range(1, 2);
        Transform spawnPoint = transform.Find("obstacleSpawn").transform;
        Vector3 originalPoisiton = spawnPoint.position;
        int randomSpawn = UnityEngine.Random.Range(1, 3);
      //  Debug.Log("valore casuale : "+randomSpawn);
        // Spawn the obstace at the position
        if (spawOb)
        {
            for (int i=0; i<randomSpawn; i++) {
                int randomObs = UnityEngine.Random.Range(0,3);
               // Debug.Log("Valore array : "+ floatposition[randomObs]);
                spawnPoint.Translate(floatposition[randomObs], 0, 0);
                Instantiate(obstaclePrefab, spawnPoint.position, obstaclePrefab.transform.rotation, transform);
                spawnPoint.position=originalPoisiton;
            }
        }
        else
        {
             SpawnPanel();
        }
    }

    public void SpawnPanel()
    {  
        

        Transform spawnTrue = transform.Find("True").transform;
        Instantiate(panelTrue, spawnTrue.position, panelTrue.transform.rotation, transform);

        Transform spawnFalse = transform.Find("False").transform;
        Instantiate(panelFalse, spawnFalse.position, panelFalse.transform.rotation, transform);
        
    }

    public void SetActivateCanvas(Boolean activate)
    {

       
        canvasDomanda.SetActive(activate);
        Debug.Log("Canvas : "+canvasDomanda);
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

        textmeshPro.SetText(domanda,true);
        Debug.Log("Valore di text : "+textmeshPro);

        //int risposta = tabellaRisposte[rand];
        //tabellaRisposte.Remove(rand);    

       // yield return new WaitForSeconds(0.5f);
        


    }

    private void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0, 0, -modspeed * Time.fixedDeltaTime);
    }



}
