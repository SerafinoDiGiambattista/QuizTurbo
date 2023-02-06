using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : OverTime
{
    public CountDown(Component component, string param, int val) : base(component) 
    {
        parameter = param;
        valuePerSecond = val;
    }

    public override void Activate()
    {
        c.ReduceComponent(parameter, valuePerSecond);
    }

    public void DoCountdown()
    {
        Activate();
    }
}
