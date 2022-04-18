using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{    
    
    public static event Action GameStarted;

    public static event Action<bool> LevelEnded;

    public static void GameStartedEvent()
    {
        GameStarted?.Invoke();
    }

    public static void LevelEndedEvent(bool successful)
    {
        LevelEnded?.Invoke(successful);
    }
}
