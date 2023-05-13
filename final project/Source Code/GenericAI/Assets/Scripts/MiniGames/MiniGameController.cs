using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    public enum MiniGames
    {
        FindTheSequence,
        SelectMostOccuringAnimal,
        FindPassCode
    }

    public GameObject[] AllMiniGames;
    private GameObject TaskBeingDone;
    public static MiniGameController instance;
    public bool isMiniGameRunning;
    GameObject MiniGame;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        isMiniGameRunning = false;
    }
    public void StartMiniGame(MiniGames GameType)
    {
        Manager.instance.MiniMap.SetActive(false);

        isMiniGameRunning = true;
        MiniGame=Instantiate(AllMiniGames[(int)GameType]);
        Manager.instance.SetGameState(Manager.GameState.MiniGameRunning);

        int Layer= LayerMask.NameToLayer("Default");

        Manager.instance.Player.layer = Layer;
        Manager.instance.Player.tag = "Untagged";


        for (int i = 0; i < Manager.instance.AllCharacters.Count; i++)
        {

            if (Manager.instance.AllCharacters[i] != null)
            {
                Manager.instance.AllCharacters[i].gameObject.tag = "Untagged";
                Manager.instance.AllCharacters[i].gameObject.layer = Layer;

            }
        }

        Manager.instance.SetBotState(CurrentState.Idle,LocoMotionState.Idle);
        
    }
    public void StopMiniGame(bool result=true)
    {
        int BotLayer = LayerMask.NameToLayer("Bot");
        for (int i = 0; i < Manager.instance.AllCharacters.Count; i++)
        {

            if (Manager.instance.AllCharacters[i] != null)
            {
                Manager.instance.AllCharacters[i].gameObject.tag = "Bot";
                Manager.instance.AllCharacters[i].gameObject.layer = BotLayer;

            }
        }


        Manager.instance.CurrentGameState = Manager.GameState.Running;

        Manager.instance.SetBotState(CurrentState.Wander, LocoMotionState.Walk);

        MiniGame.SetActive(false);
        int Layer = LayerMask.NameToLayer("Player");

        Manager.instance.Player.layer = Layer;
        Manager.instance.Player.tag = "Player";

        if(result)
            Manager.instance.Player.GetComponent<Character>().TaskCompleted();

        Destroy(MiniGame);
        isMiniGameRunning = false;
        Manager.instance.MiniMap.SetActive(true);


    }

}
