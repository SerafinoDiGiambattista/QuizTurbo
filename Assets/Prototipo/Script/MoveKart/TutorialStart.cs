using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialStart : MonoBehaviour
{
    protected TutorialManager tutorialManager;
    public GameObject tutorialStartUI;
    public GameObject tutorialEndUI;
    public Button bott;
    public static bool GameIsPaused = false;
    private static bool check =false;
    // Start is called before the first frame update


  

    void Start()
    {
 

        tutorialStartUI.SetActive(false);

        tutorialEndUI.SetActive(false);

        tutorialManager = GetComponentInParent<TutorialManager>();
      


        if (tutorialManager.GetTutorial == true)
        {
            Debug.Log("ok");
            Pause();
        }
        else
        {
            tutorialStartUI.SetActive(false);
        }

        bott.onClick.AddListener(Resume);

 


    }

     private void Update()
    {

        if (tutorialManager.GetTutorial == false && check == true)
        {
            Debug.Log("FINE TUTORIAL");
            StartCoroutine(
                        Endtut());


        }
    }

    public void Pause()
    {

        tutorialStartUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void Resume()
    {
        Debug.Log("ok si parte!");
       
        
        tutorialStartUI.SetActive(false);
        Time.timeScale = 1f;
        check=true;
        Debug.Log("CHECK " + check);
       
        GameIsPaused = false;


    }

    public IEnumerator Endtut()
    {
        tutorialEndUI.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        //Debug.Log("SEI ARRIVATO");
        check = false;

        StopCoroutine(Endtut());

        tutorialEndUI.SetActive(false);
        
    }

}



