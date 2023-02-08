using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : OverTime
{
    protected int tickPerSecond = 1;
    protected float timer = 0;
    protected bool active = true;

    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }

    public Tick(Component component, string param, int val) : base(component) 
    {
        parameter = param;
        valuePerSecond = val;
    }

    public float TickInInterval
    {
        get { return valuePerSecond; }
    }

    public override void Activate()
    {
        if (valuePerSecond >= 0)
        {
            valuePerSecond -= tickPerSecond;
            
            if (valuePerSecond < 0) active = false;
        }
       
        if (active) c.TickFeature();
    }

    public void DoTick()
    {
        Activate();
    }
}
