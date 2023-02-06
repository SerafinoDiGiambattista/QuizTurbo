using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;

public class PlayerManager : MonoBehaviour
{
    //Leggere feature: health, horizontal speed (che aumenta con la velocità verticale, secondo un certo valore minore di accelerazione)
    private int score;
    private float initialHealth;
    private float health;
    private int highScore;
    private bool isFinished;    // è true quando il giocatore muore o abbandona
    //protected Camera camera;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    private string HEALTH = "HEALTH";

    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
    }

    void Start()
    {
        LoadFeatures();
        health = initialHealth;
        //Debug.Log("initHealth: "+initialHealth);
        //initialHearts = healthController.GetInstantiatedHearts();
;    }

    protected void LoadFeatures()
    {
        initialHealth = featureManager.FeatureValue(HEALTH);
        //Debug.Log("HEALTH: "+ health);
    }

/*
    public string PlayerName{
        get { return playerName; }
        set { playerName = value; }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ostacolo"))
        {
            //float health_reduction = componentManager.FeatureValue(HEALTH);
            if(health > 0)
            {
                health = ComputeHealth();
                SetHealth(health);
                //Debug.Log("HEALTH after health_reduction: "+ health);
            }
            if(health == 0){
                Debug.Log("YOU ARE DEAD! :(");
            }
        }  
    }

    protected float ComputeHealth()
    {
        float health_reduction = componentManager.FeatureValue(HEALTH);
        Debug.Log("health_reduction: "+health_reduction);
        //float reduction = componentManager.FeatureValue(HEALTH);
        //float health_reduction = health + reduction;
        return health_reduction;
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
}