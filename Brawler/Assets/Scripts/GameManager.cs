using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _endGameTextObject;
    private TextMeshProUGUI _endGameText;

    private GameObject[] _boxes;
    private Vector3[] _boxesStartPosition;
    private float _boxesToPushCount;

    public bool GameEnded { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameEnded = false;

        HideObject(null);

        _endGameText = _endGameTextObject.GetComponent<TextMeshProUGUI>();

        EventManager.Instance.OnLoseGame += ShowText;
        EventManager.Instance.OnWinGame += ShowText;
        EventManager.Instance.OnRestartGame += HideObject;
        EventManager.Instance.OnRestartGame += RepositionBoxesAtStart;
        EventManager.Instance.OnRestartGame += ResetBoxesToPushCount;
        EventManager.Instance.OnBoxFell += DecrementBoxesCount;
        EventManager.Instance.OnRestartGame += SetGameEndBool;

        _boxes = GameObject.FindGameObjectsWithTag("Box");
        _boxesToPushCount = _boxes.Length;
        _boxesStartPosition = new Vector3[_boxes.Length];
        for (int i = 0; i < _boxes.Length; i++)
        {
            _boxesStartPosition[i] = _boxes[i].transform.position;
        }

    }


    private void ShowText(string text)
    {
        _endGameText.text = text;
        _endGameTextObject.SetActive(true);
    }


    private void HideObject(EventManager.OnRestartGameEventArgs args)
    {
        _endGameTextObject.SetActive(false);
    }

    
    private void RepositionBoxesAtStart(EventManager.OnRestartGameEventArgs args)
    {
        for (int i = 0; i < _boxes.Length; i++)
        {
            _boxes[i].transform.position = _boxesStartPosition[i];
        }
    }


    private void CheckForWin()
    {
        if (_boxesToPushCount == 0)
        {
            SetGameEndBool(new EventManager.OnRestartGameEventArgs() { GameEnded = true });
            EventManager.Instance.StopGameTrigger();
            EventManager.Instance.WinGameTrigger("КРАСАВЧИК!");
        }
    }

    private void ResetBoxesToPushCount(EventManager.OnRestartGameEventArgs args)
    {
        _boxesToPushCount = _boxes.Length;
    }

    private void DecrementBoxesCount()
    {
        if (!GameEnded)
        {
            _boxesToPushCount--;
            CheckForWin();
        }
    }

    public void SetGameEndBool(EventManager.OnRestartGameEventArgs args)
    {
        if (args != null)
        {
            GameEnded = args.GameEnded;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnLoseGame -= ShowText;
        EventManager.Instance.OnWinGame -= ShowText;
        EventManager.Instance.OnRestartGame -= HideObject;
        EventManager.Instance.OnRestartGame -= RepositionBoxesAtStart;
        EventManager.Instance.OnBoxFell -= DecrementBoxesCount;
    }
}
