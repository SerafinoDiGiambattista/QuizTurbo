using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesPowerUp : SComponent
{
   protected CountDownManager cdmanager;
   protected TickManager tm;
   protected string TIME = "TIME";
   protected string TICK = "TICK";
   protected int valuePerSecond = 1;

   public ObstaclesPowerUp(string name, string path, ComponentManager comm) : base(name, path, comm){
      cdmanager = cm.GetCountDownManager;
      tm = cm.GetTickManager;
      AddToCountdown();
      AddToTick();
   }

   protected void AddToTick()
   {
      if (!CheckFeature(TICK)) return;
      Tick t = new Tick(this, TICK, (int)m_features[TICK].CurrentValue);
      t.DoTick();
      tm.AddTick(t);
   }

   protected void AddToCountdown()
   {
      if (!CheckFeature(TIME)) return;
      cdmanager.AddCountDown(new CountDown(this, TIME, valuePerSecond));
    }   

}
