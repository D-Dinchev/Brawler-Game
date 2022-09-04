using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TextMeshProUGUI _textSlider;

    [SerializeField] private float _gameTime;
    private float _timeLeft;

    private bool _stopTimer;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        _timeLeft = _gameTime;
        _stopTimer = true;
        _timerSlider.maxValue = _gameTime;
        _timerSlider.value = _gameTime;
        _textSlider.text = ConvertToMinAndSec(_gameTime);

        EventManager.Instance.OnStartGame += LaunchTimer;
        EventManager.Instance.OnStopGame += ResetTimer;
        EventManager.Instance.OnRestartGame += LaunchTimer;
    }

    private void Update()
    {
        if (!_stopTimer) StartTimer();
    }

    public void ResetTimer()
    {
        _stopTimer = true;
        _timerSlider.value = _gameTime;
        _textSlider.text = ConvertToMinAndSec(_gameTime);
        _timeLeft = _gameTime;
    }

    private void StartTimer()
    {
        _timeLeft -= Time.deltaTime;

        string textTime = ConvertToMinAndSec(_timeLeft);

        if (_timeLeft <= 0f)
        {
            _stopTimer = true;
            GameManager.Instance.SetGameEndBool(new EventManager.OnRestartGameEventArgs() { GameEnded = true });
            EventManager.Instance.StopGameTrigger();
            EventManager.Instance.LoseGameTrigger("НЕВДАХА");
        }

        if (!_stopTimer)
        {
            _textSlider.text = textTime;
            _timerSlider.value = _timeLeft;
        }
    }

    private string ConvertToMinAndSec(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    private void LaunchTimer()
    {
        _stopTimer = false;
    }

    private void LaunchTimer(EventManager.OnRestartGameEventArgs args)
    {
        _stopTimer = false;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnStartGame -= LaunchTimer;
        EventManager.Instance.OnStopGame -= ResetTimer;
        EventManager.Instance.OnRestartGame -= LaunchTimer;
    }
}
