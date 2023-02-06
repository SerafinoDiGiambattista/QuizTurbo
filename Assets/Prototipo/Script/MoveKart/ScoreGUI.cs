using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreGUI : MonoBehaviour
{
    public GameObject objectRoad;
    private RoadManager roadManager;
    //void Awake()
    //{
        

    //}
    // Start is called before the first frame update
    void Start()
    {
        roadManager = objectRoad.GetComponent<RoadManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // Debug.Log(player.position.z);
        Debug.Log(roadManager.VerticalSpeed);
        //gameObject.SetText
    }
}
