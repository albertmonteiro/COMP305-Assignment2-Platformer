using UnityEngine;
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
    public Transform camera;
    public GameController gameController;

    // PRIVATE  INSTANCE VARIABLES
    private Animator _animator;
    private float _move;
    private float _jump;
    private bool _facingRight;
    private Transform _transform;
    private Rigidbody2D _rigidBody2D;
    private bool _isGrounded;
    private AudioSource[] _audioSources;
    private AudioSource _jumpSound;
    private AudioSource _coinSound;
    private AudioSource _hurtSound;

    // Use this for initialization
    void Start()
    {
        // Initialize Public Instance Variables
        this.velocityRange = new VelocityRange(300f, 400f);

        // Initialize Private Instance Variables
        this._transform = gameObject.GetComponent<Transform>();
        this._animator = gameObject.GetComponent<Animator>();
        this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        this._move = 0f;
        this._jump = 0f;
        this._facingRight = true;

        // Setup AudioSources
        this._audioSources = gameObject.GetComponents<AudioSource>();
        this._jumpSound = this._audioSources[0];
        this._coinSound = this._audioSources[1];
        this._hurtSound = this._audioSources[2];

        // place the hero in the starting position
        this._spawn001();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPosition = new Vector3(this._transform.position.x, this._transform.position.y, -10f);
        this.camera.position = currentPosition;

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
            this._jumpSound.Play();
            if (absVelY < this.velocityRange.maximum)
            {
                forceY = this.jumpForce;
            }
            this._animator.SetInteger("AnimationState", 2);
        }


        this._rigidBody2D.AddForce(new Vector2(forceX, forceY));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Collision with a Bear event handler, gives the player 10 points
        if (other.gameObject.CompareTag("Bear"))
        {
            this._coinSound.Play();
            Destroy(other.gameObject);
            this.gameController.ScoreValue += 10;
        }

        // Falling off the platform event handler, takes away a life and respawns to start of game
        if (other.gameObject.CompareTag("Death001"))
        {
            this._spawn001();
            this._hurtSound.Play();
            this.gameController.LivesValue--;
        }

        // Falling off the platform event handler, takes away a life and respawns middle of game
        if (other.gameObject.CompareTag("Death002"))
        {
            this._spawn002();
            this._hurtSound.Play();
            this.gameController.LivesValue--;
        }

        // Falling off the platform event handler, takes away a life and respawns to 3rd point in game
        if (other.gameObject.CompareTag("Death003"))
        {
            this._spawn003();
            this._hurtSound.Play();
            this.gameController.LivesValue--;
        }

        // Collision with a monster event handler, takes away a life and respawns to start of game
        if (other.gameObject.CompareTag("Monster001"))
        {
            this._spawn001();
            this._hurtSound.Play();
            Destroy(other.gameObject);
            this.gameController.LivesValue--;
        }

        // Collision with a monster event handler, takes away a life and respawns to 3rd point in game
        if (other.gameObject.CompareTag("Monster002"))
        {
            this._spawn003();
            this._hurtSound.Play();
            Destroy(other.gameObject);
            this.gameController.LivesValue--;
        }

        // Collision with a Bear event handler, gives the player 50 points
        if (other.gameObject.CompareTag("Trophy"))
        {
            this.gameController.ScoreValue += 50;
            this._coinSound.Play();
            Destroy(other.gameObject);
            gameController._endGame();
        }
    }

    // Flips the player right to left and vice-versa
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

    // Spawns to start of game
    private void _spawn001()
    {
        this._transform.position = new Vector3(-130f, -111f, 0);
    }

    // Spawns to mid of game
    private void _spawn002()
    {
        this._transform.position = new Vector3(2175f, -176f, 0);
    }

    // Spawns to end of game
    private void _spawn003()
    {
        this._transform.position = new Vector3(3365f, -113f, 0);
    }

}
