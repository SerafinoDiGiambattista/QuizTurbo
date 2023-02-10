using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;
using System.Reflection;

public class PlayerManager : TickSuperclass
{
    private int score;
    private float initialHealth;
    private float health;
    private bool isFinished;    // è true quando il giocatore muore o abbandona
    //protected Camera camera;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    [SerializeField] private string HEALTH = "HEALTH";
    [SerializeField] protected string TICKSPATH;
    protected TickManager tickmanager;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();

    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        LoadParameters(TICKSPATH, tickables);
    }

    void Start()
    {
        LoadFeatures();
        health = initialHealth;
;    }

    protected void LoadFeatures()
    {
        initialHealth = featureManager.FeatureValue(HEALTH);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ostacolo"))
        {
            string path = other.gameObject.GetComponent<PathManager>().Path;
            string[] n = path.Split('.');
            string name =  Path.GetFileName(n[0]);
            componentManager.ComponentPickup( path);
            //Debug.Log("name: "+name+ " path: "+path);
            DoAllTicks();
            Debug.Log("Health: "+health);
        }  
    }


    protected void DoAllTicks()
    {
        foreach (KeyValuePair<string, string> t in tickables)
        {
            ComputeByComponent(t.Key, t.Value);
        }
    }

    public Dictionary<string, float> GetAllTicks(string type)
    {   
        return componentManager.GetAllTicks(type);
    }

    public void ComputeByComponent(string type, string func)
    {
        Dictionary<string, float> filtered = GetAllTicks(type);
        //Debug.Log("Filetered : "+filtered.Count);
        float amount = ComputeFeatureValue(filtered);
        //Debug.Log("Func : "+func);
        if (amount > 0)
        {
            try
            {
                object[] p = { amount };
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(func);
                theMethod.Invoke(this, p);
            } catch{}
        }
    }

    public float ComputeFeatureValue(Dictionary<string, float> received)
    {
        float res = 0;
        foreach (string s in received.Keys)
        {
            try
            {
                Debug.Log("s: "+s+" received: "+received[s]);
                res += received[s];
            }
            catch (Exception) { }
        }
        return res;
    }
/*
    public string PlayerName{
        get { return playerName; }
        set { playerName = value; }
    }
    */

    /*public void HealMe(int h)
    {
        if(health < initialHealth) health += h;
        if(health == 0) Debug.Log("YOU ARE DEAD! :(");   
    }*/

    public void DamageDone(int dmg)
    {
        if(health > 0) health -= dmg;
    }

    public FeatureManager PlayerFeatures
    {
        get { return featureManager; }
    }

    public float FeatureValue(string f)
    {
        return PlayerFeatures.FeatureValue(f);
    }

    public float GetHealth(){
        return health;
    }

    public void SetHealth(float h){
        health = h;
    }

    public float GetInitialHealth(){
        return initialHealth;
    }

    public string GetNameFeature
    {
        get { return HEALTH; }
    }

    protected float ParseFloatValue(string val)
    {
        return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
    }
}