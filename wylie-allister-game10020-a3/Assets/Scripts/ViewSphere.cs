using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSphere : MonoBehaviour
{
    public Snake snake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //just activates run if player is in range
            snake.state = Snake.State.Run;
        }
    }
}
