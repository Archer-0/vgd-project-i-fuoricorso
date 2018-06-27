using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public bool canSpawn = true; // bool per vedere se può iniziare a spawnare o meno

    public bool isInSameSpot = false; //bool per spawnare a griglia(false) o spawnare a due a due per un numero definito

    public int numberOfSpawn = 5; //se isInSameSpot è true viene usato per il numero di spawn a due a due

    public float timer = 5F; //tempo tra uno spawn e l'altro

    public int maxLines = 5; //numero massimo di oggetti spawnati nella riga

    public int maxColumns = 5; //numero massimo di oggetti spawnati nella colonna

    public GameObject objectSpawn; //oggetto da spawnare

    public ParticleSystem particle;

    private bool isActive = true; //controllo sullo spawn
       
    //distanza fisica tra un oggetto spawnato e l'altro
    private float moveX = 0;
    private float moveZ = 0;

    //contatori
    private int count = 0;    

    // Use this for initialization
    void Start()
    {
        particle.enableEmission = false;
    }

    // Update is called once per frame
    void Update() {

        if (canSpawn == true) //se può spawnare
        {
            /*
                * Modificato perche' modificava il prefab
                */

            //if (objectSpawn.CompareTag("Enemy")) //se voglio istanziare un nemico
            //{
            //    EnemyController enemycontroller = objectSpawn.GetComponent<EnemyController>(); //prendo l'enemy controller

            //    enemycontroller.seekAndDestroyMode = true; //rendo i nemici in modalità seek and destroy                
            //}

            particle.enableEmission = true;

            isActive = true;
            Spawn();
            canSpawn = false;
        }

        if (!isActive) {

            Destroy(gameObject);
        }

    }
    
    void Spawn()
    {       
        if (isActive == true)
        {
            if(!isInSameSpot) //se è a griglia
            {
                if (count < maxLines) //se il contatore è minore del numero di linee indicate
                {
                    Vector3 position = new Vector3( //creo un vettore per la posizione dell'oggetto sottraendo una unità per asse x e z ad ogni richiamo
                    this.transform.position.x - moveX,
                    this.transform.position.y,
                    this.transform.position.z - moveZ
                    );
                    
                    GameObject enemy = Instantiate(objectSpawn, position, Quaternion.identity); //instanzio l'oggetto                    
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemyCount += 1;
                    /*
                     * MODIFICATO QUI
                     */

                    enemy.GetComponent<EnemyController>().seekAndDestroyMode = true;

                    moveX += 1; //incremento moveX per incrementare la sottrazione della posizione dell'asse x

                    if (count >= maxLines -1) //se count è maggiore delle righe volute
                    {
                        moveZ += 1; //incremento moveZ per incrementare la sottrazione della posizione dell'asse z
                        moveX = 0; //quando raggiunge la fine della riga riporto la nuova riga a capo

                        count = 0; //azzera il contatore
                    }
                    else
                    {
                        count++; //se count è minore delle righe volute incrementa il contatore
                    }
                }

                if (moveZ == maxColumns) //se move z arriva al massimo numero di colonne
                {
                    isActive = false; //disattivo l'entrata nell'if

                    //resetto i contatori per renderlo riutilizzabile
                    moveX = 0;
                    moveZ = 0;
                    count = 0;

                    //if (objectSpawn.CompareTag("Enemy")) //se voglio istanziare un nemico
                    //{
                    //    EnemyController enemycontroller = objectSpawn.GetComponent<EnemyController>(); //prendo l'enemy controller

                    //    enemycontroller.seekAndDestroyMode = false; //tolgo la modalità seek and destroy ai nemici                        
                    //}

                    particle.enableEmission = false;
                }

                StartCoroutine(Attendi()); //chiamo la coroutine per attendere e richiamare questa funzione

                Debug.Log("contatore = " + count);
            }
            else
            {
                if(count < numberOfSpawn) //se il numero corrente dello spawn è minore del numero massimo da spawnare
                {
                    Vector3 position1 = new Vector3( //creo un vettore per la posizione del primo oggetto
                    this.transform.position.x - 0.5F,
                    this.transform.position.y,
                    this.transform.position.z
                    );

                    Vector3 position2 = new Vector3( //creo un vettore per il secondo oggetto spostato di una unità nell'asse x
                        this.transform.position.x + 0.5F,
                        this.transform.position.y,
                        this.transform.position.z
                        );

                    GameObject enemy = null;
                    enemy = Instantiate(objectSpawn, position1, Quaternion.identity); //istanzio il primo oggetto
                    enemy.GetComponent<EnemyController>().seekAndDestroyMode = true;
                    enemy = Instantiate(objectSpawn, position2, Quaternion.identity); //istanzio il secondo oggetto
                    enemy.GetComponent<EnemyController>().seekAndDestroyMode = true;

                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemyCount += 2;


                    StartCoroutine(Attendi()); //chiamo la coroutine per attendere e richiamare questa funzione

                    count++; //incremento il contatore di spawn
                }
                else
                {
                    isActive = false; //disattivo lo spawn
                    count = 0; //resetto il contatore per renderlo riutilizzabile

                    //if (objectSpawn.CompareTag("Enemy")) //se voglio istanziare un nemico
                    //{
                    //    EnemyController enemycontroller = objectSpawn.GetComponent<EnemyController>(); //prendo l'enemy controller

                    //    enemycontroller.seekAndDestroyMode = false; //tolgo la modalità seek and destroy ai nemici              
                    //}

                    particle.enableEmission = false;
                }
                //Debug.Log("numero spawn = " + count);
            }
        }
    }

    IEnumerator Attendi()
    {
        yield return new WaitForSeconds(timer); //attende per "timer" secondi
        
        Spawn();

    }
}
