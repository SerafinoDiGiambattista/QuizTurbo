using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TickManager : MonoBehaviour
{
    // protected int timeIntervalInSecond = 1;
    protected Dictionary<Tick, int> ticks = new Dictionary<Tick, int>();
    protected float timer = 0;
    protected bool tick = true;
    protected Tick tickclass;
    protected int initialintervall;


    void Awake()
    {
        // timer = timeIntervalInSecond;
    }
    //HO MODIFICATO QUIIIIIIIII
    public void AddTick(Tick t, int intervall)
    {
        t.Timer = intervall;
        ticks.Add(t, intervall);

    }


    public void CheckIsActive()
    {
        // tick.RemoveAll(item => item.MyComponent.CheckIsActive() == false);
        ticks = ticks.Where(item => item.Key.MyComponent.CheckIsActive() == true).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void CheckTimer()
    {
        foreach (Tick t in ticks.Keys)
        {
            t.Timer -= Time.deltaTime;

            if (t.Timer <= 0)
            {

                t.DoTick();

                t.Timer = ticks[t];
                // Debug.Log("TICKER timer : "+t.Timer);
            }
        }
    }

    void FixedUpdate()
    {
        CheckTimer();
        CheckIsActive();
    }
}
