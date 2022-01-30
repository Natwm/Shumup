using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    CharacterBehaviours player;
    public BatBehaviour[] enemies;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        player = CharacterBehaviours.instance;
        enemies = GameObject.FindObjectsOfType<BatBehaviour>();
    }

    void StopTime(bool isStop = true)
    {
        if (isStop)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            StopTime();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopTime(false);
        }
    }

    public void EndGame()
    {
        StopTime();
        //print("Game Over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StopTime(false);
    }
}
