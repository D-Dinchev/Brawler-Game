using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event Action<OnStartEventArgs> OnStartGame;
    public class OnStartEventArgs : EventArgs
    {
        public AudioClip Clip;
    }
    public void StartGameTrigger(OnStartEventArgs args)
    {
        OnStartGame?.Invoke(args);
    }

    public void StartGameTriggerForButtonClick()
    {
        StartGameTrigger(new OnStartEventArgs());
    }

    public event Action<OnRestartGameEventArgs> OnRestartGame;

    public class OnRestartGameEventArgs : EventArgs
    {
        public bool GameEnded;

        public AudioClip Clip;
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

    public event Action<OnWinGameEventArgs> OnWinGame;
    public class OnWinGameEventArgs : EventArgs
    {
        public AudioClip Clip;

        public string endGameText;
    }

    public void WinGameTrigger(OnWinGameEventArgs args)
    {
        OnWinGame?.Invoke(args);
    }

    public event Action<OnLoseGameEventArgs> OnLoseGame;
    public class OnLoseGameEventArgs : EventArgs
    {
        public AudioClip Clip;

        public string endGameText;
    }

    public void LoseGameTrigger(OnLoseGameEventArgs args)
    {
        OnLoseGame?.Invoke(args);
    }

    public event Action<OnBoxFellEventArgs> OnBoxFell;

    public class OnBoxFellEventArgs : EventArgs
    {
        public AudioClip Clip;
    }
    public void BoxFellTrigger(OnBoxFellEventArgs args)
    {
        OnBoxFell?.Invoke(args);
    }
}
