using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI eggCollected;
    public TextMeshProUGUI timer;

    public Snake snake;
    public float levelTimer = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        eggCollected.text = ($"Eggs Collected:{snake.totalEggs}");

        levelTimer -= Time.deltaTime;

        if ( levelTimer <= 0 )
        {
            SceneManager.LoadScene("End");
        }

        timer.text = levelTimer.ToString("0");
    }
}
