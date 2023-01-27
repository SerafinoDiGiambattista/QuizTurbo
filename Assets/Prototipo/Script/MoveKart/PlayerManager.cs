using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // gameplay specific data
    // we keep these private and provide methods to modify them
    // instead, just to prevent any accidental corruption or invalid
    // data coming in

    private int score;
    private int highScore;
    private int health;
    private bool isFinished;    // è true quando il giocatore muore o abbandona
    //protected Camera camera;
    protected FeatureManager featureManager;
    protected ComponentManager componentManager;
    //protected PlayerHealthManager playerHealthManager;
    protected CharacterStatus characterStatus;
    protected MoveKart mk;
    //protected ScoreManager scoreManager;
    
    public string playerName = "Gabriella";

    public virtual void GetDefaultData()
    {
        playerName = "Gabriella";
        score = 0;
        health = 3;
        highScore = 0;
        isFinished = false;
    }

    public string PlayerName{
        get { return playerName; }
        set { playerName = value; }
    }
    
    private void Awake()
    {
        featureManager = GetComponent<FeatureManager>();
        componentManager = GetComponent<ComponentManager>();
        //healthManager = GetComponent<PlayerHealthManager>();
        characterStatus = GetComponent<CharacterStatus>();
        mk = GetComponent<MoveKart>();
        //scoreManager = GetComponent<ScoreManager>();
    }

    public FeatureManager PlayerFeatures
    {
        get { return featureManager; }
    }

    public float FeatureValue(string f)
    {
        return PlayerFeatures.FeatureValue(f);
    }


    /*void Start()
    {
        ActivateCam();
    }
    */

/*
    protected void ScoreCommand()
    {
        score = PlayerPref.GetString("SCORE");  ????????
    }*/


/*
    private void FixedUpdate()
    {
        if(healthManager.IsDead)
        {
            scorePoint.SaveState();
            SetDeathAnimator();
        }
    }
*/

/*    private void OnTriggerEnter(Collider collider)
    {
        string tag = collider.gameObject.tag.ToUpper();
        if(IsTag(tag))
        {
            string path = other.gameObject.GetComponent<PathManager>().Path;
            string[] n = path.Split('.');
            string name =  Path.GetFileName(n[0]);
            componentManager.ComponentPickup(tag, name, path);
        }
    }
*/

///////////// DOVE VANNO QUESTI ??????????????????
    public int GetHighScore(){
        return highScore;
    }

    public int GetScore(){
        return score;
    }

    public virtual void AddScore(int anAmount){
        score += anAmount;
    }
    
    public void LostScore(int num)
    {
        score -= num;
    }
    public void SetScore(int num)
    {
        score = num;
    }
    public int GetHealth()
    {
        return health;
    }
    public void AddHealth(int num)
    {
        health += num;
    }
    public void ReduceHealth(int num){
        health -= num;
    }
    public void SetHealth(int num)
    {
        health = num;
    }
    public bool GetIsFinished()
    {
        return isFinished;
    }
    public void SetIsFinished(bool aVal)
    {
        isFinished = aVal;
    }

}