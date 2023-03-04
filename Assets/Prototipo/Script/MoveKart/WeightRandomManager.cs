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
    private Dictionary<string, float> weightFeatures = new Dictionary<string, float>();


    void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        LoadFeatures();
    }

    private void FixedUpdate()
    {
        UpdateFeatures();
    }

    protected void LoadFeatures()
    {
        featuresRead = featureManager.Features;
        foreach(string s in featuresRead.Keys)
        {
            weightFeatures.Add(s, featuresRead[s].CurrentValue);
            //Debug.Log("KEY "+s+ " value : "+ featuresRead[s].CurrentValue);
        }
    }
    protected void UpdateFeatures()
    {
        weightFeatures = weightFeatures.ToDictionary(x => x.Key, x=>featureManager.FeatureValue(x.Key));
    }

    public String ChooseByProbability()
    {
       
        float total = 0;
        //GameObject go;
        foreach (float elem in weightFeatures.Values) {
            total += elem;
        }

        float randomWeight = UnityEngine.Random.Range(0, total);
        Debug.Log("Valore proba tot : "+total);
        foreach(string s in weightFeatures.Keys)
        {
            float i = weightFeatures[s];
            if (randomWeight < i)
                return s;
            randomWeight -= i;
        }
        return null;
    }


    public List<GameObject> GetObjectSpwan
    {
        get { return objectsToSpawn; }
    }

    public Dictionary<string, Feature> GetFeaturesWeight
    {
        get { return featuresRead; }
    }
}

