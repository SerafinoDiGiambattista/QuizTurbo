using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    List<GameObject> lista = new List<GameObject>();
    public string NameScene = "CarSelect";
    public int currentCarIndex;
    public GameObject[] carModels;

    public string SCORE_PATH;
    public CarBlueprint[] cars;
    public Button buyButton;
    public Button startButton;
    private int bestScore = 0;
    [SerializeField] TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        foreach(CarBlueprint ca in cars)
        {
            if (ca.price == 0)
                ca.isUnlocked = true;
            else
                ca.isUnlocked = PlayerPrefs.GetInt(ca.name, 0) == 0 ? false: true;
        }

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
        ReadScoreFromFile();
    }
    private void Update()
    {
        UpdateUI();
    }
    public void ChangeNext()
    {
        //Debug.Log("next");
        lista[currentCarIndex].SetActive(false);
        currentCarIndex++;
        if (currentCarIndex == lista.Count) currentCarIndex = 0;

        lista[currentCarIndex].SetActive(true);
        CarBlueprint c = cars[currentCarIndex];
        if (!c.isUnlocked)
            return;
        PlayerPrefs.SetInt(NameScene, currentCarIndex);
    }

    public void ChangePrevious()
    {
        //Debug.Log("previous");

        lista[currentCarIndex].SetActive(false);
        currentCarIndex--;
        if (currentCarIndex == -1) currentCarIndex = lista.Count - 1;

        lista[currentCarIndex].SetActive(true);
        CarBlueprint c = cars[currentCarIndex];
        if (!c.isUnlocked)
            return;
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

    public void ReadScoreFromFile()
    {
        string[] lines = File.ReadAllLines(SCORE_PATH);
        foreach (string l in lines)
        {
            bestScore = int.Parse(l);
        }
       
        textMeshPro.SetText(bestScore.ToString());
    }

    public void UpdateUI()
    {
        CarBlueprint c = cars[currentCarIndex];
        if(c.price == 0)
        {
            c.isUnlocked = true;   
            startButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
        if(c.isUnlocked)
        {
            startButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            startButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren < TextMeshProUGUI>().SetText("Buy- " + c.price.ToString());
            if (c.price < bestScore)
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }
    }

    public void UnlockCar()
    {
        CarBlueprint c = cars[currentCarIndex];
        PlayerPrefs.SetInt(c.name, 1);
        PlayerPrefs.SetInt(NameScene, currentCarIndex);
        c.isUnlocked = true;
        startButton.gameObject.SetActive(true);
        bestScore -= c.price;
        UpdateScoreOnFile();
        ReadScoreFromFile();
    }

    public void UpdateScoreOnFile()
    {
        File.WriteAllText(SCORE_PATH, bestScore.ToString());
    }

}

