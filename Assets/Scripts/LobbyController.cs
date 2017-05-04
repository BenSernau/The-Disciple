using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour {

    private PlayerController playerScript;

    private NextLevelManager nextLevelScript;

    private Transform twiVal;
    private Transform shrWoods;
    private Transform nmPass;

    [SerializeField]
    private GameObject gateClosed;
    
    [SerializeField]
    private GameObject gateOpen;

    [SerializeField]
    private GameObject finalAssistText;

	void Start () {
        if (GameMaster.bossCount >= 4 && GameMaster.bossCount < 6) //If the first four bosses have been killed, open the lobby's gate.
        {
            gateClosed.SetActive(false);
            gateOpen.SetActive(true);
        }
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        nextLevelScript = GameObject.FindWithTag("Exit").GetComponent<NextLevelManager>();
        twiVal = GameObject.Find("mournerCircleExitTwiVal").GetComponent<Transform>();
        shrWoods = GameObject.Find("mournerCircleExitShrWoods").GetComponent<Transform>();
        nmPass = GameObject.Find("mournerCircleExitnmPass").GetComponent<Transform>();
    }
	
	void Update () {
        if (GameMaster.bossCount >= 6) //If the player has completed the game, deactivate the lobby's exit, and activate the final message.
        {
            finalAssistText.SetActive(true);
            nextLevelScript.exitPoint.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown("s")) //When the player presses this key, perform the following.
        {
            if (GameMaster.mtwivalbool && playerScript.gameObject.transform.position.x < twiVal.position.x + 4 && playerScript.gameObject.transform.position.x > twiVal.position.x - 4) //If the player is near the leftmost door, perform the following.
            {
                nextLevelScript.levelToLoad = "twiValAlef"; //Have the next level be the first part of Twilight Valley.
                GameMaster.mtwivalbool = false; //The player may not reenter the area.
                StartCoroutine(nextLevelScript.NextScene()); //Begin the next level.
            }
            else if (GameMaster.mshrwoodsbool && playerScript.gameObject.transform.position.x < shrWoods.position.x + 4 && playerScript.gameObject.transform.position.x > shrWoods.position.x - 4) //If the player is near the middle door, perform the following.
            {
                nextLevelScript.levelToLoad = "shrWoodsAlef"; //Have the next level be the first part of Shrieking Woods.
                GameMaster.mshrwoodsbool = false; //The player may not reenter the area.
                StartCoroutine(nextLevelScript.NextScene()); //Begin the next level.
            }
            else if (GameMaster.mnmpassbool && playerScript.gameObject.transform.position.x < nmPass.position.x + 4 && playerScript.gameObject.transform.position.x > nmPass.position.x - 4) //If the player is near the rightmost door, perform the following.
            {
                nextLevelScript.levelToLoad = "nmPassAlef"; //Have the next level be the first part of No Man's Pass.
                GameMaster.mnmpassbool = false; //The player may not reenter the area.
                StartCoroutine(nextLevelScript.NextScene()); //Begin the next level.
            }
        }
    }
}
