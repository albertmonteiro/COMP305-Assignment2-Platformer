﻿using UnityEngine;
using System.Collections;

// VELOCITY RANGE UTILITY CLASS ++++++++++++++++++++++++++++
[System.Serializable]
public class VelocityRange
{
    // PUBLIC INSTANCE VARIABLES ++++
    public float minimum;
    public float maximum;

    // CONSTRUCTOR ++++++++++++++++++++++++++++++++++++
    public VelocityRange(float minimum, float maximum)
    {
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

    // PRIVATE  INSTANCE VARIABLES
    private Animator _animator;
    private float _move;
    private float _jump;
    private bool _facingRight;
    private Transform _transform;
    private Rigidbody2D _rigidBody2D;
    private bool _isGrounded;

    // Use this for initialization
    void Start()
    {
        // Initialize Public Instance Variables
        this.velocityRange = new VelocityRange(300f, 500f);

        // Initialize Private Instance Variables
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
        this._isGrounded = Physics2D.Linecast(
            this._transform.position,
            this.groundCheck.position,
            1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(this._transform.position, this.groundCheck.position);




        float forceX = 0f;
        float forceY = 0f;

        // get absolute value of velocity for our gameObject
        float absVelX = Mathf.Abs(this._rigidBody2D.velocity.x);
        float absVelY = Mathf.Abs(this._rigidBody2D.velocity.y);

        //// Ensure the player is grounded before any movement checks
        //if (this._isGrounded)
        //{
        //    // gets a number between -1 to 1 for both Horizontal and Vertical Axes
        //    this._move = Input.GetAxis("Horizontal");
        //    this._jump = Input.GetAxis("Vertical");

        //    if (this._move != 0)
        //    {
        //        if (this._move > 0)
        //        {
        //            // movement force
        //            if (absVelX < this.velocityRange.maximum)
        //            {
        //                forceX = this.moveForce;
        //            }
        //            this._facingRight = true;
        //            this._flip();
        //        }
        //        if (this._move < 0)
        //        {
        //            // movement force
        //            if (absVelX < this.velocityRange.maximum)
        //            {
        //                forceX = -this.moveForce;
        //            }
        //            this._facingRight = false;
        //            this._flip();
        //        }

        //        // call the walk clip
        //        this._animator.SetInteger("AnimationState", 1);
        //    }
        //    else
        //    {

        //        // set default animation state to "idle"
        //        this._animator.SetInteger("AnimationState", 0);
        //    }

        //    if (this._jump > 0)
        //    {
        //        // jump force
        //        if (absVelY < this.velocityRange.maximum)
        //        {
        //            forceY = this.jumpForce;
        //        }

        //    }
        //}
        //else
        //{
        //    // call the "jump" clip
        //    this._animator.SetInteger("AnimationState", 2);
        //}

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // DOES NOT Ensure the player is grounded before any movement checks
        //if (this._isGrounded)
        //{
            // gets a number between -1 to 1 for both Horizontal and Vertical Axes
            this._move = Input.GetAxis("Horizontal");
            this._jump = Input.GetAxis("Vertical");

            if (this._move != 0)
            {
                if (this._move > 0)
                {
                    // movement force
                    if (absVelX < this.velocityRange.maximum)
                    {
                        forceX = this.moveForce;
                    }
                    this._facingRight = true;
                    this._flip();
                }
                if (this._move < 0)
                {
                    // movement force
                    if (absVelX < this.velocityRange.maximum)
                    {
                        forceX = -this.moveForce;
                    }
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

            if (this._jump > 0 && this._isGrounded)
            {
                // jump force
                if (absVelY < this.velocityRange.maximum)
                {
                    forceY = this.jumpForce;
                }
                this._animator.SetInteger("AnimationState", 2);
            }
        //}
        //else
        //{
            // call the "jump" clip
        //}
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++


        Debug.Log(forceX);
        // Apply the forces to the player
        this._rigidBody2D.AddForce(new Vector2(forceX, forceY));
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
