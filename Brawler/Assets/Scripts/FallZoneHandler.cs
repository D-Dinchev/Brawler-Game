using UnityEngine;

public class FallZoneHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Box") && !GameManager.Instance.GameEnded)
        {
            EventManager.OnBoxFellEventArgs args = new EventManager.OnBoxFellEventArgs();
            args.Clip = SoundManager.Instance.FindClip("box_out");
            EventManager.Instance.BoxFellTrigger(args);
        }
    }
}
