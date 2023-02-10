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
    [SerializeField] GameObject trackroad;
    [SerializeField] protected string VERTICAL_SPEED = "VERTICAL_SPEED";
    [SerializeField] protected string MAX_SPEED = "MAX_SPEED";
    [SerializeField] protected string HORIZONTAL_SPEED = "HORIZONTAL_SPEED";
    [SerializeField] protected string MAX_HORIZONTAL = "MAX_HORIZONTAL";
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    protected float count= 0;
    protected RoadController roadController;
    protected float initialVSpeed;
    protected float verticalSpeed;
    protected float initialHSpeed;
    protected float horizontalSpeed;
    protected float maxSpeed;
    protected float maxHSpeed;
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    protected List<GameObject> instantiatedTracks = new List<GameObject>();
    protected float localSpace = 0f;
    protected TickManager tickmanager;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
   
    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadController = trackroad.GetComponent<RoadController>();
        LoadParameters(TICKSPATH, tickables);
    }
    void Start()
    {
        IstatiateRoad();
        LoadFeatures();
        initialVSpeed = verticalSpeed;
        initialHSpeed= horizontalSpeed;
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
        maxSpeed = featureManager.FeatureValue(MAX_SPEED);
        horizontalSpeed = featureManager.FeatureValue(HORIZONTAL_SPEED);
        maxHSpeed = featureManager.FeatureValue(MAX_HORIZONTAL);

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
      
        foreach (GameObject g in instantiatedTracks)
        {
            g.transform.position += new Vector3(0, 0, -verticalSpeed * Time.deltaTime);
        }
        Space += verticalSpeed * Time.deltaTime;
      
        DoAllTicks();
        Debug.Log("Speed: "+verticalSpeed);
    }

    
    public void SpawnSegment(GameObject temp)
    {
        temp.transform.position += new Vector3(0,0,count);
        temp.GetComponent<RoadController>().gameObject.SetActive(true);

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
        //Debug.Log("Amount : "+amount);
        if (amount > 0)
        {
            object[] p = { amount };
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(func);
            theMethod.Invoke(this, p);
        }
    }

    public float ComputeFeatureValue(Dictionary<string, float> received)
    {
        float res = 0;
        foreach (string s in received.Keys)
        {
            try
            {
              
                res += received[s];
            }
            catch (Exception) { }
        }
        return res;
    }

    public void SpeedUp(float acc)
    {
         verticalSpeed += acc;
         horizontalSpeed+= acc/2;
         if (verticalSpeed > maxSpeed) verticalSpeed= maxSpeed;
         if(horizontalSpeed > maxHSpeed) horizontalSpeed= maxHSpeed;
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

}
