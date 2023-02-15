using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;
using System.Reflection;
using System.IO.Abstractions.TestingHelpers;

public class RoadManager : MonoBehaviour
{ 
    [SerializeField] protected string TICKSPATH;
    [SerializeField] GameObject trackroad;
    [SerializeField] GameObject weightedObject;
    [SerializeField] GameObject questionObject;
    [SerializeField] protected string VERTICAL_SPEED = "VERTICAL_SPEED";
    [SerializeField] protected string MAX_SPEED = "MAX_SPEED";
    [SerializeField] protected string HORIZONTAL_SPEED = "HORIZONTAL_SPEED";
    [SerializeField] protected string MAX_HORIZONTAL = "MAX_HORIZONTAL";
    [SerializeField] protected string HEALTH = "HEALTH";
    //[SerializeField] protected string MAX_HEALTH = "MAX_HEALTH";
    [SerializeField] protected string BINARY_EASY;
    [SerializeField] protected string BINARY_MEDIUM;
    [SerializeField] protected string BINARY_HARD;
    [SerializeField] protected string NUM_OBSTACLE_TRACK = "NUM_OBSTACLE_TRACK";
    [SerializeField] protected string NUM_QUESTION_TRACK = "NUM_QUESTION_TRACK";
    [SerializeField] protected string EASY_OBS_DIFFICULTY = "EASY_OBS_DIFFICULTY";
    [SerializeField] protected string HARD_OBS_DIFFICULTY = "HARD_OBS_DIFFICULTY";
    [SerializeField] protected string MEDIUM_OBS_DIFFICULTY = "MEDIUM_OBS_DIFFICULTY";
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    protected WeightRandomManager weightRandomManager;
    protected RoadController roadController;
    protected QuestionManager questionManager;
    protected float initialVSpeed;
    protected float verticalSpeed;
    protected float initialHSpeed;
    private float initialHealth;
    //private float maxhealth;
    private float health;
    protected float horizontalSpeed;
    protected float maxSpeed;
    protected float maxHSpeed;
    protected float count = 0;
    protected float numObstacleTrack = 0;
    protected float numQuestionTrack = 0;
    protected float easyObsDifficulty = 0;
    protected float hardObsDifficulty = 0;
    protected float mediumObsDifficulty = 0;
    protected bool easy = true;
    protected bool medium = false;
    protected bool hard = false;
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    protected List<GameObject> instantiatedTracks = new List<GameObject>();
    protected float localSpace = 0f;
    protected TickManager tickmanager;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
    protected Dictionary<int, List<int> > binaryEasyDict = new Dictionary<int, List<int> >();
    protected Dictionary<int, List<int>> binaryMediumDict = new Dictionary<int, List<int>>();
    protected Dictionary<int, List<int>> binaryHardDict = new Dictionary<int, List<int>>();


    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadController = trackroad.GetComponent<RoadController>();
        weightRandomManager = weightedObject.GetComponent<WeightRandomManager>();
        questionManager = questionObject.GetComponent<QuestionManager>();
        LoadParameters(TICKSPATH, tickables);
        ReadBinary(BINARY_EASY, binaryEasyDict);
        ReadBinary(BINARY_MEDIUM, binaryMediumDict);
        ReadBinary(BINARY_HARD, binaryHardDict);
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
       
