using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    public Color[] CameraColors;
    public  GameObject[] Levels;
    private LevelManager CurrentLevel;
    private GameObject[] EnemyDetectors;
    public  GameObject[] BotsPrefab;
    public int Botscount = 0;
    public Transform[] InstantiatePositions;
    public GameObject PlayerCamera;
    public GameObject EnemyCamera;
    public Camera MainCamera;
    public GameObject Fade;
    public GameObject Enemy;
    public GameObject MiniMap;
    public GameObject HUDNAVIGATIONCANVAS;
    public enum GameState
    {
        Running,Paused,PlayerHasWon_Running, BotHasWon_Running, Completed,PlayerHasDied,MiniGameRunning
    }
    public GameState CurrentGameState=GameState.Running;
    public static Manager instance;
    public Joystick _joystick;

    public GameObject Player;
    public NavMeshTriangulation Triangulation;

    

 

    public void InitCharacters()
    {

        int PositionStartingRange = 0;
        int PositionMaxRange=0;
        if(Botscount!=0)
            PositionMaxRange = 10 / Botscount;

        int differencecount = PositionMaxRange;

        for (int i = 0; i < Botscount; i++)
        {
            GameObject t = Instantiate(BotsPrefab[Random.Range(0, BotsPrefab.Length)]);


            t.transform.position = InstantiatePositions[Random.Range(PositionStartingRange, PositionMaxRange)].position;
            t.GetComponent<Character>().Attributes.Name = "BOT " + (i + 1);
            t.GetComponent<NavMeshAgent>().enabled = true;
            t.GetComponent<Character>().enabled = true;
            AllCharacters.Add(t.GetComponent<Character>());

            PositionStartingRange = PositionMaxRange;
            PositionMaxRange = PositionMaxRange+ differencecount;
        }


        Enemy.SetActive(true);
    }

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }

        int levelNumber = PlayerPrefs.GetInt("LEVEL", 1);
        levelNumber--;
        MainCamera.backgroundColor = CameraColors[levelNumber];

        CurrentLevel =Instantiate(Levels[levelNumber]).GetComponent<LevelManager>();
        CurrentLevel.gameObject.SetActive(true);
        
        InstantiatePositions = CurrentLevel.GetInstantiatePositionsForBots();

        PlayersTasks = CurrentLevel.GetPlayerTasks();
        EnemyTasks = CurrentLevel.GetEnemyTasks();

        Enemy.transform.position = CurrentLevel.GetEnemyInstantiatePosition().position;
        Player.transform.position = CurrentLevel.GetPlayerInstantiatePosition().position;

        Botscount = PlayerPrefs.GetInt("BOTSQUANTITY", 5);


        AllCharacters.Add(Player.GetComponent<Character>());
        AllCharacters.Add(Enemy.GetComponent<Character>());

      

    }
  

    public  List<Character> AllCharacters;
    [HideInInspector]
    public List<ActivityTask> PlayersTasks;
    [HideInInspector]
    public List<ActivityTask> EnemyTasks;


    private void Start()
    {
        for(int i=1;i<PlayersTasks.Count;i++)
        {
            PlayersTasks[i].gameObject.SetActive(false);
        }


        Triangulation = NavMesh.CalculateTriangulation();

    }
    public GameObject LastPositionObject;
    public void DoTaskAI()
    {


        for(int i=0;i<AllCharacters.Count;i++)
        {

            if(AllCharacters[i]!=null)
            {
                if (AllCharacters[i].Attributes.Type == CharacterType.Bot && AllCharacters[i].Attributes.State != CurrentState.Dead)
                {
                    AllCharacters[i].ConvertState(CurrentState.GoingForTask);
                }
            }
           
        }
    }
    public void DoTaskEnemy()
    {
        for (int i = 0; i < AllCharacters.Count; i++)
        {
            if(AllCharacters[i]!=null)
            {
                if (AllCharacters[i].Attributes.Type == CharacterType.Enemy && AllCharacters[i].Attributes.State != CurrentState.Dead)
                {
                    AllCharacters[i].ConvertState(CurrentState.GoingForTask, LocoMotionState.Run);
                }
            }    

           
        }
    }
    public void DoWanderAI()
    {
        for (int i = 0; i < AllCharacters.Count; i++)
        {
            if (AllCharacters[i].Attributes.Type == CharacterType.Bot && AllCharacters[i].Attributes.State!=CurrentState.Dead)
            {
                AllCharacters[i].ConvertState(CurrentState.Wander);
            }
        }
    }
    public ActivityTask GetTask(int TaskNumber, CharacterType TypeOfCharacter)
    {
        if(TypeOfCharacter == CharacterType.Bot)
        {
            return PlayersTasks[TaskNumber];
        }
        if(TypeOfCharacter==CharacterType.Player)
        {
            PlayersTasks[TaskNumber].gameObject.SetActive(true);
            UI_Manager.instance.SetPlayerText(PlayersTasks[TaskNumber].TaskDescription);
            return PlayersTasks[TaskNumber];
        }

        else if (TypeOfCharacter == CharacterType.Enemy)
        {
            EnemyTasks[TaskNumber].gameObject.SetActive(true);
            return EnemyTasks[TaskNumber];
            //UI_Manager.instance.SetPlayerText(EnemyTasks[TaskNumber].TaskDescription);
        }

        return null;

     
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.R))
           
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void DelayLevelFinished()
    {
        _joystick.gameObject.SetActive(false);

        Invoke("LevelCompleted", 4f);
    }

    public void LevelCompleted()
    {
        if (CurrentGameState == GameState.PlayerHasDied)
            UI_Manager.instance.PlayerDiedPanel.gameObject.SetActive(true);
        if (CurrentGameState == GameState.PlayerHasWon_Running)
            UI_Manager.instance.LevelCompletePanel.SetActive(true);
        else if (CurrentGameState == GameState.BotHasWon_Running)
            UI_Manager.instance.LevelFailedPanel.SetActive(true);
        CurrentGameState = GameState.Completed;
        HUDNAVIGATIONCANVAS.SetActive(false);
        MiniMap.SetActive(false);
        Time.timeScale = 0;
   
    }
    
    private void OnDisable()
    {
        Time.timeScale = 1;
    }        
    public void SetGameState(GameState state)
    {
        if (CurrentGameState == GameState.BotHasWon_Running || CurrentGameState == GameState.PlayerHasWon_Running)
            return;
        CurrentGameState = state;
        if (state == GameState.PlayerHasDied)
            DelayLevelFinished();

        if(state==GameState.PlayerHasWon_Running || state == GameState.BotHasWon_Running)
        {
            for (int i = 0; i < AllCharacters.Count; i++)
            {
                if (AllCharacters[i] != null)
                {
                    if (AllCharacters[i].Attributes.Type == CharacterType.Bot)
                    {

                        AllCharacters[i].gameObject.SetActive(false);
                        //if(AllCharacters[i]!=null)
                        //AllCharacters[i].ConvertState(CurrentState.Idle);
                    }

                    if (AllCharacters[i].Attributes.Type == CharacterType.Enemy)
                    {
                        AllCharacters[i].ToogleDetectors(false);
                        }
                }
                   
              
            }
            if(state == GameState.PlayerHasWon_Running)
            {
                _joystick.gameObject.SetActive(false);
                _joystick.ResetJoystick();
                _joystick.enabled = false;

            }
            for(int x=0;x<PlayersTasks.Count;x++)
            {
                PlayersTasks[x].gameObject.SetActive(false);
            }

            _joystick.enabled = false;
        }
    }

    public IEnumerator ToogleEnemyCamera(bool status)
    {
        if(status)
        {
            Fade.gameObject.SetActive(false); 
            Fade.gameObject.SetActive(true);
            Enemy.GetComponent<Character>().SetLocomotionState(LocoMotionState.Walk);
            Enemy.transform.position = CurrentLevel.EnemyFinalPosition.position;
          
        }
        yield return new WaitForSeconds(1.5f);
        EnemyCamera.SetActive(status);
        PlayerCamera.SetActive(!status);
    }
    public void SetDelayOn()
    {
       
    }
    public AnimationClip FindAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
    public void SetCharacterSpeed(Character CHARACTER)

    {
        //if (CHARACTER.Attributes.Type == CharacterType.Enemy)
        //{
        //    CHARACTER.Attributes.WalkSpeed = 50;
        //    CHARACTER.Attributes.RunSpeed = 50;
        //    return;
        //}

        
        if (CHARACTER.Attributes.Type==CharacterType.Enemy)
        {
            if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 1|| PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 0 )
            {
                 CHARACTER.Attributes.WalkSpeed =5.5f;
                 CHARACTER.Attributes.RunSpeed = 7.5f;

                //AnimationClip anim = FindAnimation(CHARACTER.Attributes.Anim, "Running");

                //CHARACTER.Attributes.Anim.speed = 0.65f;

            }
            else if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 2)
            {
                CHARACTER.Attributes.WalkSpeed =6.5f;
                CHARACTER.Attributes.RunSpeed = 10.5f;
            }
            else
            {
                CHARACTER.Attributes.WalkSpeed = 10.5f;
                CHARACTER.Attributes.RunSpeed = 14.5f;
            }
            CHARACTER.Attributes.Anim.SetFloat("WalkingSpeed", (float)0.186 * CHARACTER.Attributes.WalkSpeed);
            CHARACTER.Attributes.Anim.SetFloat("RunningSpeed", (float)0.100 * CHARACTER.Attributes.RunSpeed);
        }



    }
    public void SetRange(Detector RangeDetector)
    {
        if (RangeDetector.TypeOfCharacter == CharacterType.Enemy)
        {
            if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 0)
            {
                RangeDetector.gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 1)
            {
                RangeDetector.range = 10;
   
            }
            else if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 2)
            {
                RangeDetector.range = 25;
            }
            else
            {
                RangeDetector.range = 50;
            }
        }
    }

    public void GetFieldOfView(FieldOfView fov)
    {
        
            if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 0)
            {
               fov.gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 1)
            {
                fov.viewRadius =5.8f;
                 fov.viewAngle = 24f;

            }
            else if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 2)
            {
                fov.viewRadius = 9.04f;
            fov.viewAngle = 76f;
            }
            else
            {
            fov.viewRadius = 12.87f;
            fov.viewAngle = 99f;
        }
        
    }


    public void SetBotState(CurrentState c,LocoMotionState l)
    {
        for (int i = 0; i < AllCharacters.Count; i++)
        {

            if (AllCharacters[i] != null)
            {
                AllCharacters[i].ConvertState(c, l);
            }
        }
    }
}
