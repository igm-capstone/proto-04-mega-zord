﻿using UnityEngine;
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
    private Image atkImage;
    private Slider health;
    private Image healthFill;

    private int[] currentPress;

    // Use this for initialization
	void Start () {
        robot = transform.parent.parent.GetComponent<RobotSyncBehavior>();
        if (robot)
        {
            numberOfPlayers = robot.NumberOfPlayers;
            robotID = robot.RobotID;
        }

        panelAnimator = GetComponent<Animator>();
        sprites = Resources.LoadAll<Sprite>("Sprites/circles");
        atkImage = GameObject.Find("AtkImage").GetComponent<Image>();
        health = transform.parent.GetComponentInChildren<Slider>();
        healthFill = health.transform.FindChild("Fill Area").GetComponentInChildren<Image>();

        imageBtn = new Image[numberOfPlayers];
        currentPress = new int[numberOfPlayers];
        animator = new Animator[numberOfPlayers];

        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject playerPanel = transform.GetChild(i).gameObject;
            playerPanel.transform.GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = sprites[11 + robot.PlayerID2JoystickID(i+1)];
            imageBtn[i] = playerPanel.transform.GetChild(1).gameObject.GetComponentInChildren<Image>();
            animator[i] = playerPanel.GetComponent<Animator>();
            currentPress[i] = -1;
        }
        
        //just in case
        for (int i = numberOfPlayers; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            //To much trouble...
            //RectTransform t = (RectTransform) transform;
            //t.sizeDelta = new Vector2(t.rect.width - 120, t.rect.height);
        }

        StartCoroutine(Test());
	}

    IEnumerator Test()
    {
        for (float i = 1; i > 0; i-=0.01f) {
            SetHealth(i);
            yield return new WaitForSeconds(0.1f);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPressed(string key, int playerID, bool state)
    {
        //Not nice, but eh
        //TODO: 8 players
        if (state == true)
        {
            imageBtn[playerID - 1].sprite = sprites[KeyStringToSpriteNumber(key)];
            imageBtn[playerID - 1].color = new Color(1, 1, 1, 1);
            currentPress[playerID - 1] = KeyStringToSpriteNumber(key);
        }
        else
        {
            imageBtn[playerID-1].color = new Color(0,0,0,0);
            currentPress[playerID - 1] = -1;
        }
        CheckGlow();
    }

    private void CheckGlow()
    {
        int[] concurrentPresses = currentPress.GroupBy(s => s).Where(g => g.Count() > 1 && g.Key != -1).Select(g => g.Key).ToArray();

        bool attackReady = true;
        int baseAttack = currentPress[0];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int pos = Array.IndexOf(concurrentPresses, currentPress[i]);
            if (pos > -1)
                animator[i].SetBool("Glow", true);
            else
                animator[i].SetBool("Glow", false);
            if (currentPress[i] != baseAttack) attackReady = false;
        }

        if (attackReady && baseAttack!=-1)
        {
            //set atkImage
            StartCoroutine(FlashAttack());
        }
        
    }
    IEnumerator FlashAttack()
    {
        atkImage.color = Color.white;
        panelAnimator.SetBool("Glow", true);
        yield return new WaitForSeconds(1.5f);
        atkImage.color = new Color(0,0,0,0);
        panelAnimator.SetBool("Glow", false);
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
