using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

    public class Component : MonoBehaviour
    {
        protected Dictionary<string, Modifier> m_mods = new Dictionary<string, Modifier>();
        protected Dictionary<string, Feature> m_features = new Dictionary<string, Feature>();
        protected new string name;
        protected string path;
        protected ComponentManager cm;
        protected bool isActive = true;
        
        // COSTRUTTORE
        public Component(string n, string p, ComponentManager c)
        {
            name = n;
            path = p;
            cm = c;
            ReadFile(p);
        }

        //aggiungi un modificatore
        public void AddModifier(Modifier mod)
        {
            m_mods[mod.GetName] = mod;
        }
        //aggiungi una feature
        public void AddFeature(Feature f)
        {
            m_features[f.Type] = f;
        }
        //ritorna tutti i modifcatori di una componente
        public Dictionary<string, Modifier> MyModifiers
        {
            get { return m_mods; }
        }
        //ritorna le feature di una componente
        public Dictionary<string, Feature> ComponentFeatures
        {
            get{return m_features; }
            set{m_features = value; }
        }
        //read file -> legge il file delle componenti modificare questo tipo di lettura
        public void ReadFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
                foreach (string l in lines)
                {
                    string[] items = l.Split(';');
                    string f_name = items[0].Trim();
                    string feature = items[1].Trim();
                    float mul = float.Parse(items[2]);
                    float add = float.Parse(items[3]);
                    Modifier modifier = new Modifier(f_name, feature, mul, add);
                    AddModifier(modifier);
                    Feature f = new Feature(feature, add);
                    AddFeature(f);
                }
        }

        public Feature GetFeatures(string f)
        {
            foreach(KeyValuePair<string, Feature> string_f in m_features)
            {
                Feature return_feature =  m_features[string_f.Key];
                if(return_feature.Type == f)
                    return return_feature;
            }
            return null;
        }

        public Modifier GetModifiers(string m)
        {
            foreach(KeyValuePair<string, Modifier> string_m in m_mods)
            {
                Modifier return_modifier =  m_mods[string_m.Key];
                if(return_modifier.Type == m)
                    return return_modifier;
            }
            return null;
        }

          public void ReduceComponent(string type, float n)
        {
            if (m_features[type].CurrentValue < 0) return;
            m_features[type].CurrentValue -= n;
            if (m_features[type].CurrentValue < 0) isActive = false;
        }

        public bool CheckIsActive()
        {
            return isActive;
        }
    }
