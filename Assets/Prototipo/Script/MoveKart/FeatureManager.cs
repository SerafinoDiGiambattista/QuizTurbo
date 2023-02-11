using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using UnityEngine;
using Unity.IO;

    public class FeatureManager : MonoBehaviour
    {
        [SerializeField] protected string featuresDirectory;
        //protected bool loaded = false;
        protected Dictionary<string, Feature> features = new Dictionary<string, Feature>();
        protected Dictionary<string, Feature> baseFeatures = new Dictionary<string, Feature>();
        protected ComponentManager componentManager;

        /*public bool Loaded
        {
            get { return loaded; }
        }
        */

        public string FeaturesDirectory
        {
            get { return featuresDirectory; }
            set { featuresDirectory = value; }
        }

        public Dictionary<string, Feature> Features
        {
            get { return features; }
            set { features = value; }
        }

        public Dictionary<string, Feature> BaseFeatures
        {
            get { return baseFeatures; }
        }

        private void Awake()
        {
            featuresDirectory = Path.Combine(Application.streamingAssetsPath, featuresDirectory);
            componentManager = GetComponent<ComponentManager>();
            LoadFeatures();
            componentManager.GetObjFeatures = features;
        }

        protected void LoadFeatures()
        { 
            if (!Directory.Exists(featuresDirectory))
            {
                Debug.LogError("Features Directory Not Found");
                return;
            }
            string[] fileEntries = Directory.GetFiles(featuresDirectory, "*.csv");
            foreach (string fileName in fileEntries)
            {
                string[] lines = File.ReadAllLines(fileName);
            
                foreach (string l in lines)
                {
              
                    string[] items = l.Split(',');
                    string name = items[0].Trim();
                    float b_value = ParseFloat(items[1]);
                    Feature f = new Feature(name, b_value);
                    AddFeature(f);
                    AddBaseFeature(f);
                    //Debug.Log("F>> name: "+ name + " b_value: "+ b_value);
                }
            }
            //loaded = true;
        }

        protected float ParseFloat(string val)
        {
            return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }

        public float FeatureValue(string feature)
        {
            feature = feature.ToUpper();
            try { 
                return features[feature].CurrentValue; 
            } catch (Exception) { 
                Feature f = new Feature(feature); AddFeature(f); return f.CurrentValue; 
            }
        }

        public Feature GetFeature(string name)
        {
            return features[name];
        }

        public void AddFeature(Feature f)
        {
            features.Add(f.Type, f);
        }

        public void AddBaseFeature(Feature f)
        {
            baseFeatures.Add(f.Type, f);
        }

        public void AddFeature(string name, float val)
        {
            AddFeature(new Feature(name, val));
        }

        public void RemoveFeature(Feature f)
        {
            features.Remove(f.Type);
        }

        public void RemoveFeature(string f)
        {
            features.Remove(f);
        }

    }