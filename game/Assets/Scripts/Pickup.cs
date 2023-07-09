using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update

    public string id;
    private Gaming _gaming;
    void Start()
    {
        _gaming = FindObjectOfType<Gaming>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gaming.OnPickUp(id);
            this.gameObject.SetActive(false);
        }
    }
    
}
