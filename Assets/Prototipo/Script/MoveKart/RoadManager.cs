using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;
using System.Reflection;

public class RoadManager : MonoBehaviour
{ 
    [SerializeField] protected string TICKSPATH;
    [SerializeField] protected string SPAWN_PROBABILITY;
    [SerializeField] GameObject trackroad;
    [SerializeField] protected string VERTICAL_SPEED = "VERTICAL_SPEED";
    [SerializeField] protected string MAX_SPEED = "MAX_SPEED";
    [SerializeField] protected string HORIZONTAL_SPEED = "HORIZONTAL_SPEED";
    [SerializeField] protected string MAX_HORIZONTAL = "MAX_HORIZONTAL";
    [SerializeField] private string HEALTH = "HEALTH";
    [SerializeField] protected string BINARY_EASY;
    [SerializeField] protected string BINARY_HARD;
    [SerializeField] protected string NUM_OBSTACLE_TRACK = "NUM_OBSTACLE_TRACK";
    [SerializeField] protected string NUM_QUESTION_TRACK = "NUM_QUESTION_TRACK";
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    protected float count= 0;
    protected RoadController roadController;
    protected float initialVSpeed;
    protected float verticalSpeed;
    protected float initialHSpeed;
    private float initialHealth;
    private float health;
    protected float horizontalSpeed;
    protected float maxSpeed;
    protected float maxHSpeed;
    protected float numObstacleTrack = 0;
    protected float numQuestionTrack = 0;
    protected Dictionary<string, int> spawningProb = new Dictionary<string, int>();
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    protected List<GameObject> instantiatedObstaclesTracks = new List<GameObject>();
    protected float localSpace = 0f;
    protected TickManager tickmanager;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
    protected Dictionary<int, List<int> > binaryEasyDict = new Dictionary<int, List<int> >();


    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadController = trackroad.GetComponent<RoadController>();
        LoadParameters(TICKSPATH, tickables);
        ReadBinary(BINARY_EASY, binaryEasyDict);
        ReadSpawningProb(SPAWN_PROBABILITY, spawningProb);
    }
    
    void Start()
    {
        LoadFeatures();
        numObstacleTrack = Mathf.CeilToInt(numObstacleTrack);
        numQuestionTrack = Mathf.CeilToInt(numQuestionTrack);
        initialVSpeed = verticalSpeed;
        initialHSpeed= horizontalSpeed;
        initialHealth= health;
        IstatiateRoad();

        Debug.Log("1: "+ spawningProb["FENCE"]);
    }

    protected void LoadParameters<T1, T2>(string path, Dictionary<T1, T2> paramDict)
    {
        string[] lines = File.ReadAllLines(path);
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            object param1 = items[0].Trim();
            object param2 = items[1].Trim();
            if (typeof(T2) == typeof(float)) param2 = ParseFloatValue(items[1]);
            paramDict.Add((T1)param1, (T2)param2);
        }
    }

    protected void ReadBinary(string path, Dictionary<int, List<int> > valuesDict)
    {
        string[] lines = File.ReadAllLines(path);
        int i = 0;
        List<int> array = new List<int>();
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            int param1 = int.Parse(items[0].Trim());
            int param2 = int.Parse(items[1].Trim());
            int param3 = int.Parse(items[2].Trim());
            array.Add(param1);
            array.Add(param2);
            array.Add(param2);
            valuesDict.Add(i, array);
            i++;
        }
    }

    protected void ReadSpawningProb(string path, Dictionary<string, int> valuesDict)
    {
        string[] lines = File.ReadAllLines(path);
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            string param1 = items[0].Trim();
            int param2 = int.Parse(items[1].Trim());
            valuesDict.Add(param1, param2);
        }
    }

    private  void IstatiateRoad()
    {
        Renderer renderer= roadController.getTrackRoad.GetComponent<Renderer>();
        if (renderer != null)
        {   //lunghezza z dell'oggetto
            float lenghtz = renderer.bounds.size.z;
            count = 0;
            for (int i =0; i<numObstacleTrack; i++)
            {
                instantiatedObstaclesTracks.Add(Instantiate(roadController.getTrackRoad, new Vector3(0,0,count), Quaternion.identity)) ;
                count += lenghtz;
            }
        }
    }

    protected void LoadFeatures()
    {
        verticalSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
        maxSpeed = featureManager.FeatureValue(MAX_SPEED);
        horizontalSpeed = featureManager.FeatureValue(HORIZONTAL_SPEED);
        maxHSpeed = featureManager.FeatureValue(MAX_HORIZONTAL);
        health = featureManager.FeatureValue(HEALTH);
        numObstacleTrack = featureManager.FeatureValue(NUM_OBSTACLE_TRACK);
        numQuestionTrack = featureManager.FeatureValue(NUM_QUESTION_TRACK);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject g in instantiatedObstaclesTracks)
        {
            g.transform.position += new Vector3(0, 0, -verticalSpeed * Time.deltaTime);
        }
        Space += verticalSpeed * Time.deltaTime;
        DoAllTicks();
        //Debug.Log("Speed: "+verticalSpeed);
        //Debug.Log("Salute : "+health);
    }

    public void SpawnSegment(GameObject temp)
    {
        temp.transform.position += new Vector3(0,0,count);
        temp.GetComponent<RoadController>().gameObject.SetActive(true);
        ActivateObstacle(temp);
        int goPosition = instantiatedObstaclesTracks.IndexOf(temp);
    }

    public void ActivateObstacle(GameObject go)
    {
        List<GameObject> listchild = new List<GameObject>();
        int randomObs0;
        int randomObs1;
        int randomObs2;
        foreach (Transform t in go.transform)
        {
            if (t.CompareTag("Ostacolo"))
            {
                listchild.Add(t.gameObject);  
            }
        }
 
        int randomRow = UnityEngine.Random.Range(0,  binaryEasyDict.Count);
        List<int> binaryRow = binaryEasyDict[randomRow];
        //1) prendo la riga di bit 
        //2) prendo l'ostacolo in base alla probability
        //3) spawnare l'oggetto nel punto 1 della riga di bit (sempre in base ai punti di spawn)

        randomObs0 = UnityEngine.Random.Range(0, listchild.Count);
        //Debug.Log("count: "+listchild.Count);
        if(randomObs0+1 == listchild.Count) randomObs1 = 0;
           else randomObs1 = randomObs0+1;
        if(randomObs1+1 == listchild.Count) randomObs2 = 0;
            else randomObs2 = randomObs1+1;
        
        /*if(!listchild[randomObs0].activeSelf && binaryRow[0] != 0) CheckProbability();
        if(!listchild[randomObs1].activeSelf && binaryRow[1] != 0) CheckProbability();
        if(!listchild[randomObs2].activeSelf && binaryRow[2] != 0) CheckProbability();*/
    }

    /*public void ChooseByProbability()
    {
        float total = 0;

        foreach (float elem in probs) {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i= 0; i < probs.Length; i++) {
            if (randomPoint < probs[i]) {
                return i;
            }
            else {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ostacolo"))
        {
            string path = other.gameObject.GetComponent<PathManager>().Path;
            string[] n = path.Split('.');
            string name = Path.GetFileName(n[0]);
            componentManager.ComponentPickup(name, path);
            //Debug.Log("name: "+name+ " path: "+path);

            // Debug.Log("Health: "+health);
        }
    }
    protected void DoAllTicks()
    {
        foreach (KeyValuePair<string, string> t in tickables)
        {
            ComputeByComponent(t.Key, t.Value);
        }
    }

    public Dictionary<string, float> GetAllTicks(string type)
    {   
        return componentManager.GetAllTicks(type);
    }

    public void ComputeByComponent(string type, string func)
    {
        Dictionary<string, float> filtered = GetAllTicks(type);
        //Debug.Log("Filetered : "+filtered.Count);
        float amount = ComputeFeatureValue(filtered);
        //Debug.Log("Amount : " + amount);
        if (amount > 0)
        {
            try
            {
            object[] p = { amount };
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(func);
            theMethod.Invoke(this, p);
            } catch{}
        }
    }

    public float ComputeFeatureValue(Dictionary<string, float> received)
    {
        float res = 0;
        foreach (string s in received.Keys)
        {
            try
            {
                //Debug.Log("s: " + s + " received: " + received[s]);
                res += received[s];
            }
            catch (Exception) { }
        }
        return res;
    }

    public void SpeedUp(float acc)
    {
       // Debug.Log("ACC : " + acc);
         verticalSpeed += acc;
         horizontalSpeed+= acc/2;
         if (verticalSpeed > maxSpeed) verticalSpeed= maxSpeed;
         if(horizontalSpeed > maxHSpeed) horizontalSpeed= maxHSpeed;
    }

    public void DamageDone(float dmg)
    {
      //  Debug.Log("Damage : "+dmg);
        if (health > 0) health -= dmg;
    }
    
    public void HealMe(float h)
    {
        Debug.Log("salute : "+health);
        if(health<initialHealth) health += h;
       // if(health == 0) Debug.Log("YOU ARE DEAD! :(");   
    }
protected float ParseFloatValue(string val)
    {
        return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
    }

    public float Space
    {
            set { localSpace = value; }
            get { return localSpace; }
    }

    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }


    public float VerticalSpeed{
        get{ return verticalSpeed;}
        set{verticalSpeed = value;}
    }

 
    public float MaxSpeed
    {
        get{ return maxSpeed;}
        set{ maxSpeed = value; }
    }
    public float GetInitialHealth
    {
        get { return featureManager.FeatureValue(HEALTH); }
    }

    public float GetHealth
    {   
        get { return health; }

        
    }

}
