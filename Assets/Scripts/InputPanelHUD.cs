using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class InputPanelHUD : MonoBehaviour {

    private RobotSyncBehavior robot;
    private int numberOfPlayers = 4;
    private int robotID = 1;

    private Animator panelAnimator;
    private Animator[] animator;
    private Sprite[] sprites;
    private Image[] imageBtn;
    private Image[] imageMov;
    private Image[] imageBlk;
    private Image atkImage;
    private Slider health;
    private Image healthFill;

    private string[] currentPress;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/circles");
        panelAnimator = GetComponent<Animator>();
        atkImage = transform.FindChild("AtkImage").GetComponent<Image>();
        health = transform.parent.GetComponentInChildren<Slider>();
        healthFill = health.transform.FindChild("Fill Area").GetComponentInChildren<Image>();
    }
    
	void Start () {
        robot = transform.parent.parent.GetComponent<RobotSyncBehavior>();
        if (robot)
        {
            numberOfPlayers = robot.NumberOfPlayers;
            robotID = robot.RobotID;
        }
        
        imageBtn = new Image[numberOfPlayers];
        imageMov = new Image[numberOfPlayers];
        imageBlk = new Image[numberOfPlayers];
        currentPress = new string[numberOfPlayers];
        animator = new Animator[numberOfPlayers];

        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject playerPanel = transform.GetChild(i).gameObject;
            playerPanel.transform.FindChild("PxImg").GetComponent<Image>().sprite = sprites[11 + robot.PlayerID2JoystickID(i+1)];
            imageBtn[i] = playerPanel.transform.FindChild("BtnImg").GetComponent<Image>();
            imageMov[i] = playerPanel.transform.FindChild("MovImg").GetComponent<Image>();
            imageBlk[i] = playerPanel.transform.FindChild("BlkImg").GetComponent<Image>();
            animator[i] = playerPanel.GetComponent<Animator>();
            currentPress[i] = "";
        }
        
        //just in case
        for (int i = numberOfPlayers; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            //To much trouble...
            //RectTransform t = (RectTransform) transform;
            //t.sizeDelta = new Vector2(t.rect.width - 120, t.rect.height);
        }
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void SetPressed(string key, int playerID, bool state)
    {
        bool isMove = false;
        if (key == "Forward" || key == "Backward" || key == "Left" || key == "Right") isMove = true;

        if (!isMove)
        {
            //Not nice, but eh
            if (state == true)
            {
                imageBtn[playerID - 1].sprite = sprites[KeyStringToSpriteNumber(key)];
                imageBtn[playerID - 1].color = new Color(1, 1, 1, 1);
                currentPress[playerID - 1] = key;
            }
            else
            {
                imageBtn[playerID - 1].color = new Color(0, 0, 0, 0);
                imageBlk[playerID - 1].color = new Color(0, 0, 0, 0);
                currentPress[playerID - 1] = "";
            }
            CheckGlow();
        }
        else
        {
            if (state == true)
            {
                imageMov[playerID - 1].sprite = sprites[KeyStringToSpriteNumber(key)];
                imageMov[playerID - 1].color = new Color(1, 1, 1, 1);
            }
            else
            {
                imageMov[playerID - 1].color = new Color(0, 0, 0, 0);
            }
        }
    }

    private void CheckGlow()
    {
        string[] concurrentPresses = currentPress.GroupBy(s => s).Where(g => g.Count() > 1 && g.Key != "").Select(g => g.Key).ToArray();

        bool attackReady = true;
        string syncedAttack = currentPress[0];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int pos = Array.IndexOf(concurrentPresses, currentPress[i]);
            if (pos > -1)
                animator[i].SetBool("Glow", true);
            else
                animator[i].SetBool("Glow", false);
            if (currentPress[i] != syncedAttack) attackReady = false;
        }

        if (attackReady && syncedAttack!="")
        {
            //set atkImage
            for (int i = 0; i < numberOfPlayers; i++)
            {
                imageBlk[i].color = new Color(1, 1, 1, 1);
                currentPress[i] = "";
            }

            StartCoroutine(FlashAttack(syncedAttack));
        }
        
    }
    IEnumerator FlashAttack(string syncedAttack)
    {
        atkImage.color = Color.white;
        atkImage.sprite = Resources.Load<Sprite>("Sprites/action-"+syncedAttack);
        panelAnimator.SetBool("Glow", true);
        yield return new WaitForSeconds(1.5f);
        panelAnimator.SetBool("Glow", false);
        yield return new WaitForSeconds(0.1f);
        atkImage.color = new Color(0, 0, 0, 0);
    }

    private int KeyStringToSpriteNumber(string key)
    {
        switch (key)
        {
            case "Forward":
                return 4;
            case "Right":
                return 5;
            case "Backward":
                return 6;
            case "Left":
                return 7;
            case "LeftPunch":
                return 2; //X
            case "RightPunch":
                return 3; //Y
            case "LeftKick":
                return 0; //A
            case "RightKick":
                return 1; //B
            case "Block":
                return 8; //RB
        }
        return 15;
    }

    public void SetHealth(float percentage) {
        health.value = percentage;
        if (percentage > 0.5f)
        {
            healthFill.color = new Color(25.0f / 255.0f, 108.0f / 255.0f, 0, 1);
        }
        else if (percentage > 0.15f)
        {
            healthFill.color = new Color(187.0f / 255.0f, 156.0f / 255.0f, 0, 1);
        }
        else
        {
            healthFill.color = new Color(143.0f / 255.0f, 0, 0, 1);
        }
    }

}
