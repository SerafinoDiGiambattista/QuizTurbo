using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PickUpComponent : MonoBehaviour
{
    protected EffectsManager effectsManager;
    protected RoadManager roadManager;
    protected QuestionManager questionManager;
    protected ComponentManager componentManager;
    [SerializeField] GameObject questionObject;
    private void Awake()
    {
        effectsManager = GetComponent<EffectsManager>();
        roadManager = GetComponent<RoadManager>();
        questionManager = GetComponent<QuestionManager>();
        componentManager = GetComponent<ComponentManager>();
        questionManager= questionObject.GetComponent<QuestionManager>();
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ostacolo") && !roadManager.IsInvincible())
        {
            CreateComponent(other);
            effectsManager.CollisionObstacleEffect(gameObject);
            roadManager.StopMove();
            yield return new WaitForSeconds(0.5f);
            roadManager.GoMove();
        }
        if (other.gameObject.CompareTag("PowerUp"))
        {
            CreateComponent(other);
        }
        if (other.gameObject.CompareTag("PanelTrue") || other.gameObject.CompareTag("PanelFalse"))
        {
            questionManager.DeactivateCanvasQuestion();
            if (other.gameObject.tag.Equals(questionManager.GetCorrectAnswer().tag))
            {
                //Debug.Log("Risposta corretta");
                questionManager.IncrementCorrectAnsw();

                effectsManager.ActivateCorrectAnswCanvas();
                CreateComponent(other);
                yield return new WaitForSeconds(1.5f);
                effectsManager.DisableCorrectAnswCanvas();
            }
            else
            {
                effectsManager.ActivateWrongAnswCanvas();
                yield return new WaitForSeconds(1.5f);
                effectsManager.DisableWrongAnswCanvas();
            }

        }
        if (other.gameObject.CompareTag("checkPointQuestion"))
        {

            questionManager.ActivateCanvasQuestion();
            other.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    public void CreateComponent(Collider other)
    {
        string path = other.gameObject.GetComponent<PathManager>().Path;
        string[] n = path.Split('.');
        string name = Path.GetFileName(n[0]);
        componentManager.ComponentPickup(name, path);

    }
}
