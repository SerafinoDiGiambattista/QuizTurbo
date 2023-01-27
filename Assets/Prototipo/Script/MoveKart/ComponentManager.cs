using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ComponentManager : MonoBehaviour
    {
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

        public Dictionary<string, Component> GetComponents
        {
            get{ return components;}
        }

        private void Awake()
        {
            featureManager = GetComponent<FeatureManager>();
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
            }
        }

    }


