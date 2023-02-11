using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpeedUp : SComponent
{
    protected CountDownManager cdmanager;
    protected TickManager tm;
    protected string TIME = "TIME";
    protected string TICK = "TICK";
    protected string TICKER = "TICKER";
    protected int valuePerSecond = 1;

    public SpeedUp(string name, string path, ComponentManager comm) : base(name, path, comm){
        cdmanager = cm.GetCountDownManager;
        tm = cm.GetTickManager;
        AddToCountdown();
        AddToTick();
        Activate();
    }

    protected void AddToTick()
    {
        if (!CheckFeature(TICK)) return;
        
        Tick t = new Tick(this, TICK, (int)m_features[TICK].CurrentValue);
        tm.AddTick(t, (int)m_mods[TICKER].MultFactor);
       // tm.SetTimeIntervalSecond((int)m_mods[TICKER].MultFactor);
    }

    protected void AddToCountdown()
    {
        if (!CheckFeature(TIME)) return;
        Debug.Log("value per seco : "+valuePerSecond);
        cdmanager.AddCountDown(new CountDown(this, TIME, valuePerSecond));
    }

    public void Activate()
    {
        
        cm.AddComponent(this);
      
    }
}
