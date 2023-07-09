using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private Gaming _gaming;
    void Start()
    {
        _gaming = FindObjectOfType<Gaming>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gaming.isDead = true;
        }
    }
}
