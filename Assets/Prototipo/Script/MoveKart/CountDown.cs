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
