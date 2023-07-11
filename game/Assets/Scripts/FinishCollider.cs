using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishCollider : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip victorySFX;
    
    // Start is called before the first frame update
    public UnityEvent onFinish;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(victorySFX, 0.7f);
            onFinish.Invoke();
            other.gameObject.GetComponent<CharacterController2D>().Finish();
        }
    }
}
