using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void BottoneStart()
    {

        SceneManager.LoadScene(1);
    }

    public void BottoneQuit()
    {
        Debug.Log("QUIT");

        Application.Quit();
    }

}
