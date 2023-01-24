
using System;
using UnityEngine;

public class SlideRoad : MonoBehaviour
{
    // GroundSpawner groundSpawner;
    ManagerSpawner roadSpawner;
    public float modspeed= 10f;  
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject panelTrue;
    [SerializeField] GameObject panelFalse;
    public string fileInput;
    private Collider checkpoint;


    private void Start()
    {
        roadSpawner = GameObject.FindObjectOfType<ManagerSpawner>();
       

    }

    private void OnTriggerExit(Collider other)
    {
        //chiedere a caturano perchè succede ??

            if (other.gameObject.CompareTag("Ciccio"))
            {   
                roadSpawner.SpawnSegment();
                //roadSpawner.SpwanQuestion(false);
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

        checkpoint =  transform.Find("checkpoint").GetComponent<Collider>();
        checkpoint.enabled=true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.Find("checkpoint").GetComponent<Collider>().enabled == true)
        {
            if (other.gameObject.CompareTag("Ciccio"))
            {
              
                Debug.Log("Checkpoint ");
                roadSpawner.SpwanQuestion(true);
                transform.Find("checkpoint").GetComponent<Collider>().enabled = false;

            }
            
        }
    }




    private void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0, 0, -modspeed * Time.fixedDeltaTime);
    }



}
