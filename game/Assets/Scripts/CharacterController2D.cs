using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : AutoMonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]
	

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public bool possess;

	public Gaming gaming;

	public bool dead => gaming.isDead;
	public bool dying;
	public UnityEvent OnDie;
	public bool frozen = false;

	private GameObject toBePossessed;
	

	public AudioSource audioSource;
	public AudioClip dieSFX;

	public float possessionDelay = 0.35f;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		gaming = FindObjectOfType<Gaming>();
	}

	private void FixedUpdate()
	{
		this.GetComponent<Animator>().SetFloat("velocity", Mathf.Abs(m_Rigidbody2D.velocity.x));
		if (frozen)
		{
			return;
		}

		if (dead)
		{
			Freeze();
			Die();
			return;
		}
		

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
				if (colliders[i].gameObject.CompareTag("Possessable") && possess)
				{
					//grounded possess
					toBePossessed = colliders[i].gameObject;
					this.GetComponent<Animator>().SetTrigger("possess");
					Freeze();
					this.WaitThen(possessionDelay, () => {
						Possess(toBePossessed);
						gaming.isPossessing = true;
						gaming.possessed = toBePossessed.GetComponentInParent<Possessable>();
						possess = false;
					});

					

				}
			}
		}
	}


	public void Move(float move, bool crouch, bool jump, bool possess)
	{
		if (dead) return;
		this.possess = possess;
		//Debug.Log("mfw i am moving");
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			Collider2D overlappingCeiling = Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround);
			if (overlappingCeiling)
			{
				crouch = true;
				if (overlappingCeiling.gameObject.CompareTag("Possessable"))
				{
					
					//air possess
					Freeze();
					this.WaitThen(possessionDelay, () => {
						Possess(overlappingCeiling.gameObject);
						gaming.isPossessing = true;
						gaming.possessed = overlappingCeiling.gameObject.GetComponentInParent<Possessable>();
						possess = false;
					});

				}
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			//Debug.Log("mfw i am grounded or aircontrol");
			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			
			
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			//Debug.Log("mfw i jump");
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void Possess(GameObject obj)
	{
		obj.GetComponentInParent<Possessable>().enabled = true;
		gaming.camFollow.SetAnchor(obj.transform);
		this.gameObject.SetActive(false);
	}

	public void Die()
	{
		OnDie.Invoke();
		audioSource.PlayOneShot(dieSFX, 0.7f);
		Debug.Log("oh no I fucking died in real life");
		this.GetComponent<Animator>().SetTrigger("die");
		this.WaitThen(0.75f, () =>
		{
			this.gameObject.SetActive(false);
		});
	}

	public void Freeze()
	{
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		frozen = true;
	}
	
	public void Unfreeze() {
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		frozen = false;
	}

	public void Finish()
	{
		Freeze();
		this.GetComponent<Animator>().SetTrigger("celebrate_trigger");
	}
}