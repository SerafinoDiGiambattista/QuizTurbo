using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneratorLevel : MonoBehaviour
{
    public GameObject[] section;
    private int zPos=0 ;  // z prodotta, perchè la x e la y non cambiano
    private int lenPezzoStrada = 30;

    //public bool creatingSection = false;
    //public int secNum;

    // Update is called once per frame
    /* void Update()
     {
          if(creatingSection == false)
           {
               creatingSection = true;
               StartCoroutine(GenerateSection());
           } 

     }

     IEnumerator GenerateSection()
     {
         secNum = Random.Range(0, 3);
         Instantiate(section[secNum], new Vector3(0, 0, zPos), Quaternion.identity);
         zPos += 30;
         yield return new WaitForSeconds(1);
         creatingSection = false;
     }*/

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        int len = section.Length; // si prende il numero di pezzi di strada che si vogliono usare per quel percorso
        for ( i = 0; i <len; i++) {
            GeneratoreStrada(i);
            //se sono molti pezzi di strada, quelli già percorsi dalla macchina sarà meglio cancellarli perchè alrtrimenti
            // potrebbero rallentare l'esecuzione!!!
        }
        if(i == len)
        {
            Debug.Log("FINE!!");
        }
    }

    public void GeneratoreStrada(int indiceStrada)
    {
        // istanziamo un pezzo di strada. si posiziona in avanti*la lunghezza delle z
        Instantiate(section[indiceStrada], transform.forward * zPos, transform.rotation);
    
        // il pezzo successivo si posizionerà a:
        zPos += lenPezzoStrada;

    }
}
