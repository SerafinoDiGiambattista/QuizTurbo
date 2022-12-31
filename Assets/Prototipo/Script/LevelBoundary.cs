using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundary : MonoBehaviour
{
    // Start is called before the first frame update
    public static float leftSide;
    public static float rightSide;
    public float internalLeft;
    public float internalRight;

    // Update is called once per frame
    void Update()
    {
        leftSide = internalLeft ;
        rightSide = internalRight;
    }
}
