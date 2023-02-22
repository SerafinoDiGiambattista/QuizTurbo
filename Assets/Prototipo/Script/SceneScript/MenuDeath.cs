using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDeath : MonoBehaviour
{
    // Start is called before the first frame update
    public void BottoneRestart()
    {

        SceneManager.LoadScene(2);
    }

    public void BottoneMenu()
    {

        SceneManager.LoadScene(0);
    }


    public void BottoneEnd()
    {
        Debug.Log("QUIT");

        Application.Quit();
    }

}
