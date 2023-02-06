using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TickManager : MonoBehaviour
{
    [SerializeField] protected int timeIntervalInSecond = 1;
    protected List<Tick> ticks = new List<Tick>();
    protected float timer = 0;
    protected bool tick = true;

    void Awake()
    {
        //timer = timeIntervalInSecond;
    }

    public void AddTick(Tick t)
    {
        t.Timer = timer;
        ticks.Add(t);
    }

    public void CheckIsActive()
    {
        ticks.RemoveAll(item => item.MyComponent.CheckIsActive() == false);
    }

    public void CheckTimer()
    {
        foreach(Tick t in ticks)
        {
            t.Timer -= Time.deltaTime;
            if (t.Timer <= 0)
            {
                t.DoTick();
                t.Timer = timeIntervalInSecond;
            }
        }
    }

    void FixedUpdate()
    {
        CheckTimer();
        CheckIsActive();
    }
}
