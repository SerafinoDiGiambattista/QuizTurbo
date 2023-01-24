using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GeneratorLevel
{
    public class Component : MonoBehaviour
    {
        protected Dictionary<string, Modifier> m_mods = new Dictionary<string, Modifier>();
        protected Dictionary<string, Feature> m_features = new Dictionary<string, Feature>();
        protected string name;
        protected string path;
        protected ComponentManager cm;

        // COSTRUTTORE
        public Component(string n, string p, ComponentManager c)
        {
            name = n;
            path = p;
            cm = c;
            ReadFile(p);
        }

  

        public void AddModifier(Modifier mod)
        {
            m_mods[mod.GetName] = mod;
        }

        public void AddFeature(Feature f)
        {
            m_features[f.Type] = f;
        }

        public Dictionary<string, Feature> ComponentFeatures
        {
            get{return m_features; }
            set{m_features = value; }
        }

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
    }
}