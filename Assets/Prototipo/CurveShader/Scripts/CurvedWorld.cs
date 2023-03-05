using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CurvedWorld : MonoBehaviour
{

    public Vector3 Curvature = new Vector3(0, 0.5f, 0);
    public float Distance = 0;
    protected RoadManager roadManager;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    [SerializeField] protected string CURVATURE = "CURVATURE";
    [SerializeField] protected string MAX_CURVATURE = "MAX_CURVATURE";
    Dictionary<string, SComponent> curvatureComponents;
    [Space]
    public float CurvatureScaleUnit = 1000f;
    public string CurvatureFileName;
    int CurvatureID;
    int DistanceID;
    protected float currentCurvature = 0;
    protected Modifier mod;
    protected bool change;
    protected float negative, positive;

    private void Awake()
    {
        roadManager = FindObjectOfType<RoadManager>();
        featureManager = roadManager.fm();
        componentManager = roadManager.cm();
    }

    private void Start()
    {   //aggiustare qui per pulire il codice 
        curvatureComponents = componentManager.ComponentsByFeature(CURVATURE);
        foreach (string key in curvatureComponents.Keys)
        {
            SComponent s = curvatureComponents[key];
            if(s.CheckFeature(CURVATURE))
            {
                mod = s.GetModifier(CURVATURE);
                negative = mod.AddFactor * -1;
                positive = mod.AddFactor;
            }
        }

        change = true;

    }

    private void OnEnable()
    {
        CurvatureID = Shader.PropertyToID("_Curvature");
        DistanceID = Shader.PropertyToID("_Distance");
    }

    private void FixedUpdate()
    {

        ApplyCurvature();
    }

    public void ApplyCurvature()
    {
        currentCurvature = featureManager.FeatureValue(CURVATURE);

        if ((currentCurvature >= GetMaxCurvature()) & change)
        {
            mod.AddFactor = negative;
            change = false;
        }
        else if ((currentCurvature <= (GetMaxCurvature() * -1)) & !change)
        {
            mod.AddFactor = positive;
            change = true;
        }

        Curvature.z = currentCurvature;
        Vector3 curvature = CurvatureScaleUnit == 0 ? Curvature : Curvature / CurvatureScaleUnit;
        Shader.SetGlobalVector(CurvatureID, curvature);
        Shader.SetGlobalFloat(DistanceID, Distance);

    }

    public float GetMaxCurvature()
    {
        return featureManager.FeatureValue(MAX_CURVATURE);
    }
}
