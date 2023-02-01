using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;

public class RoadManager : MonoBehaviour
{
    /// ha la lista di tag 
    /// Il compito princiaple � decidere cosa fa spwanare sulla strada: strada vuota, ostacoli e power up oppure domande 
    // Start is called before the first frame update
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    private float count= 0;
    protected ObstaclesPowerUp obstaclesPowerUp;
    [SerializeField] GameObject trackroad;
    protected RoadController roadController;
    private string VERTICAL_SPEED = "VERTICAL_SPEED";
    private string ACCELERATION = "ACCELERATION";
    protected float initialSpeed;
    protected float acceleration;
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    private List<GameObject> instantiatedTracks = new List<GameObject>();

    // Per calcolare i secondi nella velocità
    public float maxSpeed = 20;
    private float timer = 0f;
    
    //Variabili per la gestione degli ostacoli
    public List<GameObject> roadObjects = new List<GameObject>();


    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        obstaclesPowerUp = GetComponent<ObstaclesPowerUp>();
        roadController = trackroad.GetComponent<RoadController>();

        //ROADFEATURESPATH = Path.Combine(Application.streamingAssetsPath, ROADFEATURESPATH);
       // LoadParameters(ROADFEATURESPATH, roadFeatures);
    }
    void Start()
    {
        IstatiateRoad();
        LoadFeatures();
    }

    private  void IstatiateRoad()
    {
        Renderer renderer= roadController.getTrackRoad.GetComponent<Renderer>();
        if (renderer != null)
        {   //lunghezza z dell'oggetto
            float lenghtz = renderer.bounds.size.z;
            count = 0;
            for (int i =0; i<10; i++)
            {
                instantiatedTracks.Add(Instantiate(roadController.getTrackRoad, new Vector3(0,0,count), Quaternion.identity)) ;
                count += lenghtz;
            }
        }
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject g in instantiatedTracks)
        {
            g.transform.position += new Vector3(0, 0, -VerticalSpeed * Time.deltaTime);
        }
        //Ogni 10 secondi la velocità aumenta di un fattore pari ad Accelerazione
        IncreaseSpeedPerSeconds();
    }

    public void IncreaseSpeedPerSeconds()
    {
        timer += Time.deltaTime;
        float seconds = timer % 60;
        seconds = Mathf.CeilToInt(seconds);
        if((seconds%10) == 0 && VerticalSpeed < MaxSpeed){
            VerticalSpeed +=Acceleration;
            //Debug.Log("Seconds: "+ seconds + " speed: "+ VerticalSpeed);
        }
    }

    public float VerticalSpeed{
        get{ return initialSpeed;}
        set{initialSpeed = value;}
    }

    public float Acceleration
    {
        get{return acceleration;}
        set{acceleration = value;}
    }

    public float MaxSpeed
    {
        get{ return maxSpeed;}
        set{ maxSpeed = value; }
    }

    protected void LoadFeatures()
    {
        initialSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
        acceleration = featureManager.FeatureValue(ACCELERATION);
        //Debug.Log("VERTICAL_SPEED: "+ initialSpeed);
        //Debug.Log("ACCELERATION: "+ acceleration);
    }

    public void SpawnSegment(GameObject temp)
    {
        temp.transform.position += new Vector3(0,0,count);
        temp.GetComponent<RoadController>().SetRoad(true);
         //mi restituisce la posizione che ha nell'array se ad esempio
         // è poszione 5 inizio ad attivare gli ostacoli e dopo 20 pezzi di strada
         // disattivo gli ostacoli per 5 pezzi di strada metto le domande=IstatiateRoad[temp]; 
        int goPosition = instantiatedTracks.IndexOf(temp);
        if(goPosition > 3 && goPosition<=8)
        {
            Debug.Log("Position: "+ goPosition);
            ActivateObstacle(true, temp);
        }
        if (goPosition==9)
        {
            Debug.Log("Position: " + goPosition);
           
            ActivateQuestion(true, temp);   
        }
   

    }

    public void ActivateObstacle(bool check, GameObject go)
    {

        if (check)
        {
            int count = 0;
            List<Transform> listchild = new List<Transform>();
            foreach (Transform t in go.transform)
            {
                if (t.CompareTag("Ostacolo"))
                {
                    listchild.Add(t);
                    
                }
            }
            
            for (int i=0; i< listchild.Count; i++) 
            {

                int randomSpawn = UnityEngine.Random.Range(0, 2);




            }
            
            
        }


    }

    public void ActivateQuestion(bool check, GameObject go)
    {
       
        if (check) { 
            GameObject child = null;
            foreach (Transform t in go.transform)
            {
                if (t.CompareTag("PlaneFalse") || t.CompareTag("PlaneTrue"))
                {
                    child = t.gameObject;
                    child.SetActive(check);
                }

            }
        }
    }


/*
    protected void LoadParameters<T1, T2>(string path, Dictionary<T1, T2> p)
    {
        string[] lines = File.ReadAllLines(path);
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            object param1 = items[0].Trim();
            object param2 = items[1].Trim();
            if (typeof(T2) == typeof(float)) param2 = ParseFloatValue(items[1]);
            p.Add((T1)param1, (T2)param2);
        }
    }

    protected float ParseFloatValue(string val)
    {
        return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
    }
    
    protected void ComputeFeatures()
    {
        for(int i = 0; i < roadFeatures.Count; i++)
        {
            string s = roadFeatures.Keys.ElementAt(i);
            roadFeatures[s] = componentManager.FeatureValue(s);
            Debug.Log(" \n"+roadFeatures[s] +"\n");
        }
    }*/


}
