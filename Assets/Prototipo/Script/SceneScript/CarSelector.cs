using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSelector : MonoBehaviour
{
    List<GameObject> lista = new List<GameObject>();
    public string NameScene = "CarSelect";
    public int currentCarIndex;
    public GameObject[] carModels;
    // Start is called before the first frame update
    void Start()
    {

        Transform[] position;
      
        currentCarIndex = PlayerPrefs.GetInt(NameScene, 0);
        GameObject car = (GameObject)Instantiate(carModels[currentCarIndex]);
        position = car.GetComponentsInChildren<Transform>();

        Transform player = position.Where(x => x.gameObject.name.Equals("KartBouncingCapsule")).SingleOrDefault();
        player.gameObject.SetActive(true);

    }
}