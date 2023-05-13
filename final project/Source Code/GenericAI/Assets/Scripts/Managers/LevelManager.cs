using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LevelManager : MonoBehaviour
{
    public GameObject[] Environments;
    public List<ActivityTask> PlayerTasks;
    public List<ActivityTask> EnemyTasks;
    public Transform[] InstantiatePositions;
    public NavMeshSurface[] Nav;
    public GameObject[] ThingsToTurnOffBeforeNav;

    public Transform EnemyInstantiatePosition;
    public Transform PlayerInstantiatePosition;
    public Transform EnemyFinalPosition;
    void InitLevel()
    {
        for(int i=0;i<Environments.Length;i++)
        {
            GameObject t=Instantiate(Environments[i]);
        }

        for (int z = 0; z < ThingsToTurnOffBeforeNav.Length; z++)
            ThingsToTurnOffBeforeNav[z].gameObject.SetActive(false);

        for(int i=0;i<Nav.Length;i++)
        {
            // Nav[i].BuildNavMesh();
            int Layer = LayerMask.NameToLayer("Ground");
            Nav[i].gameObject.layer = Layer;

        }
           


        for (int z = 0; z < ThingsToTurnOffBeforeNav.Length; z++)
            ThingsToTurnOffBeforeNav[z].gameObject.SetActive(true);

      
        //Invoke("CreateCharacters", 2);
    }
    public void CreateCharacters()
    {

   
    }
    public Transform[] GetInstantiatePositionsForBots()
    {
        return InstantiatePositions;
    }
    public List<ActivityTask> GetPlayerTasks()
    {
        return PlayerTasks;
    }
    public List<ActivityTask> GetEnemyTasks()
    {
        return EnemyTasks;
    }
   
    public Transform GetEnemyInstantiatePosition()
    {
        return EnemyInstantiatePosition;
    }
    public Transform GetPlayerInstantiatePosition()
    {
        return PlayerInstantiatePosition;
    }
    private void Awake()
    {

        InitLevel();

    }
    private void Start()
    {
        UI_Manager.instance.StartTimer();
    }
}
