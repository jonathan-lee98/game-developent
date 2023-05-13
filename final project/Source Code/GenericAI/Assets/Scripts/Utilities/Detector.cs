using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Character CharacterObj;
    private Transform LastPosition;
    public CharacterType TypeOfCharacter;
    public float range;
    private float Delay = 0;
    void Start()
    {
        Manager.instance.SetRange(this);
    }
    void Update()
    {       
        RaycastHit hit;
        int layer_mask;        
        if (TypeOfCharacter==CharacterType.Enemy)
        {
            layer_mask = LayerMask.GetMask("Player","Bot","Environment");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range,layer_mask))
            {
                if ((hit.transform.gameObject.tag != "Player") && (hit.transform.gameObject.tag != "Bot"))
                    {
                       

                        if (CharacterObj.GetState()==CurrentState.ChasingPlayer)
                            {
                                CharacterObj.GetCharacterBeingFollowed().SetWanderPosition(RandomDirectionAttribute.ReverseCurrentDirection);
                                CharacterObj.ConvertState(CurrentState.CheckingLastPosition);
                            }
                       
                        return;
                    }


                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    LastPosition = hit.collider.transform;
                    CharacterObj.Attributes.Target = hit.collider.gameObject;

                    if(hit.collider.gameObject.GetComponent<Character>().GetState() != CurrentState.BeingChased)
                        {
                             hit.collider.gameObject.GetComponent<Character>().ConvertState(CurrentState.BeingChased);
                        }
                    
                    if (hit.distance < 3)
                    {
                        if (CharacterObj.GetState() != CurrentState.KillingPlayer)
                        {
                            CharacterObj.ConvertState(CurrentState.KillingPlayer);

                        }
                    }
                    else
                    {

                        CharacterObj.Attributes.SetTarget(LastPosition.position, hit.collider.gameObject);
                        if (CharacterObj.GetState() != CurrentState.ChasingPlayer)
                            CharacterObj.ConvertState(CurrentState.ChasingPlayer);
                    }            
            }
            else
            {
                if (CharacterObj.GetState() == CurrentState.ChasingPlayer)
                {
                   
                    //CharacterObj.GetCharacterBeingFollowed().Getew(true);
                    CharacterObj.ConvertState(CurrentState.CheckingLastPosition);
                }

                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);

            }
        }        
        if (TypeOfCharacter == CharacterType.Bot)
        {
            if (Delay > 0)
                Delay -= Time.deltaTime;
            if(Delay<=0)
            {

                layer_mask = LayerMask.GetMask("Enemy");
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, layer_mask))
                {
                    if (hit.transform.gameObject.tag != "Enemy")
                        return;
                    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
                    Delay = 1f;
                    CharacterObj.SetWanderPosition(RandomDirectionAttribute.ReverseCurrentDirection);
                    CharacterObj.ConvertState(CurrentState.Wander, LocoMotionState.Run);
                }
            }
           
        }
    }

   
}
