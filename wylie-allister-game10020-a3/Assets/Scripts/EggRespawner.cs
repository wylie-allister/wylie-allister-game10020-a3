using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EggRespawner : MonoBehaviour
{
    public GameObject[] eggs;

    public float eggTimer;

    public int eggSelect;
    // Start is called before the first frame update
    void Start()
    {
        //creates initial randomization timer
        eggTimer = Random.Range(3f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        //randomly picks between eggs to respawn
      eggSelect = Random.Range(0, eggs.Length);
        //reduces random timer time
        eggTimer -= Time.deltaTime;

        if (eggTimer <= 0)
        {
            //sets a random egg to spawn and resets timer
            eggs[eggSelect].SetActive(true);
            eggTimer = Random.Range(3f, 8f);
        }



}
}
