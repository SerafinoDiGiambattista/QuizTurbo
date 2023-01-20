using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UIElements;

public class ManagerSpawner : MonoBehaviour
{
    [SerializeField] string fileInput;

    [SerializeField] GameObject road;
    Vector3 nextSpawnPoint = new Vector3 (0,0,0);
    public float zpos = 30f;
    public int index = 5;
    private int count = 0;

    public void SpawnSegment()
    {
        count++;
        
      
        GameObject temp = Instantiate(road, nextSpawnPoint, Quaternion.identity);
        if (count == 3)
        {   
           
            temp.GetComponent<SlideRoad>().SpawnObstacle(false);
            
           

        }
        else if (count == 4)
        {
            //temp.GetComponent<SlideRoad>().SetActivateCanvas(false);
            count = 0;
        }
        else
         temp.GetComponent<SlideRoad>().SpawnObstacle(true);
        //temp.GetComponent<SlideRoad>().SpawnPanel();
    }


    public float StartSpwan(float position, Boolean spawnItems)
    {
        position += zpos;
        nextSpawnPoint = new Vector3(0, 0, position);
        GameObject temp = Instantiate(road, nextSpawnPoint, Quaternion.identity);
        if (spawnItems) {
            //temp.GetComponent<SlideRoad>().SpawnObstacle(false);
            temp.GetComponent<SlideRoad>().SpawnObstacle(true);
            //temp.GetComponent<SlideRoad>().SpawnPanel();
        }
        return position;
    }

    private void Start()
    {
        float position = 0;
        //disattivate il primo trigger
        Instantiate(road, nextSpawnPoint, Quaternion.identity);
        for (int i=0; i<index; i++)
        {
            if (i < index/2)
            {
                position=StartSpwan(position,false);
            }
            else
            {
                position = StartSpwan(position,true);
              
            }
        }
        
        
    }

 


  
}

