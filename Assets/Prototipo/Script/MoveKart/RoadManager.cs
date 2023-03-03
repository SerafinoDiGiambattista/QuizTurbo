using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class RoadManager : MonoBehaviour
{
    protected EffectsManager effectsManager;

    [SerializeField] GameObject trackroad;
    [SerializeField] GameObject weightedObject;
    [SerializeField] GameObject questionObject;
    [SerializeField] GameObject shield;
    [SerializeField] protected string VERTICAL_SPEED = "VERTICAL_SPEED";
    [SerializeField] protected string MAX_SPEED = "MAX_SPEED";
    [SerializeField] protected string HORIZONTAL_SPEED = "HORIZONTAL_SPEED";
    [SerializeField] protected string MAX_HORIZONTAL = "MAX_HORIZONTAL";
    [SerializeField] protected string HEALTH = "HEALTH";
    [SerializeField] protected string SCORE_MULTIPLIER = "SCORE_MULTIPLIER";
    [SerializeField] protected string SPAWNING_POSITIONS;
    [SerializeField] protected string TUTORIAL_SPAWNING;
    [SerializeField] protected string NUM_OBSTACLE_TRACK = "NUM_OBSTACLE_TRACK";
    [SerializeField] protected string NUM_QUESTION_TRACK = "NUM_QUESTION_TRACK";
    [SerializeField] protected string INVINCIBILITY = "INVINCIBILITY";
    [SerializeField] protected string MAX_CURVATURE = "MAX_CURVATURE";
    [SerializeField] protected bool isTutorial = false;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    protected WeightRandomManager weightRandomManager;
    protected RoadController roadController;
    protected QuestionManager questionManager;
    protected float verticalSpeed;
    protected float health = 0;
    protected float score_multiple = 0f;
    protected int numOfSegments = 0;
    protected float horizontalSpeed;
    protected float maxSpeed;
    protected float maxHSpeed;
    protected float count = 0;
    protected float numObstacleTrack = 0;
    protected float numQuestionTrack = 0;
    protected Dictionary<string, float> roadFeatures = new Dictionary<string, float>();
    protected List<GameObject> instantiatedTracks = new List<GameObject>();
    protected float localPoint = 0f;
    protected TickManager tickmanager;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
    protected Dictionary<int, List<int>> spawningPositions = new Dictionary<int, List<int>>();
    protected Dictionary<int, List<int>> tutorialPositions = new Dictionary<int, List<int>>();
    protected bool checkObjectActivation = true;
    protected bool invincible = false;
    protected bool is_moving;
   
    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadController = trackroad.GetComponent<RoadController>();
        weightRandomManager = weightedObject.GetComponent<WeightRandomManager>();
        questionManager = questionObject.GetComponent<QuestionManager>();
        effectsManager = GetComponent<EffectsManager>();
        ReadSpawningFile(SPAWNING_POSITIONS, spawningPositions);
        ReadSpawningFile(TUTORIAL_SPAWNING, tutorialPositions);

    }

    void Start()
    {
        LoadFeatures();
        numObstacleTrack = Mathf.CeilToInt(numObstacleTrack);
        numQuestionTrack = Mathf.CeilToInt(numQuestionTrack);
        IstatiateRoad();
        is_moving = true;


    }

    protected void ReadSpawningFile(string path, Dictionary<int, List<int>> valuesDict)
    {
        string[] lines = File.ReadAllLines(path);
        int i = 0;

        foreach (string l in lines)
        {
            List<int> array = new List<int>();
            string[] items = l.Split(',');
            foreach (string item in items)
            {
                int param = int.Parse(item.Trim());
                array.Add(param);
            }
            valuesDict.Add(i, array);
            i++;
        }
    }

    private void IstatiateRoad()
    {
        Transform[] position;
        Renderer renderer = roadController.getTrackRoad.GetComponent<Renderer>();
        if (renderer != null)
        {
            float lenghtz = renderer.bounds.size.z;
            count = 0;
            for (int i = 0; i < numObstacleTrack; i++)
            {
                instantiatedTracks.Add(Instantiate(roadController.getTrackRoad, new Vector3(0, 0, count), Quaternion.identity));
                position = instantiatedTracks[i].GetComponentsInChildren<Transform>();
                position = position.Skip(1).ToArray();
                var positionsObs = position.Where(x => x.gameObject.tag.Equals("Spawn")).ToArray();
                Transform cp = position.Where(x => x.gameObject.tag.Equals("checkPointQuestion")).SingleOrDefault();
                Collider c = cp.gameObject.GetComponent<Collider>();
                c.enabled = false;
                InstatiateObject(positionsObs);

                Transform positionTrue = position.Where(x => x.gameObject.tag.Equals("spawnTrue")).SingleOrDefault();
                Transform positionFalse = position.Where(x => x.gameObject.tag.Equals("spawnFalse")).SingleOrDefault();
                InstantiateTF(positionTrue, positionFalse);

                count += lenghtz;
            }

            for (int i = 2; i < instantiatedTracks.Count; i++)
            {
                ActivateObject(instantiatedTracks[i]);
            }
        }
    }

    private void InstantiateTF(Transform t, Transform f)
    {
        questionManager.GetPanels[1].SetActive(false);
        questionManager.GetPanels[0].SetActive(false);
        Instantiate(questionManager.GetPanels[1], t.position, questionManager.GetPanels[1].transform.rotation, t);
        Instantiate(questionManager.GetPanels[0], f.position, questionManager.GetPanels[0].transform.rotation, f);
    }

    private void InstatiateObject(Transform[] position)
    {
        foreach (Transform index in position)
        {
            foreach (GameObject g in weightRandomManager.GetObjectSpwan)
            {
                g.SetActive(false);
                Vector3 vect = new Vector3(0, g.transform.position.y, 0);
                Instantiate(g, index.position + vect, g.transform.rotation, index);

            }
        }
    }

    protected void LoadFeatures()
    {
        
        maxSpeed = featureManager.FeatureValue(MAX_SPEED);
        maxHSpeed = featureManager.FeatureValue(MAX_HORIZONTAL);
        numObstacleTrack = featureManager.FeatureValue(NUM_OBSTACLE_TRACK);
        numQuestionTrack = featureManager.FeatureValue(NUM_QUESTION_TRACK);
        verticalSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
        horizontalSpeed = featureManager.FeatureValue(HORIZONTAL_SPEED);
        score_multiple = featureManager.FeatureValue(SCORE_MULTIPLIER);

    }

    void FixedUpdate()
    { //valore booleano ismoving per far capire che si sta muovendo si deve non solo fermare ma deve anche resettare il moltiplicatore
        if (!GetMove) return;

        if (!isTutorial)
        {
            verticalSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
            horizontalSpeed = featureManager.FeatureValue(HORIZONTAL_SPEED);
            score_multiple = featureManager.FeatureValue(SCORE_MULTIPLIER);
            Point += verticalSpeed * Time.deltaTime * score_multiple;
           Debug.Log("speed " + verticalSpeed);
           
        }
        else
        {   //picollo problema nel tutorial 
            
           
        }

        if (verticalSpeed > maxSpeed || horizontalSpeed > maxHSpeed)
        {
            verticalSpeed = maxSpeed;
            horizontalSpeed = maxHSpeed;
        }

        
        foreach (GameObject g in instantiatedTracks)
        {
            g.transform.position += new Vector3(0, 0, -verticalSpeed * Time.deltaTime);
        }
        effectsManager.InvincibleShield(IsInvincible());
    }


    public void UnchangedValue()
    {
        verticalSpeed = featureManager.FeatureValueBase(VERTICAL_SPEED);
        horizontalSpeed = featureManager.FeatureValueBase(HORIZONTAL_SPEED);
        score_multiple = featureManager.FeatureValueBase(SCORE_MULTIPLIER);
        Point = 0;

    }


    public void SpawnSegment(GameObject temp)
    {
        numOfSegments++;
        temp.transform.position += new Vector3(0, 0, count);
        temp.gameObject.SetActive(true);

        //Debug.Log("numSeg: "+numOfSegments);
        Transform[] children = temp.GetComponentsInChildren<Transform>();
        Transform cp = children.Where(x => x.gameObject.tag.Equals("checkPointQuestion")).SingleOrDefault();
        Collider c = cp.gameObject.GetComponent<Collider>();

        if (numOfSegments <= 1)
        {
            CheckObjActivation = false;
            c.enabled = true;

        }
        if (numOfSegments == numQuestionTrack - 1)
        {
            SetActivatePanels(temp);
        }

        if (numOfSegments == numQuestionTrack + 1) CheckObjActivation = true;
        if (numOfSegments == numObstacleTrack + numQuestionTrack)
        {
            numOfSegments = 0;
        }
        else ActivateObject(temp);
    }

    public void SetActivatePanels(GameObject trackroad)
    {
        Transform[] children = trackroad.GetComponentsInChildren<Transform>();
        Transform positionTrue = children.Where(x => x.gameObject.tag.Equals("spawnTrue")).SingleOrDefault();
        Transform positionFalse = children.Where(x => x.gameObject.tag.Equals("spawnFalse")).SingleOrDefault();
        SetActiveRecursively(positionTrue, true);
        SetActiveRecursively(positionFalse, true);
    }

    public void SetActiveRecursively(Transform parent, bool active)
    {
        parent.gameObject.SetActive(active);
        foreach (Transform c in parent)
        {
            SetActiveRecursively(c, active);
        }
    }

    public void ActivateObject(GameObject grandfather)
    {
        Dictionary<int, List<int>> binaryFile = new Dictionary<int, List<int>>();
        if (!isTutorial) binaryFile = spawningPositions;
        else { binaryFile = tutorialPositions; }
        //la probabilita degli oggetti non deve aumentare con il tutorial attivo !!!
        if (CheckObjActivation)
        {
            int randomRow = UnityEngine.Random.Range(0, binaryFile.Count);
            List<int> binaryRow = binaryFile[randomRow];
            int index = 0;
            Transform[] father = grandfather.GetComponentsInChildren<Transform>();
            father = father.Skip(1).ToArray();
            foreach (int i in binaryRow)
            {
                if (i != 0)
                {
                    string name = weightRandomManager.ChooseByProbability();
                    for (int j = 0; j < father[index].childCount; j++)
                    {
                        GameObject child = father[index].GetChild(j).gameObject;
                        string namechild = child.name.ToUpper().Replace("(CLONE)", "");
                        if ((!child.activeSelf) && namechild.Equals(name))
                        {
                            father[index].GetChild(j).gameObject.SetActive(true);
                        }
                    }
                }
                index++;
            }
        }
    }


    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ostacolo") && !IsInvincible())
        {
            CreateComponent(other);
            effectsManager.CollisionObstacleEffect(gameObject);
            StopMove();
            yield return new WaitForSeconds(0.5f);
            GoMove();
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
                Debug.Log("Risposta corretta");
                questionManager.IncrementCorrectAnsw();

                effectsManager.ActivateCorrectAnswCanvas();
                CreateComponent(other);
                yield return new WaitForSeconds(1.5f);
                effectsManager.DisableCorrectAnswCanvas();
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

    
 
    public void StopMove()
    {
        is_moving = false;
    }

    public void GoMove()
    {
        is_moving = true;
    }


    public bool GetMove
    {
        get { return is_moving; }

    }
    public float Point
    {
        set { localPoint = value; }
        get { return localPoint; }
    }

    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }



    public float GetInitialHealth
    {
        get { return featureManager.FeatureValue(HEALTH); }
    }

    public float GetHealth()
    {
        float h = featureManager.FeatureValue(HEALTH);
        float b = featureManager.FeatureValueBase(HEALTH);
        if (h > b) { featureManager.GetFeature(HEALTH).CurrentValue = b; h = b; }

        return h;
    }

    public void ResetHealth()
    {
        featureManager.GetFeature(HEALTH).CurrentValue = featureManager.FeatureValueBase(HEALTH);
    }
    public void ResetScoreMultiplier()
    {
        featureManager.GetFeature(SCORE_MULTIPLIER).CurrentValue = featureManager.FeatureValueBase(SCORE_MULTIPLIER);
    }

    public bool CheckObjActivation
    {
        get { return checkObjectActivation; }
        set { checkObjectActivation = value; }
    }

    public bool IsInvincible()
    {
        if (componentManager.ComponentsByFeature(INVINCIBILITY).Count == 0) return false;
        else return true;
    }

    public bool IsTutorial
    {
        get { return isTutorial; }
        set { isTutorial = value; }
    }

    public FeatureManager fm()
    {
        return featureManager;
    }

    public ComponentManager cm()
    {
        return componentManager;
    }

    public float GetScoreMultiple
    {
        get{ return score_multiple; }
    }
}
