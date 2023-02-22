using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSelector : MonoBehaviour
{
    List<GameObject> lista = new List<GameObject>();

    public int currentCarIndex;
    public GameObject[] carModels;
    // Start is called before the first frame update
    void Start()
    {

        Transform[] position;

        for (int i = 0; i < carModels.Length; i++)
        {
            GameObject car = (GameObject)Instantiate(carModels[i]);
            lista.Add(car);
            position = lista[i].GetComponentsInChildren<Transform>();
           

            Transform player = position.Where(x => x.gameObject.name.Equals("KartBouncingCapsule")).SingleOrDefault();
            player.gameObject.SetActive(true);

            car.SetActive(false);
            currentCarIndex = PlayerPrefs.GetInt("SelectedCar", 0);


            /*foreach (GameObject car in carModels)
            {
                list.Add(Instantiate(car));
                 
                Debug.Log("instanzio : "+list[currentCarIndex]);
                list[currentCarIndex].SetActive(true);
                 
            }
    */
        }
        lista[currentCarIndex].SetActive(true);

    }
}