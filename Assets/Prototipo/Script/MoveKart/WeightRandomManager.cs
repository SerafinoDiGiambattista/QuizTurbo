using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;
using System.Reflection;

public class WeightRandomManager : MonoBehaviour
{
    [SerializeField] protected string TICKSPATH;    // Per modificare la probabilità nel temp
    [SerializeField] protected List<GameObject> objectsToSpawn;
    protected FeatureManager featureManager;
    protected TickManager tickmanager;
    protected ComponentManager componentManager;
    private Dictionary<string,Feature> featuresRead = new Dictionary<string,Feature>();
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
    private Dictionary<string, float> initialFeatures = new Dictionary<string, float>();
    private Dictionary<string, float> features = new Dictionary<string, float>();


    void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        LoadParameters(TICKSPATH, tickables);
    }

    private void Start()
    {
        LoadFeatures();
        initialFeatures = features;
        //string s = ChooseByProbability();
        //Debug.Log("str: " + s);
    }

    private void FixedUpdate()
    {
        DoAllTicks();
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
        //Debug.Log("Filetered : " + filtered.Count);
        if (filtered.Count > 0)
        {
            try
            {
                object[] p = { filtered };
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(func);
                theMethod.Invoke(this, p);
            }
            catch { }
        }
    }

    public void ModifyWeight(Dictionary<string, float> filter)
    {
        features = features.Concat(filter) 
                   .GroupBy(x => x.Key) 
                   .ToDictionary(x => x.Key, x => x.Sum(y => y.Value)); 

        /*foreach (string str in features.Keys)
        {
            Debug.Log("Key: " + str + " Val: " + features[str]);
        }*/
    }

    protected void LoadFeatures()
    {
        featuresRead = featureManager.Features;
        foreach(string s in featuresRead.Keys)
        {
            features.Add(s, featuresRead[s].CurrentValue);
        }
    }

    public String ChooseByProbability()
    {
        float total = 0;
        //GameObject go;
        foreach (float elem in features.Values) {
            total += elem;
        }

        float randomWeight = UnityEngine.Random.Range(0, total);
        //Debug.Log("Valore proba tot : "+total);
        foreach(string s in features.Keys)
        {
            float i = features[s];
            if (randomWeight < i)
                return s;
            randomWeight -= i;
        }
        return null;
    }

    protected float ParseFloatValue(string val)
    {
        return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
    }


    public List<GameObject> GetObjectSpwan
    {
        get { return objectsToSpawn; }
    }
}

