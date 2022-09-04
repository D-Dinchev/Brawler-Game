using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event Action OnStartGame;
    public void StartGameTrigger()
    {
        OnStartGame?.Invoke();
    }

    public event Action<OnRestartGameEventArgs> OnRestartGame;

    public class OnRestartGameEventArgs : EventArgs
    {
        public bool GameEnded;
    }

    public void RestartGameTriggerForButtonClick()
    {
        RestartGameTrigger(new EventManager.OnRestartGameEventArgs() { GameEnded = false });
    }
    public void RestartGameTrigger(OnRestartGameEventArgs args)
    {
        OnRestartGame?.Invoke(args);
    }

    public event Action OnStopGame;
    public void StopGameTrigger()
    {
        OnStopGame?.Invoke();
    }

    public event Action<string> OnWinGame;
    public void WinGameTrigger(string endGameText)
    {
        OnWinGame?.Invoke(endGameText);
    }

    public event Action<string> OnLoseGame;
    public void LoseGameTrigger(string endGameText)
    {
        OnLoseGame?.Invoke(endGameText);
    }

    public event Action OnBoxFell;
    public void BoxFellTrigger()
    {
        OnBoxFell?.Invoke();
    }
}
