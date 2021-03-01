using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        TitleScreen,
        InGame,
        InSetting
    }
    GameState currentState = GameState.TitleScreen;
    [SerializeField] CameraController cc = null;
    void Start()
    {
        cc.SwitchToTitleScreen();
    }
    
}
