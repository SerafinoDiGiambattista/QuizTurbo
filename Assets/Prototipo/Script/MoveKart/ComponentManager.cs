using System.Collections;
using System;
using Unity.IO;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class ComponentManager : MonoBehaviour
    {   [SerializeField] protected string componentDirectory;
        protected Dictionary<string, Component> components = new Dictionary<string, Component>();
        protected Dictionary<string, Feature> objFeatures = new Dictionary<string, Feature>();
        protected FeatureManager featureManager;
        protected Dictionary<string, float> featureMulMod = new Dictionary<string, float>();
        protected Dictionary<string, float> featureAddMod = new Dictionary<string, float>();
        protected CountDownManager countDownManager;

        // Classe utilizzata per dare un limite di tempo a tutti gli eventi che hanno una certa durata
        public CountDownManager TheCountDownManager
        {
            get { return countDownManager; }
        }

        private void Awake()
        {
            componentDirectory = Path.Combine(Application.streamingAssetsPath, componentDirectory);
            featureManager = GetComponent<FeatureManager>();
            LoadComponentGroup(componentDirectory);
        }

        private void FixedUpdate()
        {
            ResetModifiers();
            ComputeModifiers();
            ComputeFeatures();
        }
        
        public Dictionary<string, Feature> GetObjFeatures
        {
            get{ return objFeatures;}
            set{objFeatures = value;}
        }

        public Dictionary<string, Component> Components
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
        //Debug.Log("Path: "+path+ " string n: " +n);
        Component c = new Component(Path.GetFileName(n[0].Trim()), path, this);
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
        foreach (KeyValuePair<string, Component> kv in components)
        {
            Dictionary<string, Modifier> mod = components[kv.Key].MyModifiers;
            foreach (Modifier m in mod.Values)
            {
                featureMulMod[m.Type] *= m.MultFactor;
                featureAddMod[m.Type] += m.AddFactor;                 
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

    public Dictionary<string, Component> ComponentsByFeature(string feature)
    {
        return components.Where(x => x.Value.HasFeature(feature)).ToDictionary(x => x.Key, x=> x.Value);
    }

    public void RemoveComponent(string c)
    {
        components.Remove(c);
    }

    public void AddComponent(Component c)
    {
        if (components.ContainsKey(c.NameC)) RemoveComponent(c.NameC);
        components.Add(c.NameC, c);
    }
}


