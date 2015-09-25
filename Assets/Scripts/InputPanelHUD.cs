using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class InputPanelHUD : MonoBehaviour {

    [Range(2, 4)]
    public int numberOfPlayers = 4;

    private Animator panelAnimator;
    private Animator[] animator;
    private Sprite[] sprites;
    private Image[] imageBtn;
    private Image atkImage;
    private int[] currentPress;

    // Use this for initialization
	void Start () {
        panelAnimator = GetComponent<Animator>();
        sprites = Resources.LoadAll<Sprite>("Sprites/circles");
        atkImage = GameObject.Find("AtkImage").GetComponent<Image>();
        
        imageBtn = new Image[numberOfPlayers];
        currentPress = new int[numberOfPlayers];
        animator = new Animator[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject playerPanel = transform.GetChild(i).gameObject;
            imageBtn[i] = playerPanel.transform.GetChild(1).gameObject.GetComponentInChildren<Image>();
            animator[i] = playerPanel.GetComponent<Animator>();
            currentPress[i] = -1;
        }
        
        //just in case
        for (int i = numberOfPlayers; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPressed(string key, int playerID, bool state)
    {
        //Not nice, but eh
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
            case "Up":
                return 4;
            case "Right":
                return 5;
            case "Down":
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
}
