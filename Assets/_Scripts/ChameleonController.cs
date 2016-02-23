using UnityEngine;
using System.Collections;

// VELOCITY RANGE UTILITY CLASS ++++++++++++++++++++++++++++
[System.Serializable]
public class VelocityRange {
    //PUBLIC INSTANCE VARIABLES
    public float minimum;
    public float maximum;

    // CONSTRUCTOR
    public VelocityRange(float minimum, float maximum){
        this.minimum = minimum;
        this.maximum = maximum;
    }
}

public class ChameleonController : MonoBehaviour
{
    // PUBLIC INSTANCE VARIABLES
    public VelocityRange velocityRange;
    public float moveForce;
    public float jumpForce;
    public Transform groundCheck;

    // PRIVATE INSTANCE VARIABLES
    private Animator _animator;
    private float _move;
    private float _jump;
    private bool _facingRight = true;
    private Transform _transform;
    private Rigidbody2D _rigidBody2D;
    private bool _isGrounded;

    // Use this for initialization
    void Start()
    {
        // Initialize public instance variables
        this.velocityRange = new VelocityRange(300f, 1000f);
        this.moveForce = 50f;
        this.jumpForce = 500f;

        // Initial private instance variables
        this._transform = gameObject.GetComponent<Transform>();
        this._animator = gameObject.GetComponent<Animator>();
        this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        this._move = 0f;
        this._jump = 0f;
        this._facingRight = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this._isGrounded = Physics2D.Linecast(this._transform.position, this.groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(this._transform.position, this.groundCheck.position);

        

        float forceX = 0f;
        float forceY = 0f;

        // Absolute value of velocity for gameObject
        float absVelX = Mathf.Abs(this._rigidBody2D.velocity.x);
        float absVelY = Mathf.Abs(this._rigidBody2D.velocity.y);


        // Chacking if character is grounded
        if (this._isGrounded)
        {
            Debug.Log("Grounded");
            // gets a number between -1 and 1 for both horizontal and vertical axes
            this._move = Input.GetAxis("Horizontal");
            this._jump = Input.GetAxis("Vertical");
            if (this._move != 0)
            {
                if (this._move > 0)
                {
                    this._facingRight = true;
                    this._flip();
                }
                if (this._move < 0)
                {
                    this._facingRight = false;
                    this._flip();
                }

                // call the walk clip
                this._animator.SetInteger("AnimationState", 1);
            }
            else
            {

                // set default animation state to "idle"
                this._animator.SetInteger("AnimationState", 0);
            }
        }
        else
        {
            Debug.Log("Not Grounded");
            if (this._jump > 0)
            {
                // call the "jump" clip
                this._animator.SetInteger("AnimationState", 2);
            }
        }

        
    }

    // PRIVATE METHODS
    private void _flip()
    {
        if (this._facingRight)
        {
            this._transform.localScale = new Vector2(1, 1);
        }
        else
        {
            this._transform.localScale = new Vector2(-1, 1);
        }
    }
}
