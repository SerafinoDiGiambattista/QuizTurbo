using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    RoadManager roadManager;

    private void Awake()
    {
        roadManager = FindObjectOfType<RoadManager>();
    }

    public IEnumerator OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            //executeWait(3);
            yield return new WaitForSeconds(3.0f);

            SetRoadComplete();
            roadManager.SpawnSegment(getTrackRoad);
        }
    }

    public void SetRoadComplete()
    {
        Transform[] arr = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform index in arr)
        {
            if (index.CompareTag("Ostacolo") || index.CompareTag("PowerUp") || index.CompareTag("PanelTrue") || index.CompareTag("PanelFalse"))
            {
                index.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }

    public GameObject getTrackRoad
    {
        get { return gameObject; }
    }
}
