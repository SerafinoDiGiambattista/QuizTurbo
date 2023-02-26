using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class MultiplierManager : MonoBehaviour
{
    [SerializeField] protected List<string> namefeature;
    [SerializeField] protected List<string> namecomponetfile;
    protected FeatureManager featureManager;
    protected TickManager tickmanager;
    protected ComponentManager componentManager;
    protected Dictionary<string, string> multiplier = new Dictionary<string, string>();
    protected Dictionary<string, SpeedUp> tickresets = new Dictionary<string, SpeedUp>();
    protected RoadManager roadManager;
    protected string TICK = "TICKER";
    protected bool pass = false;
    protected float prev;
    protected float next;

    public void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadManager = GetComponent<RoadManager>();
    }

    public void Start()
    {//mettere un try catch
        for (int i = 0; i < namefeature.Count; i++)
        {
            multiplier.Add(namefeature[i], namecomponetfile[i]);
        }


        foreach (string k in multiplier.Keys)
        {
            //Debug.Log(k+" " + multiplier[k]);
            SpeedUp s = (SpeedUp)componentManager.ComponentsByFeature(k)[multiplier[k]];
            tickresets.Add(k, s);
            //Debug.Log(s.TickSpeddUp() +"  "+ s.TickSpeddUp().Timer);
        }
    }


    private void FixedUpdate()
    {
        if (!roadManager.GetMove)
        {


            foreach (string key in tickresets.Keys)
            {
                Feature f0 = featureManager.Features[key];
                f0.CurrentValue = f0.BaseValue;
                tickresets[key].resetTick();

            }
        }

        ScoreMult();
    }


    public void ScoreMult()
    {

        foreach (string features in multiplier.Keys)
        {
            if (componentManager.ComponentsByFeature(features).Count <= 1)
            {
                if (!pass) next = featureManager.FeatureValue(features);

                if (pass)
                {
                    Feature f1 = featureManager.Features[features];
                    f1.CurrentValue = prev;
                    next = prev;
                }

                prev = next;

                pass = false;
            }

            else
            {

                pass = true;
            }
        }

    }
}
