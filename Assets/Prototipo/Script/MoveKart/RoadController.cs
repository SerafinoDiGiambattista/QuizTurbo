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
            roadManager.SpawnSegment();
            //qui dentro segnaliamo la necessita di attiavre un pezzo 
            //deve invocare quindi un metodo di roadmanager??
        }


    }



}
