using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneratorLevel
{
    public class Feature
    {
        protected float base_value = 0;
        protected float current_value = 0;
        protected string type;

        public float BaseValue
        {
            get { return base_value; }
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

        public Feature(float bv, string n)
        {
            current_value = bv;
            base_value = bv;
            type = n;
        }

        public Feature(float bv, float cv, string n)
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
}

