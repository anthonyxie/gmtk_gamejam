using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHelper : MonoBehaviour
{
    public Behaviour toDisable;

    public void DoDisable() {
        toDisable.enabled = false;
    }
}
