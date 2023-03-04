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
        //ComputeFeatures();
        ComputeAllTicks();
        //Print();
    }

    public void ComponentPickup( string name, string path)
    {
            path = Path.Combine(Application.streamingAssetsPath, path);
             /*
            Type t = Type.GetType("SpeedUp");
            AddComponent((SpeedUp)Activator.CreateInstance(t, name, path, this));*/
            AddComponent(new SpeedUp(name, path, this));
    }


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
                    //if(m.Type.Equals("CURVATURE")) Debug.Log("add: " + m.AddFactor);
                    featureMulMod[m.Type] *= m.MultFactor;
                    featureAddMod[m.Type] += m.AddFactor;
                }
                catch (Exception) { }
            }
        }
    }

    public void SetAddModifierByFeature(string type, float num)
    {
        featureAddMod[type] *= num;
    }

    //calcolo valore totale della feature con i modificatori 
  /*  public void ComputeFeatures()
    {
        foreach(Feature f in objFeatures.Values)
        {
            float midVal = f.BaseValue * featureMulMod[f.Type];
            f.CurrentValue = midVal + featureAddMod[f.Type];
           //if(f.Type.Equals("VERTICAL_SPEED")) Debug.Log("Feature current : "+f.Type +" "+ midVal);
           
        }
    }*/

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
                   // Debug.Log("CHIAVE ALLTICK : "+m.GetName);
                    foreach (Feature f in objFeatures.Values)
                    {
                        if (f.Type.Equals(m.Type))
                        {
                            float midVal = f.CurrentValue * featureMulMod[f.Type];
                            f.CurrentValue = midVal + featureAddMod[f.Type];
                            
          
                        }   
                    }
                }
               
              /*  foreach (Feature f in objFeatures.Values)
                {
                        float midVal = f.CurrentValue * featureMulMod[f.Type];
                        f.CurrentValue = midVal + featureAddMod[f.Type];
                        //if(f.Type.Equals("HEALTH")) Debug.Log("Feature current : "+f.CurrentValue +" "+ midVal);
                }*/
                c.ResetTick();
            }
        }
    }

    public Dictionary<string, float> GetAllTicks(string type)
    {
        Dictionary<string, float> filtered = allTicks.Where(x => x.Key.Equals(type)).ToDictionary(x => x.Key, x => x.Value.MultFactor);
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
 

}


