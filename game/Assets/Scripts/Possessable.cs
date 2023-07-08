using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    
    public float distance = 3;
    public float thresh = 0.5f;
    public Rigidbody2D platform;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //dummy platform move

        platform.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        Vector2 vec = -platform.transform.localPosition;
        float dist = vec.magnitude;
        float fac = Mathf.Clamp(dist - distance, 0, thresh) / thresh;
        platform.velocity += vec.normalized * fac * speed;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Unpossess();
        }
    }

    void Unpossess()
    {
        Transform player = GameObject.FindObjectOfType<PlayerController>().transform;
        player.position = platform.transform.position + Vector3.up*0.2f;
        player.gameObject.SetActive(true);
        this.enabled = false;
    }
}
