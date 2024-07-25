using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightUnarmed : Player
{
    protected TextHandler textHandler;
    protected Shop shop;
    protected Quest quest;
    private bool shopInteractable = false;
    private bool outpostInteractable = false;
    private bool outpostDialog = true;

    protected override void StartChildren()
    {
        this.walkingSpeed = 120f;
        this.runningSpeed = 180f;
        this.jumpSpeed = 50f;
        this.hp = 350f;
        this.maxHp = 350f;
        this.SetPlayerActive(true);
        this.walkingTerrain = "forest";
        textHandler = GetComponent<TextHandler>();
        shop = Shop.Instance;
        quest = Quest.Instance;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "ShopZone")
        {
            textHandler.ShopEnter();
        }
        else if (other.name == "OutpostZone")
        {
            textHandler.OutpostEnter();
            outpostDialog = true;
        }
        else if (other.name == "ShopEnter")
        {
            textHandler.ShopInside();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            animator.SetBool("inWater", true);
            walkingTerrain = "water";
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "ShopInteract")
        {
            textHandler.InteractText(true);
            shopInteractable = true;
        }
        else if (other.name == "OutpostInteract")
        {
            if(outpostDialog)
                AudioManager.Instance.Dialog("questGreet");
            outpostDialog = false;
            textHandler.InteractText(true);
            outpostInteractable = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "ShopInteract")
        {
            textHandler.InteractText(false);
            shopInteractable = false;
            shop.ShowShop(false);
        }
        else if (other.name == "OutpostInteract")
        {
            textHandler.InteractText(false);
            outpostInteractable = false;
            quest.ShowQuest(false);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            animator.SetBool("inWater", false);
            walkingTerrain = "forest";
        }
    }

    protected override void UpdateChildren()
    {
        if (shopInteractable && Input.GetKeyDown(KeyCode.F))
        {
            shop.ShowShop(true);
        }
        if (outpostInteractable && Input.GetKeyDown(KeyCode.F))
        {
            quest.ShowQuest(true);
        }
    }

    public override void AttackSound(int variation)
    {

    }

    protected override void FixedUpdateChildren()
    {

    }

    protected override void BasicAttack()
    {

    }

    protected override void HeavyAttack()
    {

    }

    protected override void SpecialAttack()
    {

    }
}
