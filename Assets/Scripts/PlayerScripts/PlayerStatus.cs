using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerStatus : Fighter
{
    private Inventory inventory;
    public string playerName;
    public int experience;
    public HealthBar healthBar;

    private Ragdoll ragdoll;
    private PlayerController playerController;
    public bool ShopOpen { get; private set; }
    protected override void Start()
    {
        inventory = GetComponent<Inventory>();
        ragdoll = GetComponent<Ragdoll>();
        playerController = GetComponent<PlayerController>();

        healthBar.SetMaxHealth(maxHealth);
        ShopOpen = false;
    }

    private void OnEnable()
    {
        PlayerActions.OnShopOpen += OnShopOpen;    
        PlayerActions.OnShopClose += OnShopClose;
    }

    private void OnDisable()
    {
        PlayerActions.OnShopOpen -= OnShopOpen;
        PlayerActions.OnShopClose -= OnShopClose;
    }

    //Testing status sync with hp bar when manually adjusting hp to inspector
    public void Update()
    {
        healthBar.SetHealth(health);
    }

    public void Rewarded(int g, int e)
    {
        inventory.IncreaseGold(g);
        experience += e;
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        healthBar.SetHealth(health);
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        healthBar.SetHealth(health);
    }

    public override void ReceiveHealing(int healing)
    {
        base.ReceiveHealing(healing);
        healthBar.SetHealth(health);
    }

    public bool IsMaxHealth()
    {
        return health >= maxHealth;
    }

    protected override void Death()
    {
        //GetComponent<QuestList>().CheckQGoal(this);
        if (ragdoll != null)
        {
            dead = true;
            playerController.enabled = false;
            ragdoll.ActivateRagdoll();
            GameManager.instance.PlayerDeath(5);
        }
    }

    public void OnShopOpen()
    {
        ShopOpen = true;
    }

    public void OnShopClose()
    {
        ShopOpen = false;
    }
}