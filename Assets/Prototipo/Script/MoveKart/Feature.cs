using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Feature
    {
        protected string type;
        protected float base_value = 0;
        protected float current_value = 0;
        
        public float BaseValue
        {
            get { return base_value; }
            set {base_value = value; }
        }
        public float CurrentValue
        {
            get { return current_value; }
            set { current_value = value; }
        }
        public string Type
        {
            get { return type; }
        }

        public Feature(string n, float bv)
        {
            current_value = bv;
            base_value = bv;
            type = n;
        }

        public Feature( string n, float bv, float cv)
        {
            current_value = cv;
            base_value = bv;
            type = n;
        }

        public Feature(string n)
        {
            type = n;
        }
    }


