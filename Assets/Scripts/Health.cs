using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int MaxHealth = 100;

    private InputPanelHUD hud;
    private float currentHealth;

	// Use this for initialization
	void Awake() {
        currentHealth = MaxHealth;
	}

    void Start()
    {
        hud = GetComponentInChildren<InputPanelHUD>();
        hud.SetHealth(1);
    }

	// Update is called once per frame
    public void TakeDamage(float value)
    {
        currentHealth -= value;
        hud.SetHealth(currentHealth / (float)MaxHealth);
    }

    public void Heal(float value)
    {
        currentHealth += value;
        hud.SetHealth(currentHealth / (float)MaxHealth);
    }

}
