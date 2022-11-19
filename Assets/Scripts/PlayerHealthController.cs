using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour 
{ 

    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invincCount;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        //currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCount > 0) 
        {
            invincCount -= Time.deltaTime;
            
            if (invincCount <= 0) for (int i = 0; i < PlayerController.instance.playerSR.Length; i++) PlayerController.instance.playerSR[i].color = new Color(PlayerController.instance.playerSR[i].color.r, PlayerController.instance.playerSR[i].color.g, PlayerController.instance.playerSR[i].color.b, 1f);
        }

    }

    public void DamagePlyaer()
    {
        if (invincCount <= 0)
        {
            currentHealth--;
            AudioManager.instance.PlaySFX(11);
            MakeInvincavle(damageInvincLength);

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

            if (currentHealth <= 0)
            {
                AudioManager.instance.PlaySFX(9);
                PlayerController.instance.gameObject.SetActive(false);

                UIController.instance.deathScreen.SetActive(true);

                AudioManager.instance.PlayGameOver();
            }

            

        }
    }

    public void MakeInvincavle(float length)
    {
        invincCount = length;
        for (int i = 0; i < PlayerController.instance.playerSR.Length; i++) PlayerController.instance.playerSR[i].color = new Color(PlayerController.instance.playerSR[i].color.r, PlayerController.instance.playerSR[i].color.g, PlayerController.instance.playerSR[i].color.b, .5f);
    }

    public void HealPlyaer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
