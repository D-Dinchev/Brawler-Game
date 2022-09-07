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


    private void ShowText(EventManager.OnLoseGameEventArgs args)
    {
        _endGameText.text = args.endGameText;
        _endGameTextObject.SetActive(true);
    }

    private void ShowText(EventManager.OnWinGameEventArgs args)
    {
        _endGameText.text = args.endGameText;
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
            var box = _boxes[i];
            Rigidbody rb = box.GetComponent<Rigidbody>();

            box.transform.position = _boxesStartPosition[i];
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
            box.transform.rotation = Quaternion.identity;
        }
    }


    private void CheckForWin()
    {
        if (_boxesToPushCount == 0)
        {
            SetGameEndBool(new EventManager.OnRestartGameEventArgs() { GameEnded = true });
            EventManager.Instance.StopGameTrigger();

            EventManager.OnWinGameEventArgs args = new EventManager.OnWinGameEventArgs();
            args.Clip = SoundManager.Instance.FindClip("win");
            args.endGameText = "YOU WIN!";
            EventManager.Instance.WinGameTrigger(args);
        }
    }

    private void ResetBoxesToPushCount(EventManager.OnRestartGameEventArgs args)
    {
        _boxesToPushCount = _boxes.Length;
    }

    private void DecrementBoxesCount(EventManager.OnBoxFellEventArgs args)
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
