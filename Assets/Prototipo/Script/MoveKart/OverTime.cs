using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTime
{
    protected float valuePerSecond;
    protected string parameter;
    protected SComponent c;

    public SComponent MyComponent
    {
        get { return c; }
        set { c = value; }
    }

    public OverTime(SComponent component)
    {
        c = component;
    }

    public virtual void Activate() { }

}
