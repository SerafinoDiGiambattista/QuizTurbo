using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private int numOfHearts = 0;
    private int initialNumOfHearts;
    public GameObject heart;
    public GameObject playerObject;
    private List<GameObject> instantiatedHearts = new List<GameObject>();
    private float count = 0;
    private FeatureManager featuremanager;
    private PlayerManager playerManager;


    private void Awake()
    {
        featuremanager= playerObject.GetComponent<FeatureManager>();
        playerManager= playerObject.GetComponent<PlayerManager>();

        
    }

    void Start()
    {
        /*
       initialNumOfHearts = Mathf.CeilToInt(playerManager.GetInitialHealth());   
       Debug.Log("InitialNumOFHearts: "+ initialNumOfHearts);
       InstantiateHearts();
        */
        Debug.Log("InitialNumOFHearts: " + featuremanager.FeatureValue(playerManager.GetNameFeature)) ;
        initialNumOfHearts = Mathf.CeilToInt(featuremanager.FeatureValue(playerManager.GetNameFeature));
        InstantiateHearts();
    }



    public List<GameObject> GetInstantiatedHearts()
    {
        return instantiatedHearts;
    }
    
    private void InstantiateHearts()
    {
        count = heart.transform.position.x;
        for(int i=0; i<initialNumOfHearts; i++)
        {
            Quaternion spawnRotation = Quaternion.identity; //nessuna rotazione
            Vector2 spawnPosition = new Vector2(count,heart.transform.position.y); //heart.transform.position.y, heart.transform.position.z);
            GameObject temp = Instantiate(heart);//, spawnPosition, spawnRotation)as GameObject;
            temp.transform.SetParent(gameObject.transform);
            instantiatedHearts.Add(temp);
        }
    }
}
