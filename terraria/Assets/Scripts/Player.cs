using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float clibimgSpeed = 8f;
    [SerializeField] float attackRadius = 3f;
    [SerializeField] Vector2 hitKick = new Vector2(50f, 50f);
    [SerializeField] Transform hurtBox;
    [SerializeField] AudioClip jumpingSFX, attackingSFX, gettingHitSFX, walkingSFX;

    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersfeet;
    AudioSource myAudioSource;

    float staringGravityScale;
    bool isHurting = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersfeet = GetComponent<PolygonCollider2D>();
        myAudioSource = GetComponent<AudioSource>();

        staringGravityScale = myRigidBody2D.gravityScale;

        myAnimator.SetTrigger("Appearing");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHurting){
            Run();
            Jump();
            Climb();
            Attack();
            if(myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy"))){
                PlayerHit();
            }

            ExitLevel();
        }
    }

    private void ExitLevel(){

        if (!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Interactable"))) {return;}
        if(CrossPlatformInputManager.GetButtonDown("Vertical")){
            myAnimator.SetTrigger("Dissapearing");
        }

    }

    public void LoadNextLevel(){
        FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        TurnOffRenderer();
    }

    public void TurnOffRenderer(){
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Attack(){
        bool isAttaking = CrossPlatformInputManager.GetButtonDown("Fire1");
        if(isAttaking){
            myAnimator.SetTrigger("Attacking");
            myAudioSource.PlayOneShot(attackingSFX);

            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

            foreach (Collider2D enemy in enemiesHit)
            {
                enemy.GetComponent<Enemy>().Dying();
            }
        }
    }

    public void PlayerHit(){
        myRigidBody2D.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);

        myAnimator.SetTrigger("Hitting");
        myAudioSource.PlayOneShot(gettingHitSFX);
        isHurting = true;

        FindObjectOfType<GameSession>().ProcessPlayerDeath();

        StartCoroutine(StopHurting());
    }

    IEnumerator StopHurting(){
        yield return new WaitForSeconds(2f);
        isHurting = false;
    }

    private void Climb(){
        if(myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
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

            myAudioSource.PlayOneShot(jumpingSFX);
        }
    }

    private void Run(){
    // if you are seeing jerkiness in character moves, set option Interpolate in rigidboy2d 
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
         
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = playerVelocity;

        FlipSprite();
        ChangeRunningState();
    }

    void StepsSFX(){
        bool playerMovinHorizontally = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        if(playerMovinHorizontally){
            if(myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))){
                myAudioSource.PlayOneShot(walkingSFX);
            }else{
                myAudioSource.Stop();
            }
        }
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

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }    
}
