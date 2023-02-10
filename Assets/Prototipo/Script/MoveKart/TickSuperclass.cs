using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;
using System.Reflection;

public class TickSuperclass: MonoBehaviour
{/*
    protected ComponentManager cm;
    [SerializeField] protected string TICKSPATH;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
    protected TickManager tickmanager;
    
    void Awake()
    {
        cm = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        LoadParameters(TICKSPATH, tickables);
    }

    protected void LoadParameters<T1, T2>(string path, Dictionary<T1, T2> paramDict)
    {
        string[] lines = File.ReadAllLines(path);
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            object param1 = items[0].Trim();
            object param2 = items[1].Trim();
            if (typeof(T2) == typeof(float)) param2 = ParseFloatValue(items[1]);
            paramDict.Add((T1)param1, (T2)param2);
        }
    }

    public void DoAllTicks()
    {
        foreach (KeyValuePair<string, string> t in tickables)
        {
            ComputeByComponent(t.Key, t.Value, cm);
        }
    }

    public void ComputeByComponent<T>(string type, string func, ComponentManager cm, class<T> v)
    {
        Dictionary<string, float> filtered = cm.GetAllTicks(type);
        //Debug.Log("Filetered : "+filtered.Count);
        float amount = ComputeFeatureValue(filtered);
        //Debug.Log("Amount : "+amount);
        if (amount > 0)
        {
            object[] p = { amount };
            Type thisType = v.GetType();
            MethodInfo theMethod = thisType.GetMethod(func);
            theMethod.Invoke(v, p);
        }
    }

    public float ComputeFeatureValue(Dictionary<string, float> received)
    {
        float res = 0;
        foreach (string s in received.Keys)
        {
            try
            {
                res += received[s];
            }
            catch (Exception) { }
        }
        return res;
    }

    protected float ParseFloatValue(string val)
    {
        return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
    }
*/
}
