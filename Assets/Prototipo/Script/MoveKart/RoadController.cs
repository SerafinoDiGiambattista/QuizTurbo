using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadController : MonoBehaviour
{
    RoadManager roadManager;
    //chiediamo al professore Di Penta se gli va bene 
    private void Awake()
    {
        roadManager= FindObjectOfType<RoadManager>();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            //executeWait(3);
            SetRoadComplete();
            roadManager.SpawnSegment(getTrackRoad);
            //qui dentro segnaliamo la necessita di attiavre un pezzo 
            //deve invocare quindi un metodo di roadmanager??
        }
    }

  
    public void SetRoadComplete()
    {
        
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.SetActive(false);
        }
    }

    public GameObject getTrackRoad
    {
        get { return gameObject; }
    }
    
  

}
