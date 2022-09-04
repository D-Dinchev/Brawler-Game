using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _playerTransform;

    [SerializeField]
    private Vector3 _offset;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        transform.position = _playerTransform.position + _offset;
    }
}
