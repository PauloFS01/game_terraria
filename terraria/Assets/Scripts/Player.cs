using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float clibimgSpeed = 8f;

    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D MyBoxCollider2D;
    PolygonCollider2D myPlayersfeet;

    float staringGravityScale;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        MyBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersfeet = GetComponent<PolygonCollider2D>();

        staringGravityScale = myRigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        Climb();
    }

    private void Climb(){
        if(MyBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbingVelocity = new Vector2(myRigidBody2D.velocity.x, controlThrow * clibimgSpeed);
            myRigidBody2D.velocity = climbingVelocity;

            myAnimator.SetBool("Climb", true);

            myRigidBody2D.gravityScale = 0f;
        }else
        {
            myAnimator.SetBool("Climb", false);
            myRigidBody2D.gravityScale = staringGravityScale;
        }
    }

    private void Jump(){
        if(!myPlayersfeet.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        if(isJumping){
            Vector2 jumpVelocity = new Vector2(myRigidBody2D.velocity.x, jumpSpeed);
            myRigidBody2D.velocity = jumpVelocity;
        }
    }

    private void Run(){
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
         
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = playerVelocity;

        FlipSprite();
        ChangeRunningState();
    }

    private void ChangeRunningState (){
        bool runningHorizontaly = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", runningHorizontaly);
    }

    private void FlipSprite()
    {
        bool runningHorizontaly = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        if (runningHorizontaly)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }
    }
}
