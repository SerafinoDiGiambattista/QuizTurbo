using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected string SPAWN_PROBABILITY;
    protected FeatureManager featureManager;
    
    void Awake()
    {
        //ReadSpawningProb(SPAWN_PROBABILITY, spawningProb);
        featureManager = GetComponent<FeatureManager>();
        LoadFeatures();
    }

    /*protected void ReadSpawningProb(string path, Dictionary<string, int> valuesDict)
    {
        string[] lines = File.ReadAllLines(path);
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            string param1 = items[0].Trim();
            int param2 = int.Parse(items[1].Trim());
            valuesDict.Add(param1, param2);
        }
    }*/

    protected void LoadFeatures()
    {
        Dictionary<string,Feature> features = featureManager.Features;
        foreach(Feature f in features.Values)
        {
            Debug.Log("Feature: "+f.CurrentValue);
        }
    }

    /*public void ChooseByProbability()
    {
        float total = 0;

        foreach (float elem in probs) {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i= 0; i < probs.Length; i++) {
            if (randomPoint < probs[i]) {
                return i;
            }
            else {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }*/

}