        foreach (string l in lines)
        {   List<int> array = new List<int>();
            string[] items = l.Split(',');
            foreach(string item in items)
            {
                int param = int.Parse(item.Trim());
                array.Add(param);
                
            }
            valuesDict.Add(i, array);
            i++;
        }
    }

    private  void IstatiateRoad()
    {
        Transform[] position;
        Renderer renderer= roadController.getTrackRoad.GetComponent<Renderer>();
        if (renderer != null)
        {   //lunghezza z dell'oggetto
            float lenghtz = renderer.bounds.size.z;
            count = 0;
            for (int i = 0; i < numObstacleTrack; i++)
            {
                instantiatedTracks.Add(Instantiate(roadController.getTrackRoad, new Vector3(0, 0, count), Quaternion.identity));
                position = instantiatedTracks[i].GetComponentsInChildren<Transform>();
                position = position.Skip(1).ToArray();
                var positionsObs = position.Where(x => x.gameObject.tag.Equals("Spawn")).ToArray();
                InstatiateObject(positionsObs);

                Transform positionTrue = position.Where(x => x.gameObject.tag.Equals("spawnTrue")).SingleOrDefault();
                Transform positionFalse = position.Where(x => x.gameObject.tag.Equals("spawnFalse")).SingleOrDefault();
                InstantiateTF( positionTrue, positionFalse);

                count += lenghtz;
            }
        }
    }

    private void InstantiateTF(Transform t, Transform f)
    {
        questionManager.GetPanelTrue.SetActive(false);
        questionManager.GetPanelFalse.SetActive(false);
        Instantiate(questionManager.GetPanelTrue, t.position, questionManager.GetPanelTrue.transform.rotation, t);
        Instantiate(questionManager.GetPanelFalse, f.position, questionManager.GetPanelFalse.transform.rotation, f);
    }

    private void InstatiateObject(Transform[] position)
    {
        foreach (Transform index in position)
        {
            foreach (GameObject g in weightRandomManager.GetObjectSpwan)
            {
                g.SetActive(false);
                Instantiate(g, index.position, g.transform.rotation, index);
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
        easyObsDifficulty = featureManager.FeatureValue(EASY_OBS_DIFFICULTY);
        mediumObsDifficulty = featureManager.FeatureValue(MEDIUM_OBS_DIFFICULTY);
        hardObsDifficulty = featureManager.FeatureValue(HARD_OBS_DIFFICULTY);
       // maxhealth = featureManager.FeatureValue(MAX_HEALTH);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject g in instantiatedTracks)
        {
            g.transform.position += new Vector3(0, 0, -verticalSpeed * Time.deltaTime);
        }
        Space += verticalSpeed * Time.deltaTime;
        if (Space > mediumObsDifficulty && Space < hardObsDifficulty) { easy = false; medium = true; }
        if(Space >= hardObsDifficulty) { medium = false; hard = true; }
        DoAllTicks();
        //Debug.Log("Speed: "+verticalSpeed);
        //Debug.Log("Salute : "+health);
    }


    private int numOfSegments = 0;
    public void SpawnSegment(GameObject temp)
    {
        numOfSegments++;
        temp.transform.position += new Vector3(0,0,count);
        temp.GetComponent<RoadController>().gameObject.SetActive(true);

        if (numOfSegments <= instantiatedTracks.Count)
        {
            if (easy) ActivateObejct(temp, binaryEasyDict);
            if (medium)
            {
                ActivateObejct(temp, binaryMediumDict);
            }
            else if(binaryMediumDict==null)
            {
                ActivateObejct(temp, binaryEasyDict);
            }
            if (hard)
            {
                ActivateObejct(temp, binaryHardDict);
            }
            else if (binaryHardDict == null)
            {
                ActivateObejct(temp, binaryEasyDict);
            }
        }
        else if (numOfSegments <= (instantiatedTracks.Count + numQuestionTrack))
        {
            ActivateQuestion(temp);
        }
        else numOfSegments = 0;
        //int goPosition = instantiatedTracks.IndexOf(temp);
    }

    public void ActivateQuestion(GameObject trackroad)
    {
                
    }

    public void ActivateObejct(GameObject grandfather, Dictionary<int, List<int>> binaryFile)
    {
        int randomRow = UnityEngine.Random.Range(0, binaryFile.Count);
        List<int> binaryRow = binaryFile[randomRow];
        int index = 0;
        Transform[] father = grandfather.GetComponentsInChildren<Transform>();
        father = father.Skip(1).ToArray();
        foreach (int i in binaryRow)
        {
            if (i != 0)
            {
                //Debug.Log("intero : "+i);
                string name = weightRandomManager.ChooseByProbability();
                for (int j = 0; j < father[index].childCount; j++)
                {   
                    GameObject child = father[index].GetChild(j).gameObject;
                    string namechild = child.name.ToUpper().Replace("(CLONE)","");
                    //  Debug.Log("nome child : "+namechild);
                    if ((!child.activeSelf) && namechild.Equals(name) )
                    {
                      father[index].GetChild(j).gameObject.SetActive(true);
                    }
                }
            }
            index++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        //il nome ostacolo è hard code dobbiamo metterlo nell'ispettore 
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
        //Debug.Log("salute : "+health);
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

    public bool GetEasy
    {
        get { return easy; }
    }

    public bool GetMedium
    {
        get { return medium; }
    }

    public bool GetHard
    {
        get { return hard; }
    }
}
