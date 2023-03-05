using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.ShaderData;

public class EffectTempManager : MonoBehaviour
{
    protected List<string> namefeature;
    protected string STORE = "STORE";
    protected string CHANGE = "CHANGE";
    protected FeatureManager featureManager;
    protected TickManager tickmanager;
    protected ComponentManager componentManager;
    protected Dictionary<string, TCComponent> featureprev = new Dictionary<string, TCComponent>();
    protected RoadManager roadManager;
    protected List<float> prev = new List<float> ();
    protected List<bool> pass = new List<bool>();
    protected bool reset;


    public void Awake()
    {   
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadManager = GetComponent<RoadManager>();
        reset = roadManager.GetMove;
    }

    public void Start()
    {
        namefeature = featureManager.Features.Select(k => k.Key).ToList();
        for (int i = 0; i < namefeature.Count; i++)
        {
            Dictionary<string, SComponent> list = componentManager.ComponentsByFeature(namefeature[i]);
            if (list != null)
            {
                foreach(string key in list.Keys)
                {
                    TCComponent s = (TCComponent)list[key];
                    if (s.CheckFeature(STORE))
                    {
                        Debug.Log(key);
                        featureprev.Add(namefeature[i], s);
                        prev.Add(0.0f);
                        pass.Add(false);
                        
                    }
                }
            }
        }


     

    }


    private void FixedUpdate()
    {   
        ScoreMult();
    }


    public void ScoreMult()
    {


        for (int i = 0; i < featureprev.Count; i++)
        {
            string features = featureprev.Keys.ElementAt(i);

            Dictionary<string, SComponent> list = componentManager.ComponentsByFeature(features);
            list = list.Where(x => x.Value.CheckFeature(CHANGE) == true).ToDictionary(pair => pair.Key, pair => pair.Value);
            if (list.Count < 1) { 
               
                if (!pass[i]) prev[i] = featureManager.FeatureValue(features);


                if (pass[i])
                {
                    Feature f1 = featureManager.Features[features];
                    f1.CurrentValue = prev[i];

                }

            }
            else
            {
                pass[i] = true;
            }
          



        }
        
     
    }

    public void ResetFeature()
    {
        foreach (string key in featureprev.Keys)
        {
            Feature f0 = featureManager.Features[key];
            f0.CurrentValue = f0.BaseValue;
            featureprev[key].resetTick();

        }
    }
}


