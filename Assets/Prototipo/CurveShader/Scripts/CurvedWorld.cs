using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorld : MonoBehaviour 
{

    public Vector3 Curvature = new Vector3(0, 0.5f, 0);
    public float Distance = 0;
    protected RoadManager roadManager;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    [SerializeField] protected string CURVATURE = "CURVATURE";
    [SerializeField] protected string MAX_CURVATURE = "MAX_CURVATURE";
    [SerializeField] protected string CHANGING_CURVATURE = "CHANGING_CURVATURE";
    Dictionary<string, SComponent> curvatureComponents;
    [Space]
    public float CurvatureScaleUnit = 1000f;
    public string CurvatureFileName;
    int CurvatureID;
    int DistanceID;
    protected float currentCurvature = 0;
    protected Modifier mod;

    private void Awake()
    {
        roadManager = FindObjectOfType<RoadManager>();
        featureManager = roadManager.fm();
        componentManager = roadManager.cm();
    }

    private void Start()
    {
  
        //Debug.Log("Add mod: " + mod.AddFactor+ " Mult mod: "+mod.MultFactor);
    }

    private void OnEnable()
    {
        CurvatureID = Shader.PropertyToID("_Curvature");
        DistanceID = Shader.PropertyToID("_Distance");
    }

    private void FixedUpdate()
    {
        curvatureComponents = componentManager.ComponentsByFeature(CURVATURE);
        //SComponent comp = curvatureComponents[CurvatureFileName];
        //mod = comp.GetModifier(CURVATURE);
        ApplyCurvature();
    }

    public void ApplyCurvature()
    {
        currentCurvature = featureManager.FeatureValue(CURVATURE);
       // Debug.Log("current: " + Mathf.Abs(currentCurvature));
       // Debug.Log(" max: " + GetMaxCurvature());
        if (Mathf.Abs(currentCurvature) >= GetMaxCurvature())
        {
            componentManager.SetAddModifierByFeature(CURVATURE, -1);
            //mod.AddFactor *= -1;
        }
        //Debug.Log("add: " + mod.AddFactor);
        Curvature.z = currentCurvature;
        Vector3 curvature = CurvatureScaleUnit == 0 ? Curvature : Curvature / CurvatureScaleUnit;
        Shader.SetGlobalVector(CurvatureID, curvature);
        Shader.SetGlobalFloat(DistanceID, Distance);
        //Debug.Log("curvatura: " + Curvature.z);
    }

    public float GetMaxCurvature()
    {
        float curv = Mathf.Abs(featureManager.FeatureValue(MAX_CURVATURE));
        return curv;
    }
}
