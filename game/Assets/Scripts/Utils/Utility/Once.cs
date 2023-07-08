using UnityEngine;
using UnityEngine.Events;

public class Once : AutoMonoBehaviour
{
    public UnityEvent onTrigger;

    public float delay = 0f;

    [Header("Debug")]
    [SerializeField, ReadOnly]
    private bool _didTrigger;

    public void Trigger() {
        if (_didTrigger) return;

        _didTrigger = true;
        if (delay == 0) {
            onTrigger.Invoke();
        }
        else {
            this.WaitThen(delay, () => {
                onTrigger.Invoke();
            });
        }
    }
}