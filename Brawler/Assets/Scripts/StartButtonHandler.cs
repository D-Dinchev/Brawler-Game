using UnityEngine;

public class StartButtonHandler : MonoBehaviour
{
    private Vector3 _buttonlocalScale;
    private void Start()
    {
        _buttonlocalScale = transform.localScale;
        EventManager.Instance.OnStartGame += HideButton;
    }

    private void HideButton()
    {
        transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnStartGame -= HideButton;
    }
}
