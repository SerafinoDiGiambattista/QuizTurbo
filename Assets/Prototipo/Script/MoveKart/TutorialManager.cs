using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] protected List<String> namefeatures;
    [SerializeField] protected string PathTutorial;
    protected bool Tutorial;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    protected List<Feature> features = new List<Feature>() ;
    protected WeightRandomManager weightRandomManager;
    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        PathTutorial = System.IO.Path.Combine(Application.streamingAssetsPath, PathTutorial);
        
        ReadValue();
    }

    private void ReadValue()
    {
        string[] lines = File.ReadAllLines(PathTutorial);
        foreach (string l in lines)
        {
            Tutorial = Convert.ToBoolean(l);
        }
        
    }
    void Start()
    {
        weightRandomManager = GetComponent<RoadManager>().GetWeightRandomManager;
        foreach (string name in namefeatures)
        {
            features.Add(featureManager.GetFeature(name));
        }
        //Questo serve per non far aumentare i pesi degli ostacoli durante il tutorial
        features.AddRange(weightRandomManager.GetFeaturesWeight.Values.ToList());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Tutorial) return;
        if (Tutorial)
        {
            foreach(Feature f in features)
            {
                
                f.CurrentValue = f.BaseValue;
                Dictionary<string, SComponent> components = componentManager.ComponentsByFeature(f.Type);
                foreach(string key in components.Keys)
                {
                    SpeedUp s = (SpeedUp)components[key];
                    s.resetTick();
                }
            }
        }
    }

    public bool GetTutorial
    {
        get { return Tutorial; }
        
    }

    public void FalseTutorial()
    {
        Tutorial = false;
        File.WriteAllText(PathTutorial, Convert.ToString(false));

    }
}
