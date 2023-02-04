using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private int numOfHearts = 0;
    private int initialNumOfHearts;
    public GameObject heart;
    private List<GameObject> instantiatedHearts = new List<GameObject>();
    private float count = 0;
    PlayerManager playerManager;
    private int i;

    private void Awake()
    {
        playerManager= FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        i = 1;
       /*initialNumOfHearts = Mathf.CeilToInt(playerManager.GetInitialHealth());   
       Debug.Log("InitialNumOFHearts: "+ initialNumOfHearts);
       InstantiateHearts();*/
    }

    private void FixedUpdate()
    {
        
        if(i==1)
        {
            initialNumOfHearts = Mathf.CeilToInt(playerManager.GetInitialHealth());   
            //Debug.Log("InitialNumOFHearts: "+ initialNumOfHearts);
            InstantiateHearts();
            i++;
        }
        
        numOfHearts = Mathf.CeilToInt(playerManager.GetHealth());   
        Debug.Log("HEARTCONTROLLER Health: "+numOfHearts);
        
        if(numOfHearts > 0)
        {
            for(int i = initialNumOfHearts-1; i >= numOfHearts; i--)
            {
                if(instantiatedHearts[i].activeSelf == true)
                    instantiatedHearts[i].SetActive(false);
            }
        }
        else if( numOfHearts == 0)
        {
            Debug.Log("DEATH! :)");
        }
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
