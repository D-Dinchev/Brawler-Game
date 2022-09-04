using UnityEngine;

public class FallZoneHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Box") && !GameManager.Instance.GameEnded)
        {
            EventManager.Instance.BoxFellTrigger();
        }
    }
}
