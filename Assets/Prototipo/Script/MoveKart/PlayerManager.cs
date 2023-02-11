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
            componentManager.ComponentPickup(name , path);
            //Debug.Log("name: "+name+ " path: "+path);
    
           // Debug.Log("Health: "+health);
        }  
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