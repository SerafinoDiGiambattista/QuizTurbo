using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine.UIElements;
using System.Reflection;
using System.IO.Abstractions.TestingHelpers;
using UnityEditor;

public class RoadManager : MonoBehaviour
{
    [SerializeField] GameObject trackroad;
    [SerializeField] GameObject weightedObject;
    [SerializeField] GameObject questionObject;
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
    protected float localSpace = 0f;
    protected TickManager tickmanager;
    protected Dictionary<string, string> tickables = new Dictionary<string, string>();
    protected Dictionary<int, List<int>> spawningPositions = new Dictionary<int, List<int>>();
    protected Dictionary<int, List<int>> tutorialPositions = new Dictionary<int, List<int>>();
    protected bool checkObjectActivation = true;
    protected bool invincible = false;
    protected float score_multiple_powerup;
    protected bool pass = false;
    protected bool is_moving;

    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        tickmanager = GetComponent<TickManager>();
        roadController = trackroad.GetComponent<RoadController>();
        weightRandomManager = weightedObject.GetComponent<WeightRandomManager>();
        questionManager = questionObject.GetComponent<QuestionManager>();
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
        {   //lunghezza z dell'oggetto
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
        verticalSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
        maxSpeed = featureManager.FeatureValue(MAX_SPEED);
        horizontalSpeed = featureManager.FeatureValue(HORIZONTAL_SPEED);
        maxHSpeed = featureManager.FeatureValue(MAX_HORIZONTAL);
        numObstacleTrack = featureManager.FeatureValue(NUM_OBSTACLE_TRACK);
        numQuestionTrack = featureManager.FeatureValue(NUM_QUESTION_TRACK);
        score_multiple = featureManager.FeatureValue(SCORE_MULTIPLIER);
    }

    // Update is called once per frame

    void FixedUpdate()
    { //valore booleano ismoving per far capire che si sta muovendo si deve non solo fermare ma deve anche resettare il moltiplicatore

        if (!is_moving) return;

        if (!isTutorial)
        {
            verticalSpeed = featureManager.FeatureValue(VERTICAL_SPEED);
            horizontalSpeed = featureManager.FeatureValue(HORIZONTAL_SPEED);
        }
        else
        {
            verticalSpeed = featureManager.FeatureValueBase(VERTICAL_SPEED);
            horizontalSpeed = featureManager.FeatureValueBase(HORIZONTAL_SPEED);
        }

        if (verticalSpeed > maxSpeed || horizontalSpeed > maxHSpeed)
        {
            verticalSpeed = maxSpeed;
            horizontalSpeed = maxHSpeed;
        }
        //Debug.Log("Vert_Speed: " + verticalSpeed);
        //Debug.Log("Horz_Speed: " + horizontalSpeed);
        foreach (GameObject g in instantiatedTracks)
        {
            g.transform.position += new Vector3(0, 0, -verticalSpeed * Time.deltaTime);
        }

        ScoreMult();
    }

    public void ScoreMult()
    {


        if (componentManager.ComponentsByFeature(SCORE_MULTIPLIER).Count == 1)
        {

            if (!pass) score_multiple = featureManager.FeatureValue(SCORE_MULTIPLIER);

            if (pass)
            {
                score_multiple = featureManager.FeatureValueBase(SCORE_MULTIPLIER);
                Feature f1 = featureManager.Features[SCORE_MULTIPLIER];
                f1.CurrentValue = score_multiple;
            }

            Feature f2 = featureManager.Features[SCORE_MULTIPLIER];
            f2.BaseValue = score_multiple;
            Space += score_multiple * verticalSpeed * Time.deltaTime;
            score_multiple_powerup = 0;
            pass = false;
        }
        else
        {
            score_multiple_powerup = featureManager.FeatureValue(SCORE_MULTIPLIER);
            Space += score_multiple_powerup * verticalSpeed * Time.deltaTime;
            pass = true;
        }
    }


    public void SpawnSegment(GameObject temp)
    {
        numOfSegments++;
        temp.transform.position += new Vector3(0, 0, count);
        temp.GetComponent<RoadController>().gameObject.SetActive(true);
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
            string path = other.gameObject.GetComponent<PathManager>().Path;
            string[] n = path.Split('.');
            string name = Path.GetFileName(n[0]);
            componentManager.ComponentPickup(name, path);
            StopMove();
            yield return new WaitForSeconds(0.5f);
            GoMove();


        }
        if (other.gameObject.CompareTag("PowerUp"))
        {
            string path = other.gameObject.GetComponent<PathManager>().Path;
            string[] n = path.Split('.');
            string name = Path.GetFileName(n[0]);
            componentManager.ComponentPickup(name, path);
        }
        if (other.gameObject.CompareTag("PanelTrue") || other.gameObject.CompareTag("PanelFalse"))
        {
            questionManager.DeactivateCanvasQuestion();
            if (other.gameObject.tag.Equals(questionManager.GetCorrectAnswer().tag))
            {
                Debug.Log("Risposta corretta");
                string path = other.gameObject.GetComponent<PathManager>().Path;
                string[] n = path.Split('.');
                string name = Path.GetFileName(n[0]);
                componentManager.ComponentPickup(name, path);
            }
        }
        if (other.gameObject.CompareTag("checkPointQuestion"))
        {

            questionManager.ActivateCanvasQuestion();
            other.gameObject.GetComponent<Collider>().enabled = false;
        }
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
    public float Space
    {
        set { localSpace = value; }
        get { return localSpace; }
    }

    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }


    public float VerticalSpeed
    {
        get { return verticalSpeed; }
        set { verticalSpeed = value; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
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
}
