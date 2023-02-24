using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ShopManager : MonoBehaviour
{
    List<GameObject> lista = new List<GameObject>();
    public string NameScene = "CarSelect";
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
           // Debug.Log("D: " + player.gameObject.name);
            Transform canvas = position.Where(x => x.gameObject.name.Equals("PermanentCanvas")).SingleOrDefault();
            Transform cam = position.Where(x => x.gameObject.name.Equals("Camera")).SingleOrDefault();

            player.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);


            car.SetActive(false);
            currentCarIndex = PlayerPrefs.GetInt(NameScene, 0);

        }
        lista[currentCarIndex].SetActive(true);

    }

    public void ChangeNext()
    {
        Debug.Log("next");
        lista[currentCarIndex].SetActive(false);
        currentCarIndex++;
        if (currentCarIndex == lista.Count) currentCarIndex = 0;

        lista[currentCarIndex].SetActive(true);
        PlayerPrefs.SetInt(NameScene, currentCarIndex);
    }

    public void ChangePrevious()
    {
        Debug.Log("previous");

        lista[currentCarIndex].SetActive(false);
        currentCarIndex--;
        if (currentCarIndex == -1) currentCarIndex = lista.Count - 1;

        lista[currentCarIndex].SetActive(true);
        PlayerPrefs.SetInt(NameScene, currentCarIndex);
    }

    public void BottoneMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void BottoneStart()
    {

        SceneManager.LoadScene(2);
    }
}

