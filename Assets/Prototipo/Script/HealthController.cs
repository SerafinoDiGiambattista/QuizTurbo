using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int numOfHearts;
    public GameObject heart;
    private List<GameObject> instantiatedHearts = new List<GameObject>();
    private float count = 0;

    void Start()
    {
       InstantiateHearts();
    }

    private void InstantiateHearts()
    {
        count = heart.transform.position.x;
        for(int i=0; i<numOfHearts; i++)
        {
            Quaternion spawnRotation = Quaternion.identity; //nessuna rotazione
            Vector2 spawnPosition = new Vector2(count,heart.transform.position.y); //heart.transform.position.y, heart.transform.position.z);
            GameObject temp = Instantiate(heart);//, spawnPosition, spawnRotation)as GameObject;
            temp.transform.SetParent(gameObject.transform);
            instantiatedHearts.Add(temp);
        }
        
    }
}
