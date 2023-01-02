using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7;
    public float lefRightSpeed = 7;
    // Start is called before the first frame update
    public GameObject canvasDomanda;
    public TextMeshProUGUI  textmeshPro;
    public string fileInput;
    //public GameObject explosion; // drag your explosion prefab here

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(this.gameObject.transform.position.x > LevelBoundary.leftSide)
            {
                transform.Translate(Vector3.left * Time.deltaTime * lefRightSpeed);
            }
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if(this.gameObject.transform.position.x < LevelBoundary.rightSide)
            {
                transform.Translate(Vector3.left * Time.deltaTime * lefRightSpeed * -1);
            }
        }
        
    }


    // TRIGGER: quindi lasciare che il personaggio attraversi gli oggetti oppure deve andare
    // a sbattere per poi riprendere il controllo del veicolo??????????????????????????????????????????????????
    public GameObject gameObject;
    private int numBlink=3;

    IEnumerator OnTriggerEnter(Collider collisionInfo)
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
 
        if (collisionInfo.gameObject.CompareTag("Ostacolo"))
        {

            //Animator animator = gameObject.GetComponent<Animator>();
            for (int i = 0; i <  numBlink *2; i++)
            {
                //toggle renderer
                renderer.enabled = !renderer.enabled;
                //wait for a bit
                yield return new WaitForSeconds(0.1f);
            }
            //make sure renderer is enabled when we exit
            renderer.enabled = true; 
            
           /* var expl = (GameObject) Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(collisionInfo.gameObject); // destroy the grenade
            Destroy(expl, 3); // delete the explosion after 3 seconds*/
        }
       
        if(collisionInfo.gameObject.CompareTag("CheckPoint"))
        {
            ReadCSV();
            yield return new WaitForSeconds(0.5f);
            canvasDomanda.SetActive(true);
        }
        else if(collisionInfo.gameObject.CompareTag("CheckPointFinale"))
        {
            canvasDomanda.SetActive(false);
        }
    }

public void ReadCSV()
    {
        StreamReader strReader = new StreamReader(fileInput);

        Dictionary<int, string> tabellaDomande = new Dictionary<int, string>();
        Dictionary<int, int> tabellaRisposte = new Dictionary<int, int>();

        bool endOdFile = false;
        int i = 0;
        
        while (!endOdFile)
        {
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOdFile = true;
                break;
            }
            string[] row = data_String.Split(';');
            tabellaDomande.Add(i, row[0]);
            tabellaRisposte.Add(i, int.Parse(row[1]));
            //Debug.Log(tabellaDomande[i]+ " risposta: "+ tabellaRisposte[i]);  
            i++;
             
        }

        int sizeTabella = i;
        Debug.Log("i: " + i);

        int rand = UnityEngine.Random.Range(0, sizeTabella); // 0 è compreso, sizeTabella non è compreso
        //Debug.Log("random number: " + rand);
        while(!tabellaDomande.ContainsKey(rand))
            rand = UnityEngine.Random.Range(0, sizeTabella);

        string domanda = tabellaDomande[rand];
        tabellaDomande.Remove(rand);

        textmeshPro.SetText(domanda);
        Debug.Log(domanda);

        //int risposta = tabellaRisposte[rand];
        //tabellaRisposte.Remove(rand);    

    }

}
