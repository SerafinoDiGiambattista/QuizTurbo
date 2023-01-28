using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;

public class RoadManager : MonoBehaviour
{

    /// ha la lista di tag 
    /// Il compito princiaple � decidere cosa fa spwanare sulla strada: strada vuota, ostacoli e power up oppure domande 
    // Start is called before the first frame update
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    [SerializeField] protected List<string> collidersTags;
    protected ObstaclesPowerUp obstaclesPowerUp;
    [SerializeField] GameObject track_road;
    [SerializeField] protected string VERTICAL_SPEED = "VERTICAL_SPEED";
    protected float initialSpeed;
    [SerializeField] protected string ROADFEATURES;
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    //[SyncVar] protected bool isDead = false;

    //protected QuestionManager questionManager;

    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        obstaclesPowerUp = GetComponent<ObstaclesPowerUp>();
        ROADFEATURES = Path.Combine(Application.streamingAssetsPath, ROADFEATURES);
        LoadParameters(ROADFEATURES, roadFeatures);
    }
    void Start()
    {
        IstatiateRoad();
    }

    private  void IstatiateRoad()
    {
        Renderer renderer= track_road.GetComponent<Renderer>();
        if (renderer != null)
        {   //lunghezza z dell'oggetto
            float lenghtz = renderer.bounds.size.z;
           
            float count = 0;
            for (int i =0; i<10; i++)
            {
                Instantiate(track_road, new Vector3(0,0,count), Quaternion.identity) ;
                count += lenghtz;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LoadFeatures();
    }

    public float VerticalSpeed{
        get{ return initialSpeed;}
        set{initialSpeed = value;}
    }

    protected void LoadFeatures()
    {
        initialSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
        Debug.Log("VERTICAL_SPEED: "+ initialSpeed);
    }

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
}
