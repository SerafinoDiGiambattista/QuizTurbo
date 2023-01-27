using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Modifier
    {
        protected string name;
        protected string featureType;
        protected float mult_factor = 1.0f; //defaults to no change
        protected float add_factor = 0.0f;  //defaults to no change

        public string Type
        {
            get { return featureType; }
        }

        public string GetName
        {
            get { return name;}
        }

        public float MultFactor
        {
            get { return mult_factor; }
        }

        public float AddFactor
        {
            get { return add_factor; }
        }

        public Modifier(string t_value, float m_factor, float a_factor)
        {
            featureType = t_value;
            mult_factor = m_factor;
            add_factor = a_factor;
        }

        public Modifier(string t_name,  string t_value, float m_factor, float a_factor)
        {
            name = t_name;
            featureType = t_value;
            mult_factor = m_factor;
            add_factor = a_factor;
        }
    }
