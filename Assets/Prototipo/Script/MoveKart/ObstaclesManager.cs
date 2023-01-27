using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : Component
{
   // QUESTA CLASSE GESTISCE SIA OSTACOLI CHE POWER UP
   protected CountDownManager cdm;
   protected string TIME = "TIME";

   public ObstaclesManager(string name, string path, ComponentManager cm): base(name, path, cm){
            cdm = cm.TheCountDownManager;
            AddToCountdown();
   }

   protected void AddToCountdown()
   {
      if (!CheckFeature(TIME)) return;
      cdm.AddCountDown(new CountDown(this, TIME, 1));
   }
}
