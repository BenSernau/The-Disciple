using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour {

    private bool playerKilled;

    private int quickSuccessionStabsCount;

    private int quickSuccessionStabsTime;

    private int quickSuccessionFallsCount;

    private int quickSuccessionFallsTime;

    private PlayerController playerScript;

    private NLGManager playerNlg;

    private AudioMaster audioMast;

    private GameObject[] enemyGameObjects;

    private Text deathTextAppear;

    private GameObject spawnPoint;

    [SerializeField]
    private GameObject respawnParticles;

    void Start () {
        quickSuccessionFallsCount = 0;
        quickSuccessionFallsTime = 0;
        quickSuccessionStabsCount = 0;
        quickSuccessionStabsTime = 0;
        deathTextAppear = GameObject.Find("DeathTextAppear").GetComponent<Text>();
        audioMast = AudioMaster.instance;
        enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerNlg = GameObject.Find("Player").GetComponent<NLGManager>();
        spawnPoint = gameObject; //reference the spawn location
    }

    void Update()
    {
        deathTextAppear.text = playerNlg.S;
    }
	
	void LateUpdate () {
        quickSuccessionStabsTime++;
        quickSuccessionFallsTime++;
        if (playerNlg.hasBeenStabbed)
        {
            if (quickSuccessionStabsTime < 500)
            {
                quickSuccessionStabsCount++;
            }
            if (quickSuccessionStabsCount >= 3)
            {
                quickSuccessionStabsCount = 0;
                playerNlg.quickSuccessionStabs = true;
            }
            quickSuccessionStabsTime = 0;
        }

        if (playerScript.gameObject.transform.position.y < -30)
        {
            playerNlg.hasFallen = true;
            if (quickSuccessionFallsTime < 240)
            {
                quickSuccessionFallsCount++;
            }
            if (quickSuccessionFallsCount >= 3)
            {
                quickSuccessionFallsCount = 0;
                playerNlg.quickSuccessionFalls = true;
            }
            quickSuccessionFallsTime = 0;
            killPlayer();
        }
    }

    public void killPlayer()
    {
        playerNlg.deathsDuringLevel++;
        playerNlg.deathsDuringGame++;
        playerScript.wBox.SetActive(false); //Deactive the player's weapon collider.
        playerScript.playerAnim.SetTrigger("hasDied"); //Begin the death/fade animation.
        GameObject clone = Instantiate(respawnParticles, new Vector2(playerScript.gameObject.transform.position.x, playerScript.gameObject.transform.position.y - 1), new Quaternion(0f, 0f, 0f, 0f)) as GameObject; //Instantiate particles upon death.
        clone.transform.rotation = Quaternion.Euler(270, 0, 0); //Ensure correct direction of particle system.
        Destroy(clone, 2f); //Destroy the particle system to avoid lag.
        GameObject clone2 = Instantiate(respawnParticles, new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 2), new Quaternion(0f, 0f, 0f, 0f)) as GameObject; //Instantiate particles upon respawn.
        clone2.transform.rotation = Quaternion.Euler(270, 0, 0); //Ensure correct direction of particle system.
        Destroy(clone2, 2f); //Destroy the particle system to avoid lag.
        playerScript.gameObject.transform.localPosition = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 3);
        playerScript.playerRgdBdy.velocity = new Vector2(0f, 2f); //Have the main character "pop out" of his fade-in, giving a slight, elevating jolt.
        if (!(SceneManager.GetActiveScene().name == "tfPalaceBoss" && GameObject.FindWithTag("Enemy") == null)) //If the enemy is a boss, do not respawn.
        {
            StartCoroutine(ensureRespawn());
        }
        audioMast.PlaySound(playerScript.deathNoise);
    }

    public void respawnEnemies() //Respawn the enemies after the player has died.
    {
        for (int i = 0; i < enemyGameObjects.Length; i++) //Activate each of the scene's enemies.
        {
            enemyGameObjects[i].SetActive(true); //Activate enemy, 'i.'
            enemyGameObjects[i].GetComponent<Enemy>().wBoxEnemy.SetActive(false); //Make sure the weapon colliders are inactive.
        }
    }

    public IEnumerator ensureRespawn() //With regard to parries, if Unity fails to wait for a fraction of a second via this specific function, the enemy against whom the player parries fails to respawn.
    {
        yield return new WaitForSeconds(0.05f);
        respawnEnemies();
    }
}
