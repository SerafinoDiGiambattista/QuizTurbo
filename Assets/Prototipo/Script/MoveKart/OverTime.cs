using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTime
{
    protected float valuePerSecond;
    protected string parameter;
    protected Component c;

    public Component MyComponent
    {
        get { return c; }
        set { c = value; }
    }

    public OverTime(Component component)
    {
        c = component;
    }

    public virtual void Activate() { }

}
