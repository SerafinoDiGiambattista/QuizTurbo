using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadManager : MonoBehaviour
{
    /// <summary>
    /// ha la lista di tag 
    /// Il compito princiaple è decidere cosa fa spwanare sulla strada: strada vuota, ostacoli e power up oppure domande 
    /// </summary>
    // Start is called before the first frame update
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    [SerializeField] protected List<string> collidersTags;
    protected ObstaclesPowerUp obstaclesPowerUp;
    [SerializeField] GameObject track_road;


    //protected QuestionManager questionManager;

    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        obstaclesPowerUp = GetComponent<ObstaclesPowerUp>();
    }
    void Start()
    {
        IstatiateRoad();
    }

    private  void IstatiateRoad()
    {
        Renderer renderer= track_road.GetComponent<Renderer>();
        if (renderer != null)
        {   //lunghezza z dell'oggetto
            float lenghtz = renderer.bounds.size.z;
           
            float count = 0;
            for (int i =0; i<10; i++)
            {
                Instantiate(track_road, new Vector3(0,0,count), Quaternion.identity) ;
                count += lenghtz;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
