using UnityEngine;

public class StartButtonHandler : MonoBehaviour
{
    private Vector3 _buttonlocalScale;
    private void Start()
    {
        _buttonlocalScale = transform.localScale;
        EventManager.Instance.OnStartGame += HideButton;
    }

    private void HideButton(EventManager.OnStartEventArgs args)
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnStartGame -= HideButton;
    }
}
