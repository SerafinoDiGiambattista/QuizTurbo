using System.Collections;
using System;
using Unity.IO;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{   [SerializeField] protected string componentDirectory;
    protected Dictionary<string, SComponent> components = new Dictionary<string, SComponent>();
    protected Dictionary<string, Feature> objFeatures = new Dictionary<string, Feature>();
    protected FeatureManager featureManager;
    protected Dictionary<string, float> featureMulMod = new Dictionary<string, float>();
    protected Dictionary<string, float> featureAddMod = new Dictionary<string, float>();
    protected CountDownManager countDownManager;
    protected Dictionary<string, Modifier> allTicks = new Dictionary<string, Modifier>();
    protected TickManager tickManager;

    private void Awake()
    {
        componentDirectory = Path.Combine(Application.streamingAssetsPath, componentDirectory);
        featureManager = GetComponent<FeatureManager>();
        countDownManager = GetComponent<CountDownManager>();
        tickManager = GetComponent<TickManager>();
        LoadComponentGroup(componentDirectory);
    }

    private void FixedUpdate()
    {
        if (objFeatures == null) return;
        CheckIsActive();
        ResetModifiers();
        ComputeModifiers();
        ComputeFeatures();
        ComputeAllTicks();
        //Print();
    }
        
    /*public void ComponentPickup(string type, string name, string path)
    {
        path = Path.Combine(Application.streamingAssetsPath, path);
        type = type.ToLower();
        type = char.ToUpper(type[0]) + type.Substring(1);
        AddComponent((UAComponent)Activator.CreateInstance( name, path, this));
    }*/

    /*public void Print()
    {
        foreach(KeyValuePair<string, Component> keyValuePair in components)
        {
            Component c = components[keyValuePair.Key];
            Debug.LogError(c.NameC);
            string features = "";
            foreach (KeyValuePair<string, Feature> f in c.MyFeatures) features += c.MyFeatures[f.Key].Type + " BV: " + c.MyFeatures[f.Key].BaseValue + " CV: " + c.MyFeatures[f.Key].CurrentValue;
            //string modifiers = "";
            //foreach (KeyValuePair<string, Modifier> m in c.MyModifiers) modifiers += c.MyModifiers[m.Key].Type + " MF: " + c.MyModifiers[m.Key].MultFactor + " AF: " + c.MyModifiers[m.Key].AddFactor;
            Debug.Log("Features: " + features);
            //Debug.Log("Modifiers: " + modifiers);
        }
    }*/

    public TickManager GetTickManager
    {
        get { return tickManager; }
    }

     public CountDownManager GetCountDownManager
    {
        get { return countDownManager; }
    }

    public Dictionary<string, Feature> GetObjFeatures
    {
        get{ return objFeatures;}
        set{objFeatures = value;}
    }

    public Dictionary<string, SComponent> Components
    {
        get{ return components;}
    }

    protected void LoadComponentGroup(string path)
    {
        if (!CheckDirectory(path)) return;
        string[] fileEntries = Directory.GetFiles(path, "*.csv");
        foreach (string fileName in fileEntries)
        {
            LoadSingleComponent(fileName);
        }
    }
    protected void LoadSingleComponent(string path)
    {
        if (!CheckFile(path)) return;
        string[] n = path.Split('.');

        // SComponent c = new SComponent();
        SComponent c = new SpeedUp(Path.GetFileName(n[0].Trim()), path, this);


        AddComponent(c);
    }

    protected bool CheckFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File Not Found");
            return false;
        }
        return true;
    }
    protected bool CheckDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("Directory Not Found");
            return false;
        }
        return true;
    }


    // serve a resettare i modificaotri dopo che l�hai utilizzati
    protected void ResetModifiers()
    {
        foreach(Feature f in objFeatures.Values)
        {
            featureMulMod[f.Type] = 1.0f;
            featureAddMod[f.Type] = 0.0f;
        }
    }
    //valore della feature
    public float FeatureValue(string f)
    {
        float val = 0;
        val = objFeatures[f].CurrentValue;
        return val;
    }



    //calcolo modificatore 
    public void ComputeModifiers()
    {
        foreach (KeyValuePair<string, SComponent> kv in components)
        {
            Dictionary<string, Modifier> mod = components[kv.Key].MyModifiers;
            foreach (Modifier m in mod.Values)
            {
                try
                {
                    featureMulMod[m.Type] *= m.MultFactor;
                    featureAddMod[m.Type] += m.AddFactor;
                }
                catch (Exception) { }
            }
        }
    }

    //calcolo valore totale della feature con i modificatori 
    public void ComputeFeatures()
    {
        foreach(Feature f in objFeatures.Values)
        {
            float midVal = f.BaseValue * featureMulMod[f.Type];
            f.CurrentValue = midVal + featureAddMod[f.Type];
            //Debug.Log("f.CurrentValue: "+f.CurrentValue);
        }
    }

    public Dictionary<string, SComponent> ComponentsByFeature(string feature)
    {
        return components.Where(x => x.Value.HasFeature(feature)).ToDictionary(x => x.Key, x=> x.Value);
    }

    public void RemoveComponent(string c)
    {
        components.Remove(c);
    }

    public void AddComponent(SComponent c)
    {
        if (components.ContainsKey(c.NameC)) RemoveComponent(c.NameC);
        components.Add(c.NameC, c);
    }

    protected void CheckIsActive()
    {
        for(int i = 0; i <= components.Count; i++)
        {
            try
            {
                string s = components.ElementAt(i).Key;
                if (!components[s].CheckIsActive()) RemoveComponent(s);
            }
            catch (Exception) { }
        }
    }

    protected void ComputeAllTicks()
    {
        foreach (SComponent c in components.Values)
        {
           // Debug.Log("Valore di check "+c.CheckTick());
            if (c.CheckTick())
            {  
                foreach (Modifier m in c.MyModifiers.Values)
                {
                    if (allTicks.ContainsKey(m.GetName)) allTicks.Remove(m.GetName);
                    allTicks.Add(m.GetName, m);
                   // Debug.Log("all tick: " + m.GetName + "  featureType: " + m.AddFactor);
                }
                c.ResetTick();
            }
        }
    }


    public Dictionary<string, float> GetAllTicks(string type)
    {
        Dictionary<string, float> filtered = allTicks.Where(x => x.Value.Type == type).ToDictionary(x => x.Key, x => x.Value.AddFactor);

        foreach (string k in filtered.Keys)
        {
          
            allTicks.Remove(k);
        }


        return filtered;
    }

    public Dictionary<string, T> FilterByType<T>()
    {
        Dictionary<string, T> filtered = new Dictionary<string, T>();
        foreach (KeyValuePair<string, SComponent> c in components)
        {
            if (c.Value is T t) filtered.Add(c.Key, t);
        }
        return filtered;
    }
    public void SpeedUpPickup(string name, string path)
    {
        path = Path.Combine(Application.streamingAssetsPath, path);
        AddComponent(new SpeedUp(name, path, this));
    }

}


