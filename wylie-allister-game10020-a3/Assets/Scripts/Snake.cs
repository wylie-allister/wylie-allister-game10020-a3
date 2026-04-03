using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snake : MonoBehaviour
{

    enum State { EggHunt, EggSwallow, EggReturn, Run }
    State state;

    public Transform character;
    public Transform home;
    public GameObject[] eggs;
    public Transform[] eggChildren;

    NavMeshAgent agent;

    public float huntSpeed = 4f;
    public float runSpeed = 6f;

    public int heldEggs = 0;
    public int totalEggs = 0;

    public float swallowTime = 2f;

    public float stationaryProx = 0.3f;

    public int currentEgg = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {

       state = State.EggReturn; 

        for (int i = 0; i < eggs.Length; i++)
        {
            eggChildren[i] = eggs[i].gameObject.transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{state}");
        switch (state)
        {
            case State.EggHunt:
                HuntEgg();
                break;
            case State.EggSwallow:
                EggSwallow();
                break;
            case State.EggReturn:
                EggReturn();
                break;
            case State.Run:
                Run();
                break;

        }
    }

    public void HuntEgg()
    {
        agent.speed = huntSpeed;
        swallowTime = 2f;
        Transform eggTransform = eggChildren[currentEgg];

        if (eggChildren[currentEgg].gameObject.activeInHierarchy == false)
        {
            currentEgg++;
        }

        if (currentEgg >= eggChildren.Length)
        {
            currentEgg = 0;
        }


        agent.SetDestination(eggTransform.position);


        Vector3 posXZ = transform.position;
        posXZ.y = 0;

        Vector3 eggPosXZ = eggTransform.position;
        eggPosXZ.y = 0;

        float distance = Vector2.Distance(posXZ, eggPosXZ);
        if (distance < stationaryProx)
        {
            state = State.EggSwallow;
        }

    }

    public void EggSwallow()
    {

        if (heldEggs == 2)
        {
            state = State.EggReturn;
            return;
        }

        swallowTime -= Time.deltaTime;

        if (swallowTime <= 0)
        {
            state = State.EggHunt;
        }
    }

    public void EggReturn()
    {
        agent.SetDestination(home.position);

        Vector3 posXZ = transform.position;
        posXZ.y = 0;

        Vector3 homePosXZ = home.position;
        homePosXZ.y = 0;

        float distance = Vector2.Distance(posXZ, homePosXZ);
        if (distance < stationaryProx)
        {
            totalEggs += heldEggs;
            heldEggs = 0;
            currentEgg = 0;
            state = State.EggHunt;
        }    
    }

    public void Run()
    {
        agent.speed = runSpeed;
        agent.SetDestination(home.position);

        Vector3 posXZ = transform.position;
        posXZ.y = 0;

        Vector3 homePosXZ = home.position;
        homePosXZ.y = 0;

        float distance = Vector2.Distance(posXZ, homePosXZ);
        if (distance < stationaryProx)
        {
            totalEggs += heldEggs;
            heldEggs = 0;
            state = State.EggHunt;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Egg")
        {
            heldEggs++;
            other.gameObject.SetActive(false);
            currentEgg++;
        }
    }
}
