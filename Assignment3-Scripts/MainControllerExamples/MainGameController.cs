using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    public int indexOfDict = 1;
    static public int alarmType;
    //***************************************************************************//
    // This class is the main controller of the game and manages customer creation,
    // time management, money management, game state and win/lose state.
    // it also manages available seats in your shop.
    //***************************************************************************//

    // freeplay goal ballance
    public int freeplayGoalBallance = 170;
    static public int staticFreeplayGoalBallance; //To use freeplayGoalBallance in other classes 
                                                  //without needing the gameObject, while preserving the public type                                         //of the original variable "endlessGoalBallance"
                                                  //******************//
                                                  // Mission Variables (for Career mode) //
    public int availableTime;               //Seconds
                                            //******************//
                                            // Common variables
    static public bool canUseCandy;

    static public int availableBlender;     //total number of available blenders for this level
    public GameObject[] availableBlenderGo; //GameObject reference of available blender objects in game
    //******************//

    // Static Variables //
    //******************//
    static public string gameMode;          //game mode setting (freeplay or career)
    static public int gameTime;
    static public int requiredBalance;
    Dictionary<int, string[]> openWith = new Dictionary<int, string[]>();
    Dictionary<string[], string[] > tempDictionary = new Dictionary<string[], string[]>();
    //*******************

    //public game objects
    public GameObject[] customers;  //list of all available customers (different patience and textures)
    public GameObject Click4More; //click for more button
    
    //public variables
    //*****
    //How many seats are available in this shop?
    //These two array should always be the same size (Length)
    public bool[] availableSeatForCustomers;
    public Vector3[] seatPositions;
    public GameObject[] additionalItems;    //Items that can be purchased via in-game shop.
    public GameObject endGamePlane;         //main endgame plane
    public GameObject endGameStatus;        //gameobject which shows the texture of win/lose states
    public Texture2D[] endGameTextures;     //textures for win/lose states
    //*****
    static public double timeload=0.1;
    //static variables. do not touch!
    //static public bool deliveryQueueIsFull;               //delivery queue can accept 6 ingredients. more is not acceptable.
    //static public int deliveryQueueItems;             //number of items in delivery queue
    //static public List<int> deliveryQueueItemsContent = new List<int>();  //conents of delivery queue

    ///game timer vars
    private string remainingTime;
    static public int hours = 7;
    static public int minutes = 0;
    static public double countMinutes = 1;
    private bool minutesTrigger = false;
    private int indexHour = 0;
    private int indexMinute = 0;
    private int seconds;
    private int servicenumber;
    private int totalnumber;

    //private float badDeliveryPenalty = 5; //not implemented yet (can be used over time or money)

    //Cutomers (statistical Variables)
    //private int customersAppeared;
    //private int satisfiedCutomer;
    //private int angryCustomer;
    private int delay;                  //delay between creating a new customer (smaller number leads to faster customer creation)
    private bool canCreateNewCustomer; //flag to prevent double calls to functions

    //Money and GameState
    static public int totalMoneyMade;
    static public int satisfiedCustomer;
    static public int unsatisfiedCustomer;
    static public int customerID = 0;
    public int malenumber = 0;
    public int femalenumber = 0;
    public int healthynumber = 0;
    public int disabilitynumber = 0;

    //private int totalMoneyLost;
    static public bool gameIsFinished;     //Flag

    ///////////////////////////////////////
    //static public int slotState = 0;              //available slots for product creation (same as delivery queue)
    //static public int maxSlotState;               //maximum available slots in delivery queue (set in init)

    //****************************
    // 3D Text Objects 
    //****************************
    public GameObject moneyText;
    public GameObject missionText;
    public GameObject timeText;
    public GameObject satisfiedText;

    //AudioClips
    public AudioClip timeEndSfx;
    public AudioClip winSfx;
    public AudioClip loseSfx;

    public class CustomerList : List<Customer> { }

    public class Customer : List<Customer>
    {
        public string Type { get; set; }
        public string Time { get; set; }
        public string ServiceName { get; set; }
    }

    static public CustomerList customer_list;


    public void Awake()
    {
        Init();
    }


    //***************************************************************************//
    // Init everything here.
    //***************************************************************************//
    void Init()
    {

        //这里传进来病人的ID，Time，但是这两个只值这里必须是一一对应的，下面假设用csv的方法
        // for(int i=0; i<Length;i++){
        //  ID=csv.第i行.ID
        //  Time=csv.第i行.Time
        //  tempDictionary.Add("ID",ID)
        //  tempDictionary.Add("Time",Time)

        //  openWith.Add(str(i),tempDictionary);

        //  #清理一下
        //  tempDictionary.Clear()
        // }

        string[] data1 = { "1", "07:05", "Wrist Alarm" };
        string[] data2 = { "3", "08:10", "Wrist Alarm" };
        string[] data3 = { "1", "09:15","Pull Cord Alarm"};
        string[] data4 = { "2", "10:20", "General Alarm" };
        //为了避免数组越界需要在所有数据后面多加一个数据并且最好是永远不可能达到的时间点
        string[] data5 = { "0", "25:00" };
        openWith.Add(1, data1);
        openWith.Add(2, data2);
        openWith.Add(3, data3);
        openWith.Add(4, data4);
        openWith.Add(5, data5);

        CustomerList customer_list = new CustomerList();
        //print(customer_list);
        CSV.GetInstance().loadFile(Application.dataPath + "/Fruit-Juice-Maker/Res", "testcsv.csv");
        for (int i = 1; i < CSV.GetInstance().m_ArrayData.Count; i++)
        {
            Customer one_customer = new Customer();
            one_customer.Type = CSV.GetInstance().getString(i, 0);
            one_customer.Time = CSV.GetInstance().getString(i, 1);
            one_customer.ServiceName = CSV.GetInstance().getString(i, 2);
            string[] data = { one_customer.Type, one_customer.Time, one_customer.ServiceName };

            //Debug.Log("customer type: " + one_customer.Type);
            //Debug.Log("customer time: " + one_customer.Time);
            //Debug.Log("customer service name: " + one_customer.ServiceName);
            //openWith.Add(i, data);
        }
        //print(customer_list);
        for (int w = 0; w < customer_list.Count; w++)
        {
            for (int t = 0; t < customer_list[w].Count; t++)
            {
                //print(customer_list[w][t]);
            }
        }
        Application.targetFrameRate = 50; //Optional based on the target platform
                                          //slotState = 0;
                                          //maxSlotState = 6;
                                          //deliveryQueueIsFull = false;
                                          //deliveryQueueItems = 0;
                                          //deliveryQueueItemsContent.Clear();

        //customersAppeared = 0;
        //satisfiedCutomer = 0;
        //angryCustomer = 0;
        totalMoneyMade = 0;
        //totalMoneyLost = 0;
        gameIsFinished = false;


        //Optimal value should be between 5 (Always crowded) and 15 (Not so crowded) seconds. 
        delay = 11;
        canCreateNewCustomer = false;
        //set all seats as available at the start of the game. No seat is taken yet.
        for (int i = 0; i < availableSeatForCustomers.Length; i++)
        {
            availableSeatForCustomers[i] = true;
        }

        //check if player previously purchased these items..
        //ShopItem index starts from 1.
        for (int j = 0; j < additionalItems.Length; j++)
        {
            //format the correct string we use to store purchased items into playerprefs
            string shopItemName = "shopItem-" + (j + 1).ToString(); ;
            if (PlayerPrefs.GetInt(shopItemName) == 1)
            {
                //we already purchased this item
                additionalItems[j].SetActive(true);
            }
            else
            {
                additionalItems[j].SetActive(false);
            }
        }



        //check game mode.
        if (PlayerPrefs.HasKey("gameMode"))
            gameMode = PlayerPrefs.GetString("gameMode");
        else
            gameMode = "FREEPLAY"; //default game mode

        switch (gameMode)
        {

            case "FREEPLAY":
                requiredBalance = freeplayGoalBallance;
                gameTime = 0;
                canUseCandy = false;
                availableTime = 50;
                //override belnder limit
                availableBlender = 1;
                break;

            case "CAREER":
                requiredBalance = PlayerPrefs.GetInt("careerGoalBalance");
                availableTime = PlayerPrefs.GetInt("careerAvailableTime");

                //check if we are allowed to use candy in this career level
                canUseCandy = (PlayerPrefs.GetInt("canUseCandy") == 1) ? true : false;

                //if we just have 1 blender in this level, deactive the seonds one
                availableBlenderGo[1].SetActive(false);
                //Make sure to remove/edit this code to avoid unwanted repositioning in your game
                availableBlenderGo[0].transform.position -= new Vector3(1, 0, 0);

                //how many blender are available for this level?
                availableBlender = PlayerPrefs.GetInt("careerAvailableBlender");
                break;
        }
    }




    //***************************************************************************//
    // Starting delay. Optional.
    //***************************************************************************//
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0);
        canCreateNewCustomer = true;
    }


    //***************************************************************************//
    // FSM
    //***************************************************************************//
    void Update()
    {

        if (!gameIsFinished)
        {
            manageClock();
            manageGuiTexts();
            StartCoroutine(checkGameWinState());
            //print("index :"+indexOfDict);
            //print("id:"+int.Parse(openWith.ElementAt(indexOfDict).Value[0]));
            indexHour = int.Parse(openWith.ElementAt(indexOfDict).Value[1].Split(':')[0]);
            indexMinute = int.Parse(openWith.ElementAt(indexOfDict).Value[1].Split(':')[1]);
        }
        //print("没来啦");
        //if time passed
        if ((hours > indexHour)||((hours==indexHour)&&(minutes>indexMinute)) )
        {
            //进入下一个index
            //print(">来啦"+indexOfDict);
            indexOfDict += 1;
        }
        else { 
            if ((hours == indexHour)&&(minutes == indexMinute))
            {
                //进入下一个index
                //print("=来啦"+indexOfDict);
                indexOfDict += 1;
                //create a new customer if there is free seat and game is not finished yet
                if (canCreateNewCustomer && !gameIsFinished)
                {
                    if (monitorAvailableSeats() != 0)
                    {
                        createCustomer(freeSeatIndex[Random.Range(0, freeSeatIndex.Count)]);
                       //string key = dictionary.ElementAt("索引值").Key;
                    }
                    else
                    {
                        //print("No free seat is available!");
                    }
                }
            }
        }
    }


    //***************************************************************************//
    // New Customer creation routine.
    //***************************************************************************//
    void createCustomer(int _seatIndex)
    {

        //set flag to prevent double calls 
        canCreateNewCustomer = false;
        StartCoroutine(reactiveCustomerCreation());

        //Example： ID=openWith.ElementAt(indexOfDict).Value.ElementAt("ID").Value
        //customerID = Random.Range(0, customers.Length);
        customerID = int.Parse(openWith.ElementAt(indexOfDict-1).Value[0])-1;

        if(string.Equals("General Alarm", openWith.ElementAt(indexOfDict-1).Value[2])){
            alarmType=1;
        }
        if(string.Equals("Pull Cord Alarm", openWith.ElementAt(indexOfDict-1).Value[2])){
            alarmType=2;
        }
        if(string.Equals("Wrist Alarm",openWith.ElementAt(indexOfDict-1).Value[2])){
            alarmType=3;
        }
        
        CustomerController.alarmType = alarmType;
        if (customerID < 4)
        {
            malenumber++;
        }
        if (customerID >= 4)
        {
            femalenumber++;
        }
        if (customerID % 2 == 0)
        {
            healthynumber++;
        }
        if (customerID % 2 != 0)
        {
            disabilitynumber++;
        }

        //which customer?
        GameObject tmpCustomer = customers[customerID];

        //which seat
        Vector3 seat = seatPositions[_seatIndex];
        //mark the seat as taken
        availableSeatForCustomers[_seatIndex] = false;

        //create customer
        int offset = -11;
        GameObject newCustomer = Instantiate(tmpCustomer, new Vector3(offset, 0.1f, 0.2f), Quaternion.Euler(0, 180, 0)) as GameObject;

        //any post creation special Attributes?
        newCustomer.GetComponent<CustomerController>().mySeat = _seatIndex;
        //set customer's destination
        newCustomer.GetComponent<CustomerController>().destination = seat;
    }


    //***************************************************************************//
    // customer creation is active again
    //***************************************************************************//
    IEnumerator reactiveCustomerCreation()
    {
        yield return new WaitForSeconds(2);//define time of difference
        canCreateNewCustomer = true;
        yield break;
    }

    //***************************************************************************//
    // check if there is any free seat for customers and if true, return their index(s)
    //***************************************************************************//
    private List<int> freeSeatIndex = new List<int>();
    int monitorAvailableSeats()
    {
        freeSeatIndex = new List<int>();
        for (int i = 0; i < availableSeatForCustomers.Length; i++)
        {
            if (availableSeatForCustomers[i] == true)
                freeSeatIndex.Add(i);
        }

        //debug
        //print("Available seats: " + freeSeatIndex);

        if (freeSeatIndex.Count > 0)
            return -1;
        else
            return 0;
    }


    //***************************************************************************//
    // GUI text management
    //***************************************************************************//
    void manageGuiTexts()
    {
        moneyText.GetComponent<TextMesh>().text = "★" + totalMoneyMade.ToString();
        missionText.GetComponent<TextMesh>().text = "" + requiredBalance.ToString();
    }


    //***************************************************************************//
    // Game clock manager
    //***************************************************************************//
    void manageClock()
    {

        if (gameIsFinished){
            print("finished");
            return;
        }
        if (gameMode == "FREEPLAY")
        {

            gameTime = (int)(availableTime - Time.timeSinceLevelLoad); // time left
            //hours = 7 + (Mathf.CeilToInt(Time.timeSinceLevelLoad)+count) * 360 / (3600 * 6); //starts at 7am
            //minutes = ((Mathf.CeilToInt(Time.timeSinceLevelLoad)+count) * 60 % 3600 / 60);
            countMinutes=countMinutes+ timeload;
            minutes = (int)(countMinutes%60);
            if(minutes>=1){
                minutesTrigger = true;
            }
            if (minutes == 0 && minutesTrigger){
                hours += 1;
                minutesTrigger = false;
            }
            //hours =  Mathf.CeilToInt(Time.timeSinceLevelLoad) / 3600; //starts at 7am
            //minutes = Mathf.CeilToInt(Time.timeSinceLevelLoad-hours*3600)/60;
            //if (minutes == 0)
            //{
            //   hours += 1;
            //}
            //seconds = Mathf.CeilToInt(Time.timeSinceLevelLoad) * 360 % 3600 % 60;
            remainingTime = string.Format("{0:00} : {1:00}", hours, minutes);
            timeText.GetComponent<TextMesh>().text = remainingTime.ToString();

        }
        else if (gameMode == "CAREER")
        {
            gameTime = (int)(availableTime - Time.timeSinceLevelLoad);
            seconds = Mathf.CeilToInt(availableTime - Time.timeSinceLevelLoad) % 60;
            minutes = Mathf.CeilToInt(availableTime - Time.timeSinceLevelLoad) / 60;
            remainingTime = string.Format("{0:00} : {1:00}", minutes, seconds);
            timeText.GetComponent<TextMesh>().text = remainingTime.ToString();
        }

        /*
        if(seconds == 0 && minutes == 0) {
            gameIsFinished = true;
            processGameFinish();
        }
        */
    }


    //***************************************************************************//
    // One shot audio player
    //***************************************************************************//
    void playSfx(AudioClip _sfx)
    {
        GetComponent<AudioSource>().PlayOneShot(_sfx);
    }


    //***************************************************************************//
    // finish the game gracefully
    //***************************************************************************//
    IEnumerator processGameFinish()
    {

        playSfx(timeEndSfx);

        yield return new WaitForSeconds(1.5f);  //absolutely required.
        print("game is finished");
        //tell all customers to leave, if they are still in the shop :)))
        GameObject[] customersInScene = GameObject.FindGameObjectsWithTag("customer");
        if (customersInScene.Length > 0)
        {
            foreach (var customer in customersInScene)
            {
                customer.GetComponent<CustomerController>().leave();
            }
        }
        //did we reached the level goal?
        if (totalMoneyMade >= requiredBalance)
        {
            print("We beat the mission :))))");

            playSfx(winSfx);
        }
        else
        {
            print("better luck next time :((((");
            playSfx(loseSfx);
        }

    }

    //***************************************************************************//
    // Game Win/Lose State
    //***************************************************************************//
    IEnumerator checkGameWinState()
    {

        if (gameIsFinished)
            yield break;
        if (gameMode == "FREEPLAY" && hours >= 24 && totalMoneyMade < requiredBalance)
        {
            print("Go off work!");
            gameIsFinished = true;
            totalnumber = femalenumber + malenumber;
            servicenumber = satisfiedCustomer + unsatisfiedCustomer;
            satisfiedText.GetComponent<TextMesh>().text =
                "There are totally " + totalnumber.ToString() + " customers" + "\r\n" +
                "Male:" + malenumber + "    Female:" + femalenumber + "\r\n" +
                "Healthy:" + healthynumber + "    Unhealthy:" + disabilitynumber + "\r\n" +
                "You have served " + servicenumber.ToString() + " customers" + "\r\n" +
                "Satisfied:" + satisfiedCustomer.ToString() + "    Unsatisfied:" + unsatisfiedCustomer.ToString() + "\r\n";
            endGamePlane.SetActive(true);
            endGameStatus.GetComponent<Renderer>().material.mainTexture = endGameTextures[1];   //show the correct texture for result
            playNormalSfx(timeEndSfx);
            yield return new WaitForSeconds(2.0f);
            playNormalSfx(loseSfx);
        }

        else if (gameMode == "CAREER" && gameTime <= 0 && totalMoneyMade < requiredBalance)
        {

            print("Time is up! You have failed :(");    //debug the result
            print("F:" + femalenumber);
            print("M:" + malenumber);

            satisfiedText.GetComponent<TextMesh>().text = "Satisfied:" + satisfiedCustomer.ToString() + "\r\n" + "Unsatisfied:" + unsatisfiedCustomer.ToString();
            gameIsFinished = true;                      //announce the new status to other classes
            endGamePlane.SetActive(true);               //show the endGame plane
            endGameStatus.GetComponent<Renderer>().material.mainTexture = endGameTextures[1];   //show the correct texture for result
            playNormalSfx(timeEndSfx);
            yield return new WaitForSeconds(2.0f);
            playNormalSfx(loseSfx);

        }
        else if (gameMode == "FREEPLAY" && totalMoneyMade >= requiredBalance)
        {

            print("Wow, You beat the goal in freeplay mode. But You can continue... :)");
            playNormalSfx(winSfx);
            totalnumber = femalenumber + malenumber;
            servicenumber = satisfiedCustomer + unsatisfiedCustomer;
            satisfiedText.GetComponent<TextMesh>().text =
                "There are totally " + totalnumber.ToString() + " customers" + "\r\n" +
                "Male:" + malenumber + "    Female:" + femalenumber + "\r\n" +
                "You have served " + servicenumber.ToString() + " customers" + "\r\n" +
                "Satisfied:" + satisfiedCustomer.ToString() + "    Unsatisfied:" + unsatisfiedCustomer.ToString() + "\r\n";

            //gameIsFinished = true; 
            //we can still play in freeplay mode. 
            //there is no end here unless user stops the game and choose exit.
        }
    }


    //********************************************************
    // Save user progress in career mode.
    //********************************************************
    void saveCareerProgress()
    {
        int currentLevelID = PlayerPrefs.GetInt("careerLevelID");
        int userLevelAdvance = PlayerPrefs.GetInt("userLevelAdvance");

        //if this is the first time we are beating this level...
        if (userLevelAdvance < currentLevelID)
        {
            userLevelAdvance++;
            PlayerPrefs.SetInt("userLevelAdvance", userLevelAdvance);
        }
    }


    ///***********************************************************************
    /// play normal audio clip
    ///***********************************************************************
    void playNormalSfx(AudioClip _sfx)
    {
        GetComponent<AudioSource>().clip = _sfx;
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
    }
    
    // click to switch to "EndScene" for more graphs
    void Start () {
		Click4More.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("EndScene"); //swtcih to "EndScene"
    }
}
