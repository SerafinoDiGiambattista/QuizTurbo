using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    public Slider slider;
    public float health;
    public bool TimerOn = false;
    private const float coef = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = health;

        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            Timelife();
        }
    }
    void Timelife()
    {
        if (health>0) {
            
            health -= coef * Time.deltaTime;
            //Debug.Log(health);
            slider.value = health;
        }
        else
        {
            Debug.LogWarning("Attenzione Finita la benzina");
        }
       
    }

}
