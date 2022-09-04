using System.Collections;
using UnityEngine;

public class RestartButtonHandler : MonoBehaviour
{
    private Vector3 _buttonlocalScale;

    private void Start()
    {
        _buttonlocalScale = transform.localScale;

        HideButton(null);

        EventManager.Instance.OnStopGame += ShowButton;
        EventManager.Instance.OnRestartGame += HideButton;
    }

    private void HideButton(EventManager.OnRestartGameEventArgs args)
    {
        transform.localScale = Vector3.zero;
    }

    private void ShowButton()
    {
        StartCoroutine(ShowButtonCoroutine());
    }
    private IEnumerator ShowButtonCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        transform.localScale = _buttonlocalScale;

    }

    private void OnDestroy()
    {
        EventManager.Instance.OnStopGame -= ShowButton;
        EventManager.Instance.OnRestartGame -= HideButton;
    }
}
