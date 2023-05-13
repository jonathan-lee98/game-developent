using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTask : MonoBehaviour
{
    public bool    TaskPerformed = false;
    public Vector3 TaskPosition;
    public string  TaskDescription;
    public int     TaskDelay;
    public bool    IsEnemyTask;
    public bool    status=false;
    public GameObject LocationofInstantiatedObject;
    public GameObject ObjectToBeInstantiated;
    public bool isMiniGame;
    public MiniGameController.MiniGames Game;
   void Awake()
    {
        TaskPosition = this.transform.position;
    }
    public void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject.name);
            if (isMiniGame==false)
                collision.gameObject.GetComponent<Character>().ConvertState(CurrentState.PerformingTask, LocoMotionState.Idle);
            else 
            {

                //collision.gameObject.GetComponent<Character>().ConvertState(CurrentState.PerformingTask, LocoMotionState.Idle);

                if(Manager.instance.CurrentGameState!=Manager.GameState.MiniGameRunning)
                 MiniGameController.instance.StartMiniGame(Game);

            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
         
            collision.gameObject.GetComponent<Character>().ConvertState(CurrentState.Wander, LocoMotionState.Idle);
        }
    }
    public void TaskCompleted()
    {
        TaskPerformed = true;
        if (IsEnemyTask)
        {
            Manager.instance.DoTaskEnemy();
        }

        if(ObjectToBeInstantiated!=null)
        {
            GameObject t = Instantiate(ObjectToBeInstantiated);

            if (LocationofInstantiatedObject == null)
                t.transform.position = this.transform.position;
            else
                t.transform.position = LocationofInstantiatedObject.transform.position;
        }

    }
}
