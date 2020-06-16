using UnityEngine;
using System.Collections;
using TMPro;


public class behaviour : MonoBehaviour
{

    public GameObject text;
    public GameObject startText;
    public GameObject tmp;
    private float displayTime;
    private float secondsCount;
    private float rawSecondsCount;
    private float firstFoodTime;
    private float secondFoodTime;
    private float thirdFoodTime;
    private int minuteCount;
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
    public TextMeshPro TextMeshPTimer;
    public GameObject TimerText;

    private bool timerOn;

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


    private leaderboard leaderboard;
    private string defaultPlayerName = "Cur. Attempt";

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
        timerOn = false;

        StartCoroutine(displayTutorialText());

        PlayerText = GameObject.Find("PlayerText");
        TextMeshP = PlayerText.GetComponent<TextMeshPro>();
        physicalNarrator = GameObject.Find("Physical Narrator");
        narratorAudio = physicalNarrator.GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
        controller = player.GetComponent<OVRPlayerController>();
        nest = GameObject.Find("Home");

        leaderboard = GameObject.Find("Leaderboard").GetComponent<leaderboard>();
        leaderboard.Start();

        Debug.Log("Current State: " + currentState);
        //gameObject.GetComponent<WriteInFile>().enabled=true;


    }

    // Update is called once per frame
    void Update()
    {




        if (timerOn)
        {
            secondsCount += Time.deltaTime;
            rawSecondsCount += Time.deltaTime;

            if (minuteCount > 0)
                TextMeshPTimer.text = minuteCount.ToString("00") + ":" + ((int)secondsCount).ToString("00");
            else
                TextMeshPTimer.text = ((int)secondsCount).ToString();

            if (secondsCount >= 60)
            {
                minuteCount++;
                secondsCount -= 60;
            }
        }




        if (challengeDone)
        {
            
            //StartCoroutine(displayChallengeResult());
            StartCoroutine(challengeAudio());
            challengeDone = false;
            // Enabling player movement
            controller.SetMoveScaleMultiplier(1f);
            playerRigidBody.freezeRotation = false;
            //antEyeRunning = true;
            //StartCoroutine (antEyeText ());


        }

        if (antEyeRunning)
        {

            Vector3 targetDir = nest.transform.position - transform.position;
            targetDir = targetDir.normalized;
            float dot = Vector3.Dot(targetDir, transform.forward);
            angleToPosition = Mathf.Acos(dot) * Mathf.Rad2Deg;


            BrightnessRef.brightness = 1.2f * (1f / angleToPosition);
            /*if (angleToPosition < lastAnglePosition) {
				BrightnessRef.brightness = (1.5f * angleToPosition) / 180;
			}
			if (angleToPosition > lastAnglePosition) {
				BrightnessRef.brightness = (0.5f * angleToPosition) / 180;
			}*/


            lastAnglePosition = angleToPosition;

            Debug.Log(RenderSettings.ambientLight);
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

    IEnumerator displayTutorialText()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        OVRPlayerController controller = player.GetComponent<OVRPlayerController>();
        // Disabling player movement
        controller.SetMoveScaleMultiplier(0f);
        PlayerText.SetActive(true);
        TextMeshP.text = "Ant Navigation Challenge";
        //yield return new WaitUntil(() => Input.anyKey);
        //PlayerText.SetActive(false);
        //TextMeshP.text = "Ant Navigation Challenge";        

        yield return new WaitForSeconds(26);
        PlayerText.SetActive(false);
        TextMeshP.color = new Color(255, 0, 0);
        TextMeshP.text = "Get Ready...";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
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
        TextMeshP.color = new Color(0, 0, 0);
        // Enabling player movement
        controller.SetMoveScaleMultiplier(1f);
        TextMeshPTimer.text = "0";
        TextMeshPTimer.color = new Color(255, 255, 255, 0.5f);
        TimerText.SetActive(true);
        timerOn = true;
    }

    IEnumerator displayFoundFoodText()
    {
        TextMeshP.text = "You have found some food";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);

    }

    IEnumerator displayChallangeText()
    {
        TextMeshP.text = "Congratulations! You have found all the food in ";
        if (minuteCount == 1)
        {
            TextMeshP.text += minuteCount + " minute and ";
        } else if (minuteCount > 1)
        {
            TextMeshP.text += minuteCount + " minutes and ";
        }
        if ((int)secondsCount == 1)
        {
            TextMeshP.text += (int)secondsCount + " second.";
        } else if ((int)secondsCount > 1)
        {
            TextMeshP.text += (int)secondsCount + " seconds.";
        }
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(false);
        TimerText.SetActive(false);
        TextMeshP.text = "Now you need to take the food back!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        TextMeshP.text = "Which direction do you think home is?";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        TextMeshP.text = "Turn towards it";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(2);
        PlayerText.SetActive(false);
        leaderboard.HideCheckpointLeaderBoard();
        TextMeshP.text = "You will have <color=red>5</color> seconds";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        TextMeshP.text = "5";
        TextMeshP.color = new Color(255, 0, 0);
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
        challengeDone = true;

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

    IEnumerator displayChallengeResult()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();
        OVRPlayerController controller = player.GetComponent<OVRPlayerController>();
        Vector3 targetDir = nest.transform.position - transform.position;
        targetDir = targetDir.normalized;
        float dot = Vector3.Dot(targetDir, transform.forward);
        angleToPosition = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Debug.Log(angleToPosition);
        Debug.Log("Nest Position " + nest.transform.position);
        Debug.Log("Player position " + transform.position);
        Debug.Log("Target Direction " + targetDir);

        //Rounding angle to 2 decimal place
        angleToPosition = Mathf.Round(angleToPosition * 100.0f) / 100.0f;
        leaderboard.SaveAngle(angleToPosition);
        TextMeshP.faceColor = new Color(255, 255, 255);
        TextMeshP.color = new Color(255, 255, 255);
        TextMeshP.richText = true;
        if (angleToPosition < 45)
        {
            TextMeshP.text = "YOU WERE OUT BY " + "<color=green>" + angleToPosition + "</color> DEGREES";
        }
        else if (angleToPosition <= 90)
        {
            TextMeshP.text = "YOU WERE OUT BY " + "<color=yellow>" + angleToPosition + "</color> DEGREES";
        }
        //else if (angleToPosition > 90) {
        else
        {
            TextMeshP.text = "YOU WERE OUT BY " + "<color=red>" + angleToPosition + "</color> DEGREES";
        }
        //TextMeshP.text = "YOU WERE OUT BY " + "<color=" + guessColor + ">" + angleToPosition +  "</color> DEGREES";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(6);
        PlayerText.SetActive(false);
        TextMeshP.color = new Color(255, 255, 255);
        TextMeshP.text = "<color=red>ANT EYE</color>";
        yield return new WaitForSeconds(4);
        PlayerText.SetActive(true);
        TextMeshP.text = "Hint: look around slowly!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        antEyeRunning = true;
        yield return new WaitForSeconds(5);
        PlayerText.SetActive(false);
        //antEyeRunning = true;
    }

    IEnumerator findMoreFoodText()
    {
        TextMeshP.text = "Well done!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(2);
        PlayerText.SetActive(false);
        TextMeshP.text = "You need to find more!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
    }

    IEnumerator lastFoodText()
    {
        TextMeshP.text = "Almost there!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        TextMeshP.text = "You need to find one more piece!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
    }

    IEnumerator displayFinishedText()
    {
        PlayerText.SetActive(false);
        TextMeshP.text = "Well done!";
        TextMeshP.faceColor = new Color(255, 255, 255);
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        TextMeshP.text = "You have completed <color=red>the Ant Navigation Challenge!</color>";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(5);
        PlayerText.SetActive(false);
        TextMeshP.text = "Want to know more?";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        TextMeshP.text = "Ask us a question!";
        PlayerText.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerText.SetActive(false);
        leaderboard.LoadLeaderBoardStats();
        leaderboard.ShowLeaderBoard();
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


    /* IEnumerator displayText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
    }

    IEnumerator displayChageVector()
    {
        changeTexture(4);
        text.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
        changeTexture(5);
        text.SetActive(true);
        yield return new WaitForSeconds(4);
        text.SetActive(false);
    }*/


    /* void changeTexture(int i)
    {
        Renderer rend = text.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = materials[i];
        }
    }*/

    public GameObject getFoodObject()
    {
        return food;
    }


    void OnTriggerEnter(Collider other)
    {

        // Collect food
        if (other.gameObject.CompareTag("Food"))
        {
            //changeTexture(3);
            food = other.gameObject;
            // cambiar texto de llevala a casa
            //food.transform.position = GameObject.FindGameObjectWithTag("NoFoodZone").GetComponent<NoFoodZone>().getInitialPositionFood().position;
            food.SetActive(false);
            // Compass 
            //StartCoroutine(displayFoundFoodText());
            hasFood = true;
            if (hasFood & currentState == 0)
            {
                firstFoodTime = Mathf.Round(rawSecondsCount * 100.0f) / 100.0f;
                leaderboard.SaveFirstTime(defaultPlayerName, firstFoodTime);
                leaderboard.LoadCheckpointLeaderBoardStats(1);
                leaderboard.ShowCheckpointLeaderBoard();
                StartCoroutine(findMoreFoodText());
                Instantiate(secondCupcake, spawnPoint.position, spawnPoint.rotation);

                // Set narrator to nearby player once found first cupcake
                physicalNarrator.transform.position = new Vector3(-133.481f, -22.407f, 69.438f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-12.773f, 80.254f, 3.552f);

                narratorAudio.clip = Resources.Load("Narration 2") as AudioClip;
                //narratorAudio.PlayOneShot(narratorClip);
                narratorAudio.Play();
            }
            if (hasFood & currentState == 1)
            {
                secondFoodTime = Mathf.Round(rawSecondsCount * 100.0f) / 100.0f;
                leaderboard.SaveSecondTime(secondFoodTime);
                leaderboard.LoadCheckpointLeaderBoardStats(2);
                leaderboard.ShowCheckpointLeaderBoard();
                StartCoroutine(lastFoodText());
                Instantiate(thirdCupcake, spawnPoint2.position, spawnPoint2.rotation);

                physicalNarrator.transform.position = new Vector3(-196.016f, -21.442f, 18.086f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-7.928f, 43.162f, 12.636f);
                narratorAudio.clip = Resources.Load("Narration 3") as AudioClip;
                narratorAudio.Play();
            }
            if (hasFood & currentState == 2)
            {
                
                thirdFoodTime = Mathf.Round(rawSecondsCount * 100.0f) / 100.0f;
                leaderboard.SaveThirdTime(thirdFoodTime);
                leaderboard.LoadCheckpointLeaderBoardStats(3);
                leaderboard.ShowCheckpointLeaderBoard();
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();
                OVRPlayerController controller = player.GetComponent<OVRPlayerController>();
                // Disabling player movement
                controller.SetMoveScaleMultiplier(0f);
                timerOn = false;
                //StartCoroutine(displayChallangeText());
                playerRigidBody.freezeRotation = true;
                //Vector3 toPosition = (nest.transform.position - transform.position);
                //float angleToPosition = Vector3.Angle (transform.forward, toPosition);
                
                physicalNarrator.transform.position = new Vector3(-236.647f, -21.97f, 73.888f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-20.883f, 206.86f, 14.784f);

                challengeDone = true;
                Debug.Log(challengeDone);
                //StartCoroutine (countdownTimerText ()); 
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
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();
                OVRPlayerController controller = player.GetComponent<OVRPlayerController>();
                // Disabling player movement
                controller.SetMoveScaleMultiplier(0f);
                StartCoroutine(displayFinishedText());
                //StartCoroutine(displayChageVector());
                //hasFood = false;
                compass.GetComponent<Compass>().setTarget(food.transform);
                compass.GetComponent<Renderer>().material = RedArrow;

                GameObject f = GameObject.FindGameObjectWithTag("Food");
                food.SetActive(true);
                tornadoZone = true;
                increaseState();

                physicalNarrator.transform.position = new Vector3(0.63f, -27.284f, 1.055f);
                physicalNarrator.transform.rotation = Quaternion.Euler(-12.529f, 231.681f, 5.573f);
                narratorAudio.clip = Resources.Load("Narration 6") as AudioClip;
                narratorAudio.Play();
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