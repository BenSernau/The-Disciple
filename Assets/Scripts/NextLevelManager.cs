using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelManager : MonoBehaviour {

    public Transform exitPoint;

    public string levelToLoad;

    private PlayerController playerScript;

    private Animator levelFadeAnim;

    private AudioMaster audioMast;

    private NLGManager nlgMan;

	// Use this for initialization
	void Start () {
        levelFadeAnim = GameObject.Find("LevelFade").GetComponent<Animator>();
        audioMast = AudioMaster.instance;
        exitPoint = GameObject.FindWithTag("Exit").GetComponent<Transform>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        nlgMan = playerScript.gameObject.GetComponent<NLGManager>();
	}

	void Update () {
        nlgMan.distFromGoal = Mathf.Sqrt(Mathf.Pow(playerScript.gameObject.transform.position.x -
                                gameObject.transform.position.x, 2f) +
                                Mathf.Pow(playerScript.gameObject.transform.position.y -
                                gameObject.transform.position.y, 2f)); //PYTHSWAGOREAN THEOREM
        if (exitPoint != null && playerScript.gameObject.transform.position.x <= exitPoint.position.x + 3 && 
            playerScript.gameObject.transform.position.x >= exitPoint.position.x - 3 && 
            playerScript.gameObject.transform.position.y <= exitPoint.position.y + 3 && 
            playerScript.gameObject.transform.position.y >= exitPoint.position.y - 3) //If the player has reached the exit, perform the following.
        {
            StartCoroutine(NextScene());
        }
    }

    public IEnumerator NextScene()
    {
        nlgMan.deathsDuringLevel = 0;
        nlgMan.killsDuringLevel = 0;
        levelFadeAnim.SetBool("isOver", true); //Begin the screen-darkening animation.
        yield return new WaitForSeconds(0.12f); //Give the animation some time.
        audioMast.StopAll(); //Don't allow noises to bleed into the next scene.
        SceneManager.LoadScene(levelToLoad);
        levelFadeAnim.SetBool("isOver", false); //End the animation.
    }
}
