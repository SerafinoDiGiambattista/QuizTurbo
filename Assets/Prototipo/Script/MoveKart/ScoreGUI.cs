using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreGUI : MonoBehaviour
{
    public GameObject objectRoad;
    private RoadManager roadManager;
    private float score = 0f;

    void Start()
    {
        roadManager = objectRoad.GetComponent<RoadManager>();

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        score = (roadManager.Space);
        gameObject.GetComponent<Text>().text = Mathf.Abs((Mathf.CeilToInt(score))).ToString();
    }
}
