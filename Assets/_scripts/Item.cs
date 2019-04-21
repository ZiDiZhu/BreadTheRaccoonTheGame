﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject instruction;

    public string itemName;
    public Sprite itemSprite;

    bool interactable;
    public bool canPickUp;

    // Start is called before the first frame update
    void Start()
    {
        print("Apple!");
        instruction = GameObject.Find("Instructions");
        interactable = false;
        //instruction.SetActive(false);
        itemSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Inventory.inventoryOpened)
        {
            interactable = true;
            //instruction = GameObject.Find("Instructions");
            Inventory.ShowInstruction();
            PlayerBehaviors.itemsAround.Add(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Inventory.inventoryOpened)
        {
            interactable = false;
            //instruction = GameObject.Find("Instructions");
            Inventory.HideInstruction();
            PlayerBehaviors.itemsAround.Remove(gameObject);
        }
    }

    public string GetItemName()
    {
        return itemName;
    }

    public void HideItem()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void ShowItem()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public void RemoveItemFromItemsAround()
    {
        PlayerBehaviors.itemsAround.Remove(gameObject);
    }

    public void DropItem()
    {
        GameObject player = GameObject.Find("Player");
        transform.position = player.transform.position;
        ShowItem();
    }

    public void TakeEffect()
    {
        if (gameObject.CompareTag("Food"))
        {
            int[] effects = ItemWiki.ReturnEffects(itemName);

            PlayerBehaviors.health += effects[0];
            PlayerBehaviors.health = Mathf.Clamp(PlayerBehaviors.health, 0, 100);

            PlayerBehaviors.hunger += effects[1];
            PlayerBehaviors.hunger = Mathf.Clamp(PlayerBehaviors.hunger, 0, 100);

            PlayerBehaviors.sanity += effects[2];
            PlayerBehaviors.sanity = Mathf.Clamp(PlayerBehaviors.sanity, 0, 100);

            DestroyItem();
        }
    }

    public void AddToInventory()
    {
        if (gameObject.CompareTag("Food") || gameObject.CompareTag("Item"))
        {
            if(Inventory.inventorySpace > 0)
            {
                //Item copy = new Item(); 
                //copy.itemSprite = itemSprite;
                
                for (int i = 0; i < Inventory.inventory.Length; i++)
                {
                    if (Inventory.inventory[i] == null)
                    {
                        Inventory.inventory[i] = gameObject;
                        //Inventory.inventoryItemName[i] = itemName; 
                        Inventory.inventorySpace -= 1;
                        break;
                    }
                }
                GameObject.Find("Inventory").GetComponent<Inventory>().UpdateInventory();
                PlayerBehaviors.itemsAround.Remove(gameObject);
                HideItem();
            }
        }
    }
}
