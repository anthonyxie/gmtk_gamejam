using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    
    public float distance = 3;
    public float thresh = 0.5f;
    public Rigidbody2D platform;

    public float speed = 10;
    private Gaming gaming;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //dummy platform move 
        float jumpAmount = 25;

        //platform.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * speed;
        Vector2 inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        platform.velocity = inputVelocity * speed;

        Vector2 vec = -platform.transform.localPosition;
        float dist = vec.magnitude;
        float fac = Mathf.Clamp(dist - distance, 0, thresh) / thresh;
        platform.velocity += vec.normalized * fac * speed;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Unpossess();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Transform player = gaming.player.gameObject.transform;
            Unpossess();
            player.GetComponent<Rigidbody2D>().AddForce(inputVelocity * jumpAmount, ForceMode2D.Impulse);
        }
    }

    void OnEnable()
    {
        platform.constraints = RigidbodyConstraints2D.FreezeRotation;
        gaming = FindObjectOfType<Gaming>();
    }

    public void Unpossess()
    {
        platform.constraints = RigidbodyConstraints2D.FreezeAll;
        Debug.Log("mfw i unpossussy");
        Transform player = gaming.player.gameObject.transform;
        player.position = platform.transform.position + Vector3.up*0.2f;
        player.gameObject.GetComponent<CharacterController2D>().possess = false;
        player.gameObject.SetActive(true);
        gaming.camFollow.SetAnchor(player);
        gaming.isPossessing = false;
        gaming.possessed = null;
        this.enabled = false;
        
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
