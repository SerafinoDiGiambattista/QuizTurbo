using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] GameObject correctAnswerCanvas;

    public void CollisionObstacleEffect(GameObject go)
    {
        StartCoroutine(Blink(go));
    }

    public IEnumerator Blink(GameObject gameObj)
    {
        GameObject parent = gameObj.transform.parent.gameObject;
        MeshRenderer[] meshes = parent.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer skinnedMeshes = parent.GetComponentInChildren<SkinnedMeshRenderer>();
        int i = 0;
        while (i < 3)
        {
            foreach (MeshRenderer mr in meshes)
            {
                mr.enabled = false;
            }
            skinnedMeshes.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.10f);
            foreach (MeshRenderer mr in meshes)
            {
                mr.enabled = true;
            }
            skinnedMeshes.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.10f);
            i++;
        }
    }

    public void InvincibleShield(bool invincible)
    {
        if (invincible)
        {
            shield.SetActive(true);
        }
        else
            shield.SetActive(false);
    }

    public void ActivateCorrectAnswCanvas()
    {
        correctAnswerCanvas.SetActive(true);
    }

    public void DisableCorrectAnswCanvas()
    {
        correctAnswerCanvas.SetActive(false);
    }
}
