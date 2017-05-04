using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //REMEMBER ENVIRONMENT LABELS, TAGS, LAYERS, ETC.

    [SerializeField]
    private float movementSpeed = 25f; //This is the speed at which the player moves.

    [SerializeField]
    private float boltSpeed = 50f; //This is the speed at which the player moves while the player holds the "shift" key.

    [SerializeField]
    private float jumpForce = 40f; //This is the vector with which the player jumps.

    [SerializeField]
    private float wallJumpForce = 30f; //This is the vertical vector with which the player leaps from a wall.

    [SerializeField]
    private float focusAmt = 100f; //This is the maximum amount of the resource responsible for the player's ability to bolt.

    public Animator playerAnim; //This is the animator responsible for the player's animations.

    [SerializeField]
    public GameObject wBox; //"W" is for weapon.  This box allows for the program to determine whether the player's sword has connected with an enemy.

    [SerializeField]
    private LayerMask gnd; //This is a reference to the "ground" layer.

    [SerializeField]
    private LayerMask walls; //This is a reference to the "walls" layer.

    private RectTransform boltBar; //This UI component is a visualization of how much focus the player has.

    public Rigidbody2D playerRgdBdy; //This is a reference to the RigidBody component of the "player" GameObject.

    private CircleCollider2D wallL; //This is a reference to the collider responsible for detecting whether there is a wall to the player's left.

    private CircleCollider2D wallR; //This is a reference to the collider responsible for detecting whether there is a wall to the player's right.

    private CircleCollider2D groundCircle; //This is a reference to the collider responsible for ground detection.

    private bool dirBool = true; //This Boolean is responsible for changing the direction of the player.

    private AudioMaster audioMast; //This is the script responsible for any in-game noises the player makes.

    //The following three strings are for referencing specific sounds.
    public string jumpNoise = "jump";
    public string landNoise = "groundHit";
    public string deathNoise = "playerDeath";

    private bool isGrounded; //This boolean determines whether the player is on the ground.

    private NLGManager nlgMan;

    private float boltUnusedForLongTime = 0;

    void Start()
    {
        //All statements in this method set the variables to the correct components of the correct GameObjects.
        boltBar = GameObject.Find("boltBar").GetComponent<RectTransform>();
        playerRgdBdy = GetComponent<Rigidbody2D>(); 
        wallL = GameObject.Find("wallLeft").GetComponent<CircleCollider2D>();  
        wallR = GameObject.Find("wallRight").GetComponent<CircleCollider2D>(); 
        groundCircle = GetComponent<CircleCollider2D>();
        nlgMan = GetComponent<NLGManager>();
        audioMast = AudioMaster.instance;
    }

    void Update() //This built-in method runs during each frame.
    {
        
        boltUnusedForLongTime++;
        if (nlgMan.hasStabbed && !groundCircle.IsTouchingLayers(gnd))
        {
            nlgMan.aerialKill = true;
        }
        if (boltUnusedForLongTime > 10000)
        {
            nlgMan.boltUnusedForLong = true;
        }
        if (groundCircle.IsTouchingLayers(walls) && !wallL.IsTouchingLayers(walls) && !wallR.IsTouchingLayers(walls)) //If the groundCircle touches the walls and neither wallL nor wallR touches the walls, perform the following.
        {
            playerRgdBdy.AddForce(new Vector2(0, jumpForce/4), ForceMode2D.Impulse); //Execute a small leap so that the player may not slide along horizontal segments of the wall.
            return; //Ensure that there are no especially high leaps.
        }
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isAttacking")) //If the player has attacked and is not attacking, already, perform the following.
        {
            playerAnim.SetBool("isSliding", false); //Stop the wall slide animation if the player is attacking.
            StartCoroutine(Strike()); //Begin the Strike( ) function.
        }
        boltBar.sizeDelta = new Vector2(focusAmt, 5); //Change the size of the boltBar given the focusAmt.
        movementAerial(); //Execute the movementAerial( ) function.
        if (playerRgdBdy.velocity.y < -5) //If the player's y-component velocity is less than -5 m/s, perform the following.
            playerAnim.SetBool("isFalling", true); //Begin the falling animation.
        else
            playerAnim.SetBool("isFalling", false); //Perform the jumping animation, instead.
    }

    void FixedUpdate() //This built-in method runs during each frame, but the interval between two of the method's calls is static.
    {
        bool wasGrounded = isGrounded; //This reference is for sound management, only.
        isGrounded = false; //By default, isGrounded is equal to false during every frame.
        movementGnd(); //Execute the movementGnd( ) function.
        if (playerRgdBdy.velocity.x < -0.3 && dirBool == true) //If the player moves to the left and dirBool is true, perform the following.
            changeDir(); //Change the direction in which the player faces.
        else if (playerRgdBdy.velocity.x > 0.3 && dirBool == false) //If the player moves to the right and dirBool is false, perform the following.
            changeDir(); //Change the direction in which the player faces.
        if (wasGrounded != isGrounded && isGrounded == true) //If the player has touched the ground during an instant, perform the following.
        {
            audioMast.PlaySound(landNoise); //Emit this sound via the "audio" script.
        }
    }

    void changeDir() //Change the direction in which the player faces.
    {
        Vector2 playerScale = transform.localScale; //Reference the scale of the object so that the object may reverse.
        if (dirBool == true) //If the player is facing right, perform the following.
        {
            //Change the positions of the wallR, wallL, and the player, accordingly.
            transform.position = new Vector2(transform.position.x - 0.7f, transform.position.y); // Keep the player from moving when the player flips.
            //wallL must always be to the player's left, and wallR must always be to the player's right.
            //The two colliders must swap positions.
            wallR.transform.localPosition = new Vector2(-0.37f, -0.4f); 
            wallL.transform.localPosition = new Vector2(-0.15f, -0.4f);
        }
        else
        {
            //Change the positions of the wallR, wallL, and the player, accordingly.
            transform.position = new Vector2(transform.position.x + 0.7f, transform.position.y); // Keep the player from moving when the player flips.
            //wallL must always be to the player's left, and wallR must always be to the player's right.
            //The two colliders must swap positions.
            wallR.transform.localPosition = new Vector2(-0.15f, -0.4f);
            wallL.transform.localPosition = new Vector2(-0.37f, -0.4f);
        }
        playerScale.x *= -1; //Invert the scale of the player.
        transform.localScale = playerScale; //Store the current scale of the player in the "playerScale" variable.
        dirBool = !dirBool; //Invert the 'dirBool' Boolean value.
    }

    void movementAerial() //This method is responsible for the player's aerial movement.
    {
        if (!groundCircle.IsTouchingLayers(gnd) && (wallL.IsTouchingLayers(walls) || wallR.IsTouchingLayers(walls))) //If the player is not touching the ground and either wallL or wallR touches a wall, perform the following.
        {
            playerAnim.SetBool("isSliding", true); //Begin the sliding animation.
        }
        else //By default, the player is neither jumping nor sliding.
        {
            playerAnim.SetBool("isJumping", false); //End the jumping animation.
            playerAnim.SetBool("isSliding", false); //End the sliding animation.
        }
        if (Input.GetKeyDown("w")) //If the player has pressed this key, perform the following.
        {
            if (groundCircle.IsTouchingLayers(gnd)) //If the player is touching the ground, perform the following.
            {
                audioMast.PlaySound(jumpNoise); //Emit the "jumpNoise."
                playerAnim.SetBool("isJumping", true); //begin the jumping animation.
                playerRgdBdy.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); //Add an upward impulse.
            }
            else
            {
                if (wallL.IsTouchingLayers(walls) && Input.GetAxis("Horizontal") != -1) //If wallL touches the wall and the player is not trying to move into the wall, perform the following.
                {
                    audioMast.PlaySound(jumpNoise); //Emit the "jumpNoise."
                    playerRgdBdy.AddForce(new Vector2(30, wallJumpForce), ForceMode2D.Impulse); //Add an upward, sideways impulse away from the wall. 
                }
                if (wallR.IsTouchingLayers(walls) && Input.GetAxis("Horizontal") != 1) //If wallR touches the wall and the player is not trying to move into the wall, perform the following.
                {
                    audioMast.PlaySound(jumpNoise); //Emit the "jumpNoise."
                    playerRgdBdy.AddForce(new Vector2(-30, wallJumpForce), ForceMode2D.Impulse); //Add an upward, sideways impulse away from the wall.
                }
            }
            return; //This forbids especially high leaps.
        }
    }

    void movementGnd() //This method is responsible for movement while the player is on the ground, excepting the functionality after the initial "else."
    {
        if (groundCircle.IsTouchingLayers(gnd)) //If the player's circle collider is touching the ground, perform the following.
        {
            nlgMan.inAir = 0;
            //By default, if the player is on the ground, the player is not running, and isGrounded is true.
            playerAnim.SetBool("isRunning", false);
            isGrounded = true;
            if (Input.GetAxis("Horizontal") != 0) //If the player attempts to move, perform the following.
            {
                playerAnim.SetBool("isRunning", true); //Begin the running animation.
                playerRgdBdy.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, playerRgdBdy.velocity.y); //Base the player's velocity on the player's input.
                if (focusAmt >= 3 && (Input.GetKey(KeyCode.LeftShift) == true || Input.GetKey(KeyCode.RightShift) == true)) //If the player's amount of focus is greater than or equal to 3 and the player has pressed the "shift" key, perform the following.
                {
                    nlgMan.boltUnusedForLong = false;
                    boltUnusedForLongTime = 0;
                    focusAmt -= 10; //Decrease the amount of focus, since the player is using it.
                    playerAnim.speed = 2; //Increase the speed of the animation, only.
                    playerRgdBdy.velocity = new Vector2(Input.GetAxis("Horizontal") * boltSpeed, playerRgdBdy.velocity.y); //Double the player's speed.
                }
                else if (focusAmt < 4)
                {
                    nlgMan.boltMeterEmpty = true;
                }
                playerAnim.speed = 1; //Have the animator return to its original speed.
            }
        }
        else //If the player fails to touch the ground, perform the following.
        {
            nlgMan.inAir++;
            if (playerRgdBdy.velocity.x <= movementSpeed && playerRgdBdy.velocity.x >= -movementSpeed) //If the player's velocity is less than movementSpeed, perform the following.
            {
                playerRgdBdy.AddForce(new Vector2(Input.GetAxis("Horizontal") * 30 * movementSpeed, 0f), ForceMode2D.Force); //Given input from the player, exert a force on the airborne player.
            }
            else //If the player's velocity is greater than movementSpeed, perform the following.
            {
                playerRgdBdy.AddForce(new Vector2(-(playerRgdBdy.velocity.x * 1.5f), 0f), ForceMode2D.Force); //Exert a slight force on the player (the force is opposite the current movement) so that the player does not continue to accelerate.
            }
            //The player is not running, but the player is jumping.
            playerAnim.SetBool("isRunning", false); //End the running animation.
            playerAnim.SetBool("isJumping", true); //Begin the jumping animation.
        }
        if (focusAmt < 100) //If the player's focus is beneath full capacity, perform the following.
            focusAmt += 2; //Replenish focus.
    }
    
    public IEnumerator Strike() //Perform an attack.
    {
        playerAnim.SetBool("isAttacking", true); //Begin the attack animation.
        yield return new WaitForSeconds(0.02f); //Wait for 0.02 seconds.
        wBox.SetActive(true); //Activate the weapon's collider, so that it may collide with an enemy.
        yield return new WaitForSeconds(0.14f); //Wait for 0.14 seconds.
        playerAnim.SetBool("isAttacking", false); //End the attack animation.
        wBox.SetActive(false); //Deactivate the weapon's collider.
    }
}