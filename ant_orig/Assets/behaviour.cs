using UnityEngine;
using System.Collections;
using TMPro;


public class behaviour : MonoBehaviour
{

    public GameObject text;
    public GameObject startText;
    public GameObject tmp;
    private float displayTime;
    private bool showText;
    private bool showGui;
    public Material newMaterial1;
    public Material newMaterial2;
    public Material newMaterial3;
    public Material newMaterial4;
    public Material newMaterial5;
    public Material newMaterial6;
    public Material newMaterial7;
    public Material newMaterial8;
    private Material[] materials;
    private bool hasFood;
    private GameObject food;
    public Material RedArrow;
    public Material GreenArrow;
    public GameObject nest;
    public GameObject compass;
    private int foodCount;
    private bool tornadoZone;
    public GameObject dust;
    public bool gameOver;
    public KidnappedRobot krobot;

    private int currentState;
    public TextMeshPro TextMeshP;
    public GameObject PlayerText;

    public GameObject PlayerCharacter;

	private bool challengeDone;
	private float angleToPosition;

	private bool antEyeRunning;

	private float lastAnglePosition;

	public Brightness BrightnessRef; 

    public GameObject secondCupcake;
    public Transform spawnPoint;

    public GameObject thirdCupcake;
    public Transform spawnPoint2;

    private Color guessColor;

    GameObject physicalNarrator;
    AudioSource narratorAudio;
    GameObject player;
    Rigidbody playerRigidBody;
    OVRPlayerController controller;

    // 0 SearchFreelyForFood
    // 1 Food found and bring it back home
    // 2 Get another one
    // 3 Return home before Kidnapped Robot
    // 4 After Kidnapped Robot get back home exp 1
    // 5 Get more food using vector
    // 6 Return home using vector
    // 7 After Kidnapped Robot get back home exp 2
    // 8 Get more food using vector
    // 9 Return home using vector
    // 10 After Kidnapped Robot get back home exp 3


    // Use this for initialization
    void Start()
    {
        displayTime = 5;
        showText = false;
        showGui = true;
        materials = new Material[8] { newMaterial1, newMaterial2, newMaterial3, newMaterial4, newMaterial5, newMaterial6, newMaterial7, newMaterial8 };
        hasFood = false;
        foodCount = 0;
        tornadoZone = false;
        currentState = 0;
        gameOver = false;
		challengeDone = false;
		antEyeRunning = false;

        PlayerText = GameObject.Find("WelcomeText");
        TextMeshP = PlayerText.GetComponent<TextMeshPro>();
        physicalNarrator = GameObject.Find("Physical Narrator");
        narratorAudio = physicalNarrator.GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
        controller = player.GetComponent<OVRPlayerController>();

        //StartCoroutine(displayTutorialText());

        Debug.Log("Current State: " + currentState);
        //gameObject.GetComponent<WriteInFile>().enabled=true;


    }

    // Update is called once per frame
    void Update()
    {

		if (challengeDone) {

            StartCoroutine(challengeAudio());
            
			challengeDone = false;

			//antEyeRunning = true;
			//StartCoroutine (antEyeText ());

		}

		if (antEyeRunning) {

			Vector3 targetDir = nest.transform.position - transform.position;
			targetDir = targetDir.normalized;
			float dot = Vector3.Dot (targetDir, transform.forward);
			angleToPosition = Mathf.Acos (dot) * Mathf.Rad2Deg;

           
            BrightnessRef.brightness = 1.2f * (1f / angleToPosition);


            lastAnglePosition = angleToPosition;

		}

        /* bn   
        
        
        
        if (isOver())
        {
            changeTexture(7);
            text.SetActive(true);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<OVRPlayerController>().enabled = false;
        }*/


        if (showGui)
        {
            if (showText)
            {
                //StartCoroutine(displayTutorial());
                //displayTutorial();
                showGui = false;
            }
            else
            {
                if (displayTime < 0)
                {
                    showText = true;
                    displayTime = 5;
                }
                else
                {
                    displayTime -= Time.deltaTime;
                }
            }
        }

    }

    public void increaseState()
    {
        currentState++;
        Debug.Log("Current State: " + currentState);
    }

    public int getCurrentState()
    {
        return currentState;
    }

    public bool isOver()
    {
        if (getCurrentState() == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    IEnumerator countdownTimerText()
    {
        TextMeshP.text = "5";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerText.SetActive(false);
        TextMeshP.text = "4";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerText.SetActive(false);
        TextMeshP.text = "3";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerText.SetActive(false);
        TextMeshP.text = "2";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerText.SetActive(false);
        TextMeshP.text = "1";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerText.SetActive(false);

    }

    IEnumerator displayChallengeResult()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();
        OVRPlayerController controller = player.GetComponent<OVRPlayerController>();
        Vector3 targetDir = nest.transform.position - player.transform.position;
        //targetDir = targetDir.normalized;
        float dot = Vector3.Dot(targetDir.normalized, transform.forward.normalized);
        angleToPosition = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Debug.Log(angleToPosition);
        Debug.Log("Nest Position " + nest.transform.position);
        Debug.Log("Player position " + transform.position);
        Debug.Log("Target Direction " + targetDir);

        TextMeshP.faceColor = new Color(255, 255, 255);
        TextMeshP.color = new Color(255, 255, 255);
        TextMeshP.richText = true;
        if (angleToPosition < 45)
        {
            TextMeshP.text = "YOU WERE OUT BY " + "<color=green>" + angleToPosition + "</color> DEGREES";

        }
        else if (angleToPosition < 90)
        {
            TextMeshP.text = "YOU WERE OUT BY " + "<color=yellow>" + angleToPosition + "</color> DEGREES";
        }
        else if (angleToPosition > 90)
        {
            TextMeshP.text = "YOU WERE OUT BY " + "<color=red>" + angleToPosition + "</color> DEGREES";
        }

        PlayerText.SetActive(true);
        yield return new WaitForSeconds(5);
        PlayerText.SetActive(false);
        antEyeRunning = true;
    }

    /* ======= Text related functions (VOID AS OF ITERATION 1 of Russell Project 26/02/20 - now done via audio =======
      
     IEnumerator displayTutorialText()
    {
        PlayerText.SetActive(true);
		yield return new WaitUntil(() => Input.anyKey);
        PlayerText.SetActive(false);
        TextMeshP.text = "We need your help!";
        TextMeshP.color = new Color(0, 0, 0);
        TextMeshP.fontSize = 18;
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
        TextMeshP.text = "Find some food and bring it home!";
        PlayerText.SetActive(true);
		yield return new WaitForSeconds (4);
        PlayerText.SetActive(false);
        TextMeshP.text = "Use the joystick to move forward";
        PlayerText.SetActive(true);
		yield return new WaitForSeconds (4);
        PlayerText.SetActive(false);
        TextMeshP.text = "Rotate your head to change direction";
        PlayerText.SetActive(true);
		yield return new WaitForSeconds (4);
        PlayerText.SetActive(false);
    }
    IEnumerator displayFoundFoodText()
    {
        TextMeshP.text = "You have found some food";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
		PlayerText.SetActive (false);

    }

	IEnumerator displayChallangeText()
	{
		TextMeshP.text = "Now you need to take the food back!";
		PlayerText.SetActive(true);
		yield return new WaitForSeconds(4);
		PlayerText.SetActive (false);
		TextMeshP.text = "Which direction do you think home is?";
		PlayerText.SetActive (true);
		yield return new WaitForSeconds(4);
		PlayerText.SetActive (false);
		TextMeshP.text = "Turn towards it";
		PlayerText.SetActive (true);
		yield return new WaitForSeconds (2);
		PlayerText.SetActive (false);
		TextMeshP.text = "You have <color=red>5</color> seconds";
		PlayerText.SetActive (true);
		yield return new WaitForSeconds (4);
		PlayerText.SetActive (false);
		TextMeshP.text = "5";
		TextMeshP.color = new Color(255, 0, 0);
		PlayerText.SetActive(true);
		yield return new WaitForSeconds(1);
		PlayerText.SetActive (false);
		TextMeshP.text = "4";
		PlayerText.SetActive(true);
		yield return new WaitForSeconds(1);
		PlayerText.SetActive (false);
		TextMeshP.text = "3";
		PlayerText.SetActive(true);
		yield return new WaitForSeconds(1);
		PlayerText.SetActive (false);
		TextMeshP.text = "2";
		PlayerText.SetActive(true);
		yield return new WaitForSeconds(1);
		PlayerText.SetActive (false);
		TextMeshP.text = "1";
		PlayerText.SetActive(true);
		yield return new WaitForSeconds(1);
		PlayerText.SetActive (false);
		challengeDone = true;

	}

	

    IEnumerator findMoreFoodText() {
        TextMeshP.text = "Well done!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(2);
        PlayerText.SetActive(false);
        TextMeshP.text = "You have found some food!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
        TextMeshP.text = "You need to find more!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
    }

    IEnumerator lastFoodText() {
        TextMeshP.text = "Almost there!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
        TextMeshP.text = "You need to find one more piece!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
        TextMeshP.text = "Look around until you see it!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
    }

     IEnumerator displayFinishedText()
    {
        PlayerText.SetActive(false);
        TextMeshP.text = "Congratulations!";
        TextMeshP.faceColor = new Color(255, 255, 255);
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
        TextMeshP.text = "You have completed <color=red>the Ant Navigation Challenge!</color>";
        PlayerText.SetActive(true);
		yield return new WaitForSeconds (4);
        PlayerText.SetActive(false);
        TextMeshP.text = "Want to know more?";
        PlayerText.SetActive(true);
		yield return new WaitForSeconds (4);
        PlayerText.SetActive(false);
        TextMeshP.text = "Ask us a question!";
        PlayerText.SetActive(true);
    }
	

    /* IEnumerator displayTutorial()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
        changeTexture(0); // Look for food...
        text.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
        changeTexture(1); // You have a Compass
        text.SetActive(true);
        compass.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
        changeTexture(2); // Try to find some food
        text.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
    }


    // IEnumerator displayText()
    //{
    //    text.SetActive(true);
    //    yield return new WaitForSeconds(4);
    //    text.SetActive(false);
    //}

    //IEnumerator displayChageVector()
    //{
    //    changeTexture(4);
    //    text.SetActive(true);
    //    yield return new WaitForSeconds(4);
    //    text.SetActive(false);
    //    changeTexture(5);
    //    text.SetActive(true);
    //    yield return new WaitForSeconds(4);
    //    text.SetActive(false);
    //}


    // void changeTexture(int i)
    //{
    //    Renderer rend = text.GetComponent<Renderer>();
    //    if (rend != null)
    //    {
    //        rend.material = materials[i];
    //    }
    //}

     */

    public GameObject getFoodObject()
    {
        return food;
    }


    IEnumerator challengeAudio()
    {
        controller.SetMoveScaleMultiplier(0f);
        playerRigidBody.freezeRotation = true;
        narratorAudio.clip = Resources.Load("Narration 4") as AudioClip;
        narratorAudio.Play();
        yield return new WaitForSeconds(20);
        StartCoroutine(countdownTimerText());
        yield return new WaitForSeconds(6);
        StartCoroutine(displayChallengeResult());
        controller.SetMoveScaleMultiplier(1f);
        playerRigidBody.freezeRotation = false;
        narratorAudio.clip = Resources.Load("Narration 5") as AudioClip;
        narratorAudio.Play();
        yield return new WaitForSeconds(2);
        controller.SetMoveScaleMultiplier(1f);
        playerRigidBody.freezeRotation = false;
    }

    void OnTriggerEnter(Collider other)
    {


        // Collect food
        if (other.gameObject.CompareTag("Food"))
        {
            //changeTexture(3);
            food = other.gameObject;

            //food.transform.position = GameObject.FindGameObjectWithTag("NoFoodZone").GetComponent<NoFoodZone>().getInitialPositionFood().position;
            food.SetActive(false);

            // Compass 
            //StartCoroutine(displayFoundFoodText());

            
            hasFood = true;
            if (hasFood & currentState == 0) { // First cupcake
                                                
                Instantiate(secondCupcake, spawnPoint.position, spawnPoint.rotation);

                // Set narrator to nearby player once found first cupcake
                physicalNarrator.transform.position = new Vector3(-133.481f, -22.407f, 69.438f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-12.773f, 80.254f, 3.552f);

                narratorAudio.clip = Resources.Load("Narration 2") as AudioClip;
                //narratorAudio.PlayOneShot(narratorClip);
                narratorAudio.Play();
                
            }
            if (hasFood & currentState == 1) { // Second cupcake
                Instantiate(thirdCupcake, spawnPoint2.position, spawnPoint2.rotation);

                physicalNarrator.transform.position = new Vector3(-196.016f, -21.442f, 18.086f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-7.928f, 43.162f, 12.636f);
                narratorAudio.clip = Resources.Load("Narration 3") as AudioClip;
                narratorAudio.Play();

            }
            if (hasFood & currentState == 2) { // Third cupcake
							

                physicalNarrator.transform.position = new Vector3(-236.647f, -21.97f, 73.888f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-20.883f, 206.86f, 14.784f);

                challengeDone = true;

				Debug.Log(challengeDone);

            }

            //compass.GetComponent<Compass>().setTarget(nest.transform);
            //compass.GetComponent<Renderer>().material = GreenArrow;

            increaseState();
        }
        if (other.gameObject.CompareTag("Nest"))
        {
            if (hasFood)
            {   
                antEyeRunning = false;
                BrightnessRef.brightness = 1f;
                GameObject player = GameObject.FindGameObjectWithTag ("Player");
				Rigidbody playerRigidBody = player.GetComponent<Rigidbody> ();
				OVRPlayerController controller = player.GetComponent<OVRPlayerController>();
				controller.SetMoveScaleMultiplier (0f);
                
                //hasFood = false;                
                GameObject f = GameObject.FindGameObjectWithTag("Food");
                food.SetActive(true);
                increaseState();

                physicalNarrator.transform.position = new Vector3(0.63f, -27.284f, 1.055f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-12.529f, 231.681f, 5.573f);
                narratorAudio.clip = Resources.Load("Narration 6") as AudioClip;
                narratorAudio.Play();

                // Press any button for next level
                TextMeshP.text = "IF YOU WOULD LIKE TO PLAY THE NEXT LEVEL, PLEASE ASK DEMONSTRATOR";
                TextMeshP.fontSize = 12;
                PlayerText.SetActive(true);

            }

        }
        if (other.gameObject.CompareTag("TornadoZone"))
        {
            if (tornadoZone)
            {
                if (getCurrentState() == 3 || getCurrentState() == 6 || getCurrentState() == 9)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.GetComponent<ShakeScreen>().enabled = true;
                    dust.SetActive(true);
                }
            }
        }


    }

}