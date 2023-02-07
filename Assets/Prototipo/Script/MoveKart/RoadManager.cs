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
    private string MAX_SPEED = "MAX_SPEED";
    private string SOT_DURABILITY = "DURABILITY";
    protected float initialSpeed;
    private float verticalSpeed;
    protected float acceleration;
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    private List<GameObject> instantiatedTracks = new List<GameObject>();
    private float localSpace = 0f;

    // Per calcolare i secondi nella velocità
    public float maxSpeed = 0;
    private float timer = 0f;

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
        initialSpeed = verticalSpeed;
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

    protected void LoadFeatures()
    {
        verticalSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
        //acceleration = featureManager.FeatureValue(ACCELERATION);
        maxSpeed = featureManager.FeatureValue(MAX_SPEED);
        //Debug.Log("VERTICAL_SPEED: "+ initialSpeed);
        //Debug.Log("ACCELERATION: "+ acceleration);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject g in instantiatedTracks)
        {
            g.transform.position += new Vector3(0, 0, -verticalSpeed * Time.deltaTime);
        }
        Space += verticalSpeed * Time.deltaTime;
        //Ogni 10 secondi la velocità aumenta di un fattore pari ad Accelerazione
        SpeedOverTime(verticalSpeed);
        Debug.Log("Speed: "+verticalSpeed);
    }

    public float Space
    {
        set { localSpace = value; }
        get { return localSpace; }
    }

    /*public void IncreaseSpeedPerSeconds()
    {
        timer += Time.deltaTime;
        float seconds = timer % 60;
        seconds = Mathf.CeilToInt(seconds);
        if((seconds%10) == 0 && verticalSpeed < MaxSpeed){
            verticalSpeed += acceleration;
            //Debug.Log("Seconds: "+ seconds + " speed: "+ VerticalSpeed);
        }
    }*/

    public void SpeedOverTime(float currentSpeed)
    {
        VerticalSpeedOverTime(currentSpeed);
        //HorizontalSpeedOverTime();
    }
    
    public void VerticalSpeedOverTime(float currentSpeed)
    {
        Dictionary<string, Component> comp = componentManager.ComponentsByFeature(VERTICAL_SPEED);
        foreach(Component c in comp.Values)
        {
            try
            {
                float reduction = c.MyModifiers[ACCELERATION].AddFactor;
                c.IncreaseComponent(VERTICAL_SPEED, reduction);
            }
            catch (Exception) { }
        }
    }


    public float VerticalSpeed{
        get{ return verticalSpeed;}
        set{verticalSpeed = value;}
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

    public void SpawnSegment(GameObject temp)
    {
        temp.transform.position += new Vector3(0,0,count);
        temp.GetComponent<RoadController>().gameObject.SetActive(true);
         //mi restituisce la posizione che ha nell'array se ad esempio
         // è poszione 5 inizio ad attivare gli ostacoli e dopo 20 pezzi di strada
         // disattivo gli ostacoli per 5 pezzi di strada metto le domande=IstatiateRoad[temp]; 
        int goPosition = instantiatedTracks.IndexOf(temp);
        if(goPosition > 3 && goPosition<=8)
        {
          //  Debug.Log("Position: "+ goPosition);
            ActivateObstacle(true, temp);
        }
        if (goPosition==9)
        {
           // Debug.Log("Position: " + goPosition);
           
            ActivateQuestion(true, temp);   
        }
   

    }

    public void ActivateObstacle(bool check, GameObject go)
    {
        if (check)
        {
            List<GameObject> listchild = new List<GameObject>();
            foreach (Transform t in go.transform)
            {
                if (t.CompareTag("Ostacolo"))
                {
                    listchild.Add(t.gameObject);  
                }
            }
            int start = 0;
            int finish = 3; 
            for (int i=1; i<= listchild.Count; i++) 
            {
                if (i%3!=0) {
                    int random = UnityEngine.Random.Range(start, finish);
                    if (!listchild[random].activeSelf) listchild[random].SetActive(check);
                }
                else
                {
                    start = 3;
                    finish = listchild.Count;
                }
            }
        }
    }

    public void ActivateQuestion(bool check, GameObject go)
    {
       
        if (check) { 
         
            foreach (Transform t in go.transform)
            {
                if (t.CompareTag("PlaneFalse") || t.CompareTag("PlaneTrue"))
                {
                    t.gameObject.SetActiveRecursively(true);
                }
            }
        }
    }
    
   

}
