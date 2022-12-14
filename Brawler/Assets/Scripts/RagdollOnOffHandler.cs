using UnityEngine;

public class RagdollOnOffHandler : MonoBehaviour
{
    public CapsuleCollider MainCollider;
    public GameObject Rig;
    public Animator ModelAnimator;

    private void Start()
    {
        GetRagdollParts();
        RagdollModeOff(null);

        EventManager.Instance.OnRestartGame += RagdollModeOff;
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("FallZone"))
        {
            if (!GameManager.Instance.GameEnded)
            {
                GameManager.Instance.SetGameEndBool(new EventManager.OnRestartGameEventArgs() { GameEnded = true });
                EventManager.Instance.StopGameTrigger();

                EventManager.OnLoseGameEventArgs args = new EventManager.OnLoseGameEventArgs();
                args.Clip = SoundManager.Instance.FindClip("lose");
                args.endGameText = "YOU LOSE!";
                EventManager.Instance.LoseGameTrigger(args);
            }

            RagdollModeOn();
        }
    }

    private Collider[] _rdColliders;
    private Rigidbody[] _limbsRigidbodies;
    private void GetRagdollParts()
    {
        _rdColliders = Rig.GetComponentsInChildren<Collider>();
        _limbsRigidbodies = Rig.GetComponentsInChildren<Rigidbody>();
    }

    private void RagdollModeOn()
    {
        ModelAnimator.enabled = false;

        foreach (Collider collider in _rdColliders)
        {
            collider.enabled = true;
        }

        Vector3 mainVelocity = GetComponent<Rigidbody>().velocity;
        Vector3 rotation = GetComponent<Rigidbody>().angularVelocity;
        foreach (var rb in _limbsRigidbodies)
        {
            rb.isKinematic = false;
            rb.velocity = mainVelocity;
            rb.angularVelocity = rotation;
            rb.drag = 1.25f;
        }

        MainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void RagdollModeOff(EventManager.OnRestartGameEventArgs args)
    {
        foreach (Collider collider in _rdColliders)
        {
            collider.enabled = false;
        }

        foreach (var rb in _limbsRigidbodies)
        {
            rb.isKinematic = true;
        }

        ModelAnimator.enabled = true;
        MainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnRestartGame -= RagdollModeOff;
    }
}
