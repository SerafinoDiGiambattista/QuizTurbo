using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGUI : MonoBehaviour
{
    private int numOfHearts = 0;
    private int initialNumOfHearts;
    public GameObject heart;
    public GameObject playerObject;
    private List<GameObject> instantiatedHearts = new List<GameObject>();
    private float count = 0;
    private FeatureManager featuremanager;
    private RoadManager roadManager;


    private void Awake()
    {
        featuremanager= playerObject.GetComponent<FeatureManager>();
        roadManager= playerObject.GetComponent<RoadManager>();
    }

    void Start()
    {
        /*
       initialNumOfHearts = Mathf.CeilToInt(playerManager.GetInitialHealth());   
       Debug.Log("InitialNumOFHearts: "+ initialNumOfHearts);
       InstantiateHearts();
        */
        
        initialNumOfHearts = Mathf.CeilToInt(roadManager.GetInitialHealth);
        //Debug.Log("InitialNumOFHearts: " + initialNumOfHearts) ;
        InstantiateHearts();
    }

    private void FixedUpdate()
    {
        UpdateHealth();
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
            GameObject temp = Instantiate(heart);
            temp.transform.SetParent(gameObject.transform);
            instantiatedHearts.Add(temp);
        }
    }

    private void UpdateHealth()
    {
        numOfHearts = Mathf.CeilToInt(roadManager.GetHealth);
      // Debug.Log("HEARTCONTROLLER Health: " + numOfHearts);

        if (numOfHearts > 0)
        {
            for (int i = initialNumOfHearts - 1; i >= numOfHearts; i--)
            {
                if (instantiatedHearts[i].activeSelf == true)
                    instantiatedHearts[i].SetActive(false);
            }

            for (int i=0; i < numOfHearts; i++)
            {
                if (instantiatedHearts[i].activeSelf == false)
                    instantiatedHearts[i].SetActive(true);
            }

        }
        else if (numOfHearts == 0)
        {
            instantiatedHearts[0].SetActive(false);
            Debug.Log("DEATH! :)");
        }
    }
}
