using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public Follow camFollow;
    public bool isPossessing;
    public Possessable possessed = null;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceUnpossess()
    {
        Debug.Log("mfw I am being forced to unpossussy");
        if (possessed)
        {
            isPossessing = false;
            possessed.Unpossess();
        }
    }
}
