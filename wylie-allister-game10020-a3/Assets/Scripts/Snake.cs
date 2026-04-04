using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snake : MonoBehaviour
{

    public enum State { EggHunt, EggSwallow, EggReturn, Run }
    public State state;

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

       state = State.EggHunt; 
        //gets the child of each nest (aka the eggs)
        for (int i = 0; i < eggs.Length; i++)
        {
            eggChildren[i] = eggs[i].gameObject.transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //state swap
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
        //sets hunt speed and resets swallow time
        agent.speed = huntSpeed;
        swallowTime = 2f;
        //sets the transform to the current egg
        Transform eggTransform = eggChildren[currentEgg];

        //sets agent destination to egg transform
        agent.SetDestination(eggTransform.position);

        //distance until the state swaps
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
        //Adjust index number if nothing exists to lock onto
        AdjustIndex();
        //if the snake has more than 2 eggs in it, it returns home to deposit them
        if (heldEggs >= 2)
        {
            state = State.EggReturn;
            return;
        }

        swallowTime -= Time.deltaTime;
        // if the swallow timer elapses, return to hunt
        if (swallowTime <= 0)
        {
            state = State.EggHunt;
        }
    }

    public void EggReturn()
    {
        //sets the snake destination to home
        agent.SetDestination(home.position);

        Vector3 posXZ = transform.position;
        posXZ.y = 0;

        Vector3 homePosXZ = home.position;
        homePosXZ.y = 0;

        float distance = Vector2.Distance(posXZ, homePosXZ);
        if (distance < stationaryProx)
        {
            //adds held eggs to total, resets held eggs and current egg position
            totalEggs += heldEggs;
            heldEggs = 0;
           // currentEgg = 0;
            state = State.EggHunt;
        }    
    }

    public void Run()
    {
        //makes snake run away faster, otherwise pretty much just returnhome
        agent.speed = runSpeed;
        agent.SetDestination(home.position);

        Vector3 posXZ = transform.position;
        posXZ.y = 0;

        Vector3 homePosXZ = home.position;
        homePosXZ.y = 0;

        float distance = Vector2.Distance(posXZ, homePosXZ);
        if (distance < stationaryProx)
        {
            state = State.EggHunt;
        }
    }

    public void AdjustIndex()
    {
        //if that nest's egg is disabled, add up until you find one that isn't
        if (eggChildren[currentEgg].gameObject.activeInHierarchy == false)
        {
            currentEgg++;
        }

        if (currentEgg >= eggChildren.Length)
        {
            currentEgg = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //add held egg and disable gameobject
        if (other.tag == "Egg" && heldEggs < 2)
        {
            heldEggs++;
            other.gameObject.SetActive(false);
        }
    }
}
