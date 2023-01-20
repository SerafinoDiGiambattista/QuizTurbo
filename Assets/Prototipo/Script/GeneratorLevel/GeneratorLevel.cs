using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneratorLevel : MonoBehaviour
{
    public GameObject[] section;
    private int zPos = 0;  // z prodotta, perchè la x e la y non cambiano
    private int lenPezzoStrada = 30;

    
    void Start()
    {
        int i = 0;
        int len = section.Length; // si prende il numero di pezzi di strada che si vogliono usare per quel percorso
        for (i = 0; i < len; i++)
        {
            GeneratoreStrada(i);
            //se sono molti pezzi di strada, quelli già percorsi dalla macchina sarà meglio cancellarli perchè alrtrimenti
            // potrebbero rallentare l'esecuzione!!!
        }
        if (i == len)
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
