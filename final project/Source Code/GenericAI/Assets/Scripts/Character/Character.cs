using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class Character : MonoBehaviour
{
        public GenericAttributes Attributes;            
        //Private Variables
        private Transform _transform;
        private Joystick _joystick;
        public Vector3 RandomLocation;        
        private GameObject LastPositionGameObject;
        public  Material Radar;
        public  GameObject ProgressBar;
        private Image ProgressBarImage;
        public GameObject[] EnemyDetectors;
        private int StateChangeDelay = 0;
        public TextMeshProUGUI Name;
        private float CollisionCoolDown=0;
    private bool isNavStarted = false;
    void Awake()
    {
        Attributes.nav = this.GetComponent<NavMeshAgent>();
        Attributes.Anim = this.GetComponent<Animator>();
        //Turn Off Detectors
        if(Attributes.Type==CharacterType.Enemy)
        {
            if (PlayerPrefs.GetInt("ENEMYDIFFICULTY", 1) == 0)
            {
                for (int i = 0; i < EnemyDetectors.Length; i++)
                {
                    EnemyDetectors[i].SetActive(false);
                }
            }
        }
      
    }
    public void StartState()
    {
        isNavStarted = true;
           }
    void Start()
    {
        Manager.instance.SetCharacterSpeed(this.GetComponent<Character>());
     _joystick = Manager.instance._joystick;
        _transform = this.transform;
        ConvertState(Attributes.State, Attributes.CurrentMovementState);

        Invoke("StartState", 0.5f);
       // ConvertState(Attributes.State, Attributes.CurrentMovementState);
        if(Attributes.Type!=CharacterType.Enemy)
            Attributes.CurrentActivity = Manager.instance.GetTask(Attributes.CurrentTask,Attributes.Type);       
        ProgressBarImage = ProgressBar.transform.GetChild(0).GetComponent<Image>();

        if(Name!=null)
        Name.text = Attributes.Name;


        SetWanderPosition(RandomDirectionAttribute.Random);

    }
    public void Wandering()
    {       
        Attributes.nav.SetDestination(RandomLocation);    
        if (Vector3.Distance(this.transform.position, RandomLocation) < 0.5f && Attributes.nav.isStopped == false)
        {
            SetWanderPosition(RandomDirectionAttribute.Random);
        }
    }
    
    public void ToogleDetectors(bool Status)
    {
        for(int i=0;i<EnemyDetectors.Length;i++)
        {
            EnemyDetectors[i].gameObject.SetActive(Status);
        }
    }
    public void SetLocomotionState(LocoMotionState LocoState = LocoMotionState.Walk)
    {

        Attributes.Anim.SetInteger("State", (int) LocoState);        
        if (LocoState == LocoMotionState.Idle)
        {
            Attributes.nav.speed = 0;           
        }          
        else if (LocoState == LocoMotionState.Walk)
            Attributes.nav.speed = Attributes.WalkSpeed;
        else if (LocoState == LocoMotionState.Run)
            Attributes.nav.speed = Attributes.RunSpeed;
        else if (LocoState == LocoMotionState.Dead)
            {
                Attributes.nav.speed = 0;
                Attributes.Anim.SetTrigger("Dead");
            }
        Attributes.CurrentMovementState = LocoState;
    }


    public bool BlockChangeState(CurrentState NewState)
    {
        bool status = false;
        if(Attributes.Type==CharacterType.Enemy)
        {
            if (Attributes.State == CurrentState.GoingForTask && NewState != CurrentState.PerformingTask)
                status=true ;
        }
        return status;
    }
    public void ConvertState(CurrentState NewState, LocoMotionState LocoState=LocoMotionState.Walk)
    {

        if (StateChangeDelay > 0)
        {
            if(NewState == CurrentState.GoingForTask)
            {
              
                TaskCompleted();
                
            }
            else
                return;
        }                  
        Attributes.PreviousState = Attributes.State;
        if (Attributes.Type==CharacterType.Bot)
        {
            if (NewState == CurrentState.Idle)
            {
                Attributes.nav.speed = 0;
                SetLocomotionState(LocoState);
            }
            if (NewState == CurrentState.Wander)
            {
                Attributes.TaskDelay = Attributes.GetTaskDelay();
                SetLocomotionState(LocoState);
            }
            if (NewState == CurrentState.Dead)
            {
                tag = "Untagged";
                Attributes.nav.isStopped = true;
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                SetLocomotionState(LocoMotionState.Dead);
                Destroy(this.gameObject, 5f);
            }
            if (NewState == CurrentState.GoingForTask)
            {

                Attributes.CurrentActivity = Manager.instance.GetTask(Attributes.CurrentTask, Attributes.Type);
                SetLocomotionState(LocoMotionState.Run);
            }
            if (NewState == CurrentState.PerformingTask)
            {
                Attributes.nav.speed = 0;
                //SetLocomotionState(LocoMotionState.Interect);              
                //Invoke("TaskCompleted", 5f);
            }
            if (NewState==  CurrentState.BeingChased)
            {
                SetWanderPosition();
                SetLocomotionState(LocoMotionState.Run);
            }            
        }
        if (Attributes.Type == CharacterType.Enemy)
        {
            if(Attributes.State==CurrentState.GoingForTask && NewState!=CurrentState.PerformingTask)
            {
                return;
            }
            if (Attributes.State == CurrentState.PerformingTask)
                return;     
            if (NewState == CurrentState.Wander)
            {
                Attributes.nav.isStopped = false;
                SetLocomotionState(LocoState);
                Radar.SetColor("_Color", Color.green);
            }  
            if (NewState == CurrentState.ChasingPlayer)
            {
                //Destroy Last Position GameObject
                if (LastPositionGameObject != null)
                    Destroy(LastPositionGameObject);
                SetLocomotionState(LocoMotionState.Run);
                Radar.SetColor("_Color", Color.red);
            }
            if(NewState==CurrentState.KillingPlayer)
            {
                Attributes.nav.speed = 0;
                Attributes.nav.isStopped = true;

                Attributes.Anim.SetInteger("State", (int)LocoMotionState.Idle);
                Attributes.Anim.SetTrigger("Kill");
                Attributes.Target.GetComponent<Character>().ConvertState(CurrentState.Dead);
                StateChangeDelay = 1;
                Invoke("TaskCompleted", 2f);
            }
            if (NewState == CurrentState.PerformingTask)
            {
      

                //If It has peformed the slipping Task
                //
                Attributes.CurrentTask++;
                //If all Tasks are completed
                if (Manager.instance.EnemyTasks.Count == Attributes.CurrentTask)
                    Attributes.Anim.SetTrigger("LevelEndAnimation");
                Attributes.nav.isStopped = true;
                Manager.instance.DelayLevelFinished();

                Attributes.CurrentActivity = null;

            }
            if (NewState==CurrentState.CheckingLastPosition)
            {
                if (LastPositionGameObject != null)
                    Destroy(LastPositionGameObject);
                LastPositionGameObject = Instantiate(Manager.instance.LastPositionObject);
                LastPositionGameObject.transform.position = Attributes.TargetLastPosition;
                Color DetectionColour = Color.cyan;
                Radar.SetColor("_Color", DetectionColour);  
            }
            if (NewState==CurrentState.GoingForTask)
            {
                this.GetComponent<CapsuleCollider>().isTrigger = false;
                StartCoroutine(Manager.instance.ToogleEnemyCamera(true));
              
                Attributes.CurrentActivity = Manager.instance.GetTask(Attributes.CurrentTask, Attributes.Type);
            }
        }
        if (Attributes.Type==CharacterType.Player)
        {
            if(NewState == CurrentState.Dead)
            {
             tag = "Untagged";

                Attributes.nav.speed = 0;
                //Attributes.nav.isStopped = true;
             this.GetComponent<CapsuleCollider>().enabled = false;             
             SetLocomotionState(LocoMotionState.Dead);
             Manager.instance.SetGameState(Manager.GameState.PlayerHasDied);
            }           
        }

        Attributes.State = NewState;

        if (NewState == CurrentState.PerformingTask && Attributes.Type != CharacterType.Enemy)
        {
            SetLocomotionState(LocoMotionState.Interect);
            ProgressBar.gameObject.SetActive(true);
            Attributes.TaskPerformingTimer = 0;




        }
        else
        {
            ProgressBar.gameObject.SetActive(false);
        }
    }
    
    public void TaskCompleted()
    {      
        if (Attributes.Type == CharacterType.Player)
        {
            Attributes.CurrentActivity.TaskCompleted();

            Attributes.CurrentTask++;
            Attributes.CurrentActivity.gameObject.SetActive(false);

            if(Attributes.CurrentTask == Manager.instance.PlayersTasks.Count)
                    {
                        Manager.instance.SetGameState(Manager.GameState.PlayerHasWon_Running);
                    }
            else
            {
                Attributes.CurrentActivity = Manager.instance.GetTask(Attributes.CurrentTask, Attributes.Type);
            }
           


            ConvertState(CurrentState.Wander);
         
        }
        if (Attributes.Type == CharacterType.Bot)
        {
            Attributes.CurrentActivity.TaskCompleted();


            Attributes.CurrentTask++;
            if (Attributes.CurrentTask == Manager.instance.PlayersTasks.Count)
            {
                Manager.instance.SetGameState(Manager.GameState.BotHasWon_Running);
                //Manager.instance.LevelFailed();
            }


            ConvertState(CurrentState.Wander,LocoMotionState.Walk);
        }
        if (Attributes.Type==CharacterType.Enemy)
        {
            if(StateChangeDelay==1)
            {
                StateChangeDelay = 0;
                Attributes.nav.isStopped = false;
                //ConvertState(Attributes.PreviousState);

                if (Attributes.CurrentActivity != null)
                    ConvertState(CurrentState.GoingForTask, LocoMotionState.Run);
                else
                    ConvertState(CurrentState.Wander, LocoMotionState.Walk);

                this.GetComponent<CapsuleCollider>().isTrigger = true;
            }

        
        }
      
    }
    public void ReachActivity()
    {
        Attributes.nav.SetDestination(Attributes.CurrentActivity.TaskPosition);
        if (Vector3.Distance(this.transform.position, Attributes.CurrentActivity.TaskPosition) < 4 && Attributes.nav.isStopped == false)
        {
          
            ConvertState(CurrentState.PerformingTask);
           // Attributes.nav.isStopped = true;
        }
    }
    public void FollowPlayer()
    {
        Attributes.nav.SetDestination(Attributes.TargetLastPosition);
       
        if (Vector3.Distance(this.transform.position, Attributes.TargetLastPosition) < 1 && Attributes.nav.isStopped == false)
        {
            //Delete Last Position Object
            if (LastPositionGameObject != null)
                Destroy(LastPositionGameObject);

            //Set Random Position

            //SetRandomPosition(true);            

            SetWanderPosition(RandomDirectionAttribute.TowardsCurrentDirection);

            //If Bot Was Doing Some Task Then Resume That Task
            if (Attributes.CurrentActivity!=null)
                     ConvertState(CurrentState.GoingForTask,LocoMotionState.Run);
            //Otherwise Return the Bot into Wander State
            else
                ConvertState(CurrentState.Wander);

        }
    }
    public void PlayerMove()
    {

        //if (((Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) && _joystick.Vertical == 0 && _joystick.Horizontal == 0))
        //{
        //    SetLocomotionState(LocoMotionState.Idle);

        //    return;
        //}

        //var joystickVector = new Vector3(0, 0, 0);

     
        //joystickVector = _transform.forward * Input.GetAxisRaw("Vertical") + _transform.right * Input.GetAxisRaw("Horizontal");



        //float angle = 0;

        //if (_joystick.Vertical != 0 || _joystick.Horizontal != 0)
        //    angle = Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;
        //else
        //    angle = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;



        //var deltaAngle = Mathf.Abs(Mathf.DeltaAngle(_transform.localEulerAngles.y, angle)) / 90f;
        //deltaAngle = 1 - Mathf.Clamp01(deltaAngle);

        ////float angleLerpFactor = 10;
        //float angleLerpFactor = 5;
        //angle = Mathf.LerpAngle(_transform.localEulerAngles.y, angle,
        //Time.deltaTime * angleLerpFactor * joystickVector.sqrMagnitude);
        //_transform.localEulerAngles = new Vector3(0f, angle, 0f);
        //Vector3 direction = _transform.forward;
        //float speed;



        ////if (joystickVector.magnitude >= 0.35f)
        ////{
        ////    SetLocomotionState(LocoMotionState.Run);
        ////    speed = Attributes.RunSpeed;
        ////}
        ////else
        ////{
        ////    SetLocomotionState(LocoMotionState.Walk);
        ////    speed = Attributes.WalkSpeed;
        ////}

        //SetLocomotionState(LocoMotionState.Run);
        //speed = Attributes.RunSpeed;




        //var moveSpeed = speed * deltaAngle * joystickVector.magnitude;
        //var newPosition = _transform.position + direction.normalized * Time.deltaTime * moveSpeed;
        //_transform.position = newPosition;
    }
     
    private void Update()
    {
        if (Attributes.State != CurrentState.Dead && Manager.instance.CurrentGameState!=Manager.GameState.MiniGameRunning)
        {


            if (Attributes.Type == CharacterType.Bot || Attributes.Type == CharacterType.Enemy)
                {
                    if (Attributes.State == CurrentState.Wander || Attributes.State == CurrentState.BeingChased)
                        Wandering();
                }
            if (Attributes.Type == CharacterType.Bot)
            {
                if (CollisionCoolDown > 0)
                    CollisionCoolDown -= Time.deltaTime;

                Attributes.AutoStateChangeDelay -= Time.deltaTime;
                    if (Attributes.AutoStateChangeDelay < 0)
                        {
                            Attributes.AutoStateChangeDelay = 100;
                            BehaviourHandler.instance.SetRandomMotion(this);
                        }
                
                    if (Attributes.State == CurrentState.GoingForTask)
                        ReachActivity();     
                    
                    if(Attributes.State==CurrentState.Wander|| Attributes.State == CurrentState.Idle)
                            {
                                Attributes.TaskDelay -= Time.deltaTime;
                                if(Attributes.TaskDelay<=0)
                                    {
                                        ConvertState(CurrentState.GoingForTask, LocoMotionState.Run);
                                    }
                            }
                }
            if (Attributes.Type == CharacterType.Enemy)
            {
                if (CollisionCoolDown > 0)
                    CollisionCoolDown -= Time.deltaTime;


                if (Attributes.State == CurrentState.ChasingPlayer || Attributes.State == CurrentState.CheckingLastPosition)                
                    FollowPlayer();

                if (Attributes.State == CurrentState.GoingForTask)
                    ReachActivity();

            }
            if (Attributes.Type == CharacterType.Player)
            {
                PlayerMove();
            }


            if (Attributes.State == CurrentState.PerformingTask && Attributes.Type != CharacterType.Enemy)
            {
                Attributes.TaskPerformingTimer += Time.deltaTime;
                ProgressBarImage.fillAmount = Attributes.TaskPerformingTimer / Attributes.CurrentActivity.TaskDelay;
                //ProgressBarText.text = ((int)((Attributes.TaskPerformingTimer / Attributes.CurrentActivity.TaskDelay) * 100)).ToString() + "%";

                if (Attributes.TaskPerformingTimer >= Attributes.CurrentActivity.TaskDelay)
                {
                    ConvertState(CurrentState.Wander);
                    TaskCompleted();
                    
                }
            }
        }

    }
    //public void SetRandomPosition(bool isReversedDirection = false)
    //{

    //    RandomLocation = Attributes.GetRandomLocation(this.transform.position, 100, 1, 20, isReversedDirection);
    //}


    bool CalculateNewPath()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        navMeshPath.ClearCorners();

        Attributes.nav.CalculatePath(RandomLocation, navMeshPath);
       

        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            print("PTAH FAILED:"+name+RandomLocation);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetWanderPosition(RandomDirectionAttribute Direction=RandomDirectionAttribute.Random)
    {


        int counter = 0;
        do
        {
            RandomLocation = Attributes.GetLocation(this.transform, 300, 10, 0, Direction);

            counter++;
         
        } while (isNavStarted == true && CalculateNewPath() == false && counter < 2);


        //LastPositionGameObject = Instantiate(Manager.instance.LastPositionObject);
        //LastPositionGameObject.transform.position = RandomLocation;
    }

    public CurrentState GetState()
    {
        return Attributes.State;
    }
    public Character GetCharacterBeingFollowed()
    {
        return Attributes.Target.GetComponent<Character>();
    }


    private void OnCollisionStay(Collision other)
    {
        if (Attributes.Type == CharacterType.Enemy && CollisionCoolDown <= 0)
        {
           
            if (Attributes.State == CurrentState.Wander || Attributes.State == CurrentState.BeingChased)
            {
                if (other.gameObject.tag == "Wall")
                {
                    CollisionCoolDown = 0.5f;
                    Debug.Log("NAME:"+other.gameObject.name);
                    SetWanderPosition(RandomDirectionAttribute.ReverseCurrentDirection);
                }
            }
        }



    }
    //private void OnCollisionStay(Collision other)
    //{

    //    //if (Attributes.Type == CharacterType.Bot)
    //    //{
    //    //    //if (Attributes.State == CurrentState.Wander || Attributes.State == CurrentState.BeingChased)
    //    //    //{
    //    //    if (other.gameObject.tag == "Wall")
    //    //    {
    //    //        SetWanderPosition(RandomDirectionAttribute.Random);
    //    //    }

    //    //}



    //    }

    private void OnTriggerStay(Collider other)
    {
        if (Attributes.Type == CharacterType.Bot)
        {
            if (Attributes.State == CurrentState.Wander || Attributes.State == CurrentState.BeingChased)
            {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Bot")
            {
                    if(CollisionCoolDown<=0)
                    {
                              CollisionCoolDown = 3f;
                SetWanderPosition(RandomDirectionAttribute.ReverseCurrentDirection);
                    }
              
            }
            }
        }

     
    }

    
}
