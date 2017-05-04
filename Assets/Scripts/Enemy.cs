using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Enemy : MonoBehaviour {

    private NLGManager playerNlg;

    [SerializeField]
    private float enemySpeed = 10f; //This is the speed at which the enemy moves.

    [SerializeField]
    private float rangeRadius = 10f; //This is the distance to which the enemy walks before turning toward the other direction.

    [SerializeField]
    public Animator enemyAnim; //This is the animator responsible for animating the enemy.

    [SerializeField]
    public GameObject wBoxEnemy; //This is the enemy's weapon.

    [SerializeField]
    private GameObject playerPrefab; //This is the player in the scene.

    [SerializeField]
    private PlayerController playerScript; //This is the script responsible for controlling the player.

    private RespawnManager respawnScript; //This is the script responsible for bringing the player and the enemies back to life.

    public GameObject enemyDeathParticles; //This is the particleSystem that appears when the enemy dies.

    [SerializeField]
    private GameObject exitPointBoss; //This is the exit of a boss level; such exits are hidden by the game until the player defeats the boss.

    private Rigidbody2D enemyRgdBdy; //This is the RigidBody component of the enemy object.

    public float AtkRangeVert = 3; //This is the vertical range within which the enemy initiates an attack.
    public float AtkRangeHor = 5; //This is the horizontal range within which the enemy initiates an attack.

    [SerializeField]
    private bool isStandardBoss = false; //This Boolean keeps track of whether there is a typical boss in the scene.

    [SerializeField]
    private bool isRwKeepBoss = false; //This Boolean keeps track of whether the current boss is Nittonio, if there is a boss at all.

    [SerializeField]
    private bool isTfPalaceBoss = false; //This Boolean keeps track of whether the current boss is Qalem, if there is a boss at all.

    //The following two variables regard the enemy's movement.

    float RangeA; 
    float RangeB;

    //The following two variables allow for the relocation of the enemy if the enemy falls.

    float SpawnX;
    float SpawnY;

    //This is for flipping the enemy in the two-dimensional space.

    float dir = 1; //Have the enemy face right, at first.

    private AudioMaster audioMast; //This is the script in which one finds sound functionality for the enemy.

    public string deathNoise = "enemyDeath"; //This string is a reference to the noise the enemy makes upon death.

    //The following four Booleans are for determining when to attack the player.

    private bool shouldAtkRight;
    private bool shouldAtkLeft;
    private bool shouldAtkUp;
    private bool shouldAtkDown;

    void Start()
    {
        playerNlg = GameObject.Find("Player").GetComponent<NLGManager>();
        wBoxEnemy.SetActive(false);
        enemyRgdBdy = GetComponent<Rigidbody2D>();
        RangeA = transform.position.x + rangeRadius;
        RangeB = transform.position.x - rangeRadius;
        SpawnX = transform.position.x;
        SpawnY = transform.position.y;
        audioMast = AudioMaster.instance;
        respawnScript = GameObject.Find("mournerCircle").GetComponent<RespawnManager>();
    }

    void Update()
    {
        //Find values for each boolean during an instant.  These booleans are for the enemies' "intelligence" functionality.
        shouldAtkRight = dir == 1 && playerPrefab.transform.position.x <= transform.position.x + AtkRangeHor && playerPrefab.transform.position.x >= transform.position.x;
        shouldAtkLeft = dir == -1 && playerPrefab.transform.position.x >= transform.position.x - AtkRangeHor && playerPrefab.transform.position.x <= transform.position.x;
        shouldAtkUp = playerPrefab.transform.position.y <= transform.position.y + AtkRangeVert;
        shouldAtkDown = playerPrefab.transform.position.y >= transform.position.y - AtkRangeVert;
        if (Physics2D.IsTouching(playerScript.wBox.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>()) || transform.position.y < -30) //If either the enemy has contacted the player's sword or has fallen, kill the enemy.
        {
            StartCoroutine(killEnemy());
            playerNlg.killsDuringLevel++;
            playerNlg.killsDuringGame++;
        }
        strikeAI();
        enemyMvmt();
    }

    void enemyMvmt()
    {
        if (enemyRgdBdy.velocity.x <= 5 && enemyRgdBdy.velocity.x >= -5) //If the enemy's velocity is beneath a certain amount, end the running animation.
            enemyAnim.SetBool("isRunning", false);
        else //Otherwise, have the enemy run.
            enemyAnim.SetBool("isRunning", true);

        if (transform.position.x <= RangeB) //If the enemy has not reached the movement radius in the positive direction, perform the following.
        {
            dir = 1;
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f); //Have this be the current orientation of the enemy (Quaternions are responsible for rotation).
        }

        if (transform.position.x >= RangeA) //If the enemy has not reached the movement radius in the negative direction, perform the following.
        {
            dir = -1;
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f); //Have this be the current orientation of the enemy (Quaternions are responsible for rotation).
        }
        enemyRgdBdy.velocity = new Vector2(enemySpeed * dir, enemyRgdBdy.velocity.y); //The enemy moves in whatever direction is provided by the "dir" variable.
    }

    public IEnumerator killEnemy()
    {
        wBoxEnemy.SetActive(false); //Deactivate the enemy's attack.
        enemyAnim.SetBool("isAttacking", false);
        enemyAnim.SetBool("isDying", true);
        yield return new WaitForSeconds(.08f); //Give the fading animation some time.
        if (transform.position.y >= -30)
        {
            playerNlg.hasStabbed = true;
        }
        enemyAnim.SetBool("isDying", false);
        gameObject.SetActive(false); //Deactivate the entire gameObject.
        if (isStandardBoss) //If the player is in a typical boss level, perform the following.
        {
            playerNlg.hasKilledBoss = true;
            deathNoise = "bossDeath"; //Use this noise instead of the standard enemy death noise.
            //Instantiate the boss's death particles, and destroy them after 6 seconds.
            GameObject otherClone = Instantiate(enemyDeathParticles, new Vector2(transform.position.x, transform.position.y - 18), new Quaternion(0f, 0f, 0f, 0f)) as GameObject;
            otherClone.transform.rotation = Quaternion.Euler(270, 0, 0);
            Destroy(otherClone, 6f);
            exitPointBoss.SetActive(true);
            GameMaster.bossCount++;
        }
        else
        {
            //Instantiate the enemy's death particles, and destroy them after 3 seconds.
            GameObject otherClone = Instantiate(enemyDeathParticles, new Vector2(transform.position.x, transform.position.y - 1), new Quaternion(0f, 0f, 0f, 0f)) as GameObject;
            otherClone.transform.rotation = Quaternion.Euler(270, 0, 0);
            Destroy(otherClone, 3f);
            if (isTfPalaceBoss && GameObject.FindWithTag("Enemy") == null) //If the player is in the final scene and there are no more enemies, perform the following.
            {
                exitPointBoss.SetActive(true); //Activate the exit so that the player may return to the lobby.
                GameMaster.bossCount++; //Increment the 'bossCount' variable.
            }
            
        }
        transform.localPosition = new Vector2(SpawnX, SpawnY); //Have the enemy appear at its original location.
        audioMast.PlaySound(deathNoise);
    }
    
    public IEnumerator enemyStrike()
    {
        if (isRwKeepBoss) //If the player is near the first boss, perform the following.
        {
            GetComponent<BoxCollider2D>().enabled = false; //Deactivate the weak area of the boss when the boss attacks.
        }
        enemyAnim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.02f); //Give the animation some time.
        wBoxEnemy.SetActive(true);
        yield return new WaitForSeconds(0.14f); //Give the animation some time, and give the weapon box some time to catch the player.
        if (Physics2D.IsTouching(wBoxEnemy.GetComponent<BoxCollider2D>(), playerPrefab.GetComponent<PolygonCollider2D>())) //If the enemy's sword connects with the player's collider, perform the following.
        {
            wBoxEnemy.SetActive(false); //Don't kill the player multiple times.
            playerNlg.hasBeenStabbed = true;
            respawnScript.killPlayer(); //Kill the player.
            
        }
        wBoxEnemy.SetActive(false); //Deactivate the enemy's weapon box once the enemy's finished attacking.
        enemyAnim.SetBool("isAttacking", false);
        if (isRwKeepBoss) //If the player is near the first boss, perform the following.
        {
            GetComponent<BoxCollider2D>().enabled = true; //Reactivate the weak area when the boss is no longer attacking.
        }
    }

    void strikeAI()
    {
        if (shouldAtkUp && shouldAtkDown && (shouldAtkLeft || shouldAtkRight)) //If the player is within a certain area and the enemy is facing that area, attempt to hit the player.
        {
            StartCoroutine(enemyStrike());
        }
    }
}
