using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

// InventoryManager.cs
// Josiah Nistor
// Handels and displays rewources and items from all storage areas
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int m_gold;
    [SerializeField] private int m_food;
    [SerializeField] private int m_drinks;
    [SerializeField] private int m_wood;
    [SerializeField] private int m_stone;
    [SerializeField] private int m_fiber;
    [SerializeField] private int m_leather;
    [SerializeField] private int m_metal;

    [SerializeField] private List<Object> m_objects = new List<Object>();

    [SerializeField] private TMP_Text m_goldText;
    [SerializeField] private TMP_Text m_foodsText;
    [SerializeField] private TMP_Text m_drinksText;
    [SerializeField] private TMP_Text m_woodText;
    [SerializeField] private TMP_Text m_stoneText;
    [SerializeField] private TMP_Text m_fiberText;
    [SerializeField] private TMP_Text m_leatherText;
    [SerializeField] private TMP_Text m_metalText;

    [SerializeField] private GameObject m_itemInvenvtory;
    [SerializeField] private GameObject m_items;
    [SerializeField] private GameObject m_itemSlotPrefab;

    [SerializeField] private WorldManager m_worldManager;


    private void Update()
    {
        m_gold = 0;
        m_food = 0;
        m_drinks = 0;
        m_wood = 0;
        m_stone = 0;
        m_fiber = 0;
        m_leather = 0;
        m_metal = 0;

        foreach (GameObject storageLocation in m_worldManager.GetStorageLocations())
        {
            StorageArea storage = storageLocation.GetComponent<StorageArea>();
            m_gold += storage.GetGold();
            m_food += storage.GetFood();
            m_drinks += storage.GetDrinks();
            m_wood += storage.GetWood();
            m_stone += storage.GetStone();
            m_fiber += storage.GetFiber();
            m_leather += storage.GetLeather();
            m_metal += storage.GetMetal();

            List<Object> objects = storage.GetObjects();
            foreach (Object newObj in objects)
            {
                bool same = false;
                foreach (Object obj in m_objects)
                {
                    if (newObj == obj)
                        same = true;
                }
                if (!same)
                {
                    m_objects.Add(newObj);
                    GameObject newItemSlot = Instantiate(m_itemSlotPrefab, m_items.transform);
                    Transform newItemSlotTransform = newItemSlot.transform;
                    newItemSlotTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newObj.GetName();
                    newItemSlotTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "This craft made of " + newObj.GetMaterial().GetMaterial().ToString() + " and is of " + newObj.GetDiscription() + " quality";
                    newItemSlotTransform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Value: " + newObj.GetValue();
                    newItemSlotTransform.GetChild(5).GetComponent<Button>().onClick.AddListener( () => SellItem(newObj, newItemSlot));
                }            
            }
        }

        m_goldText.text = "Total Gold: " + m_gold.ToString();
        m_foodsText.text = "Total Food: " + m_food.ToString();
        m_drinksText.text = "Total Drinks: " + m_drinks.ToString();
        m_woodText.text = "Total Wood: " + m_wood.ToString();
        m_stoneText.text = "Total Stone: " + m_stone.ToString();
        m_fiberText.text = "Total Fiber: " + m_fiber.ToString();
        m_leatherText.text = "Total Leather: " + m_leather.ToString();
        m_metalText.text = "Total Metal: " + m_metal.ToString();
    }

    // Getters
    public int GetFood() { return m_food; }
    public int GetDrinks() { return m_drinks; }
    public int GetWood() { return m_wood; }
    public int GetStone() { return m_stone; }
    public int GetFiber() { return m_fiber; }
    public int GetLeather() { return m_leather; }
    public int GetMetal() { return m_metal; }

    // Update amount of material and type from the first availible storage
    public void UpdateMaterial(int amount, type matType)
    {
        foreach (GameObject storageLocation in m_worldManager.GetStorageLocations())
        {
            StorageArea storage = storageLocation.GetComponent<StorageArea>();
            bool inStorage = false;

            if (matType == type.Metal && storage.GetMetal() > 0)
                inStorage = true;
            else if (matType == type.Leather && storage.GetLeather() > 0)
                inStorage = true;
            else if (matType == type.Fiber && storage.GetFiber() > 0)
                inStorage = true;
            else if (matType == type.Stone && storage.GetStone() > 0)
                inStorage = true;
            else if (matType == type.Wood && storage.GetWood() > 0)
                inStorage = true;

            if (inStorage)
            {
                storage.UpdateMaterial(amount, matType);
                break;
            }
        }
    }

    // Sell an item, remove it from your inventory and give you its value in gold
    public void SellItem(Object obj, GameObject self)
    {
        foreach (GameObject storageLocation in m_worldManager.GetStorageLocations())
        {
            StorageArea storage = storageLocation.GetComponent<StorageArea>();
            if (storage.GetObjects().Contains(obj))
            {
                storage.UpdateGold(obj.GetValue());
                m_objects.Remove(obj);
                Destroy(obj.GetMaterial().gameObject);
                storage.GetObjects().Remove(obj);
                Destroy(self);
            }
        }
    }

    // UI Button to toggle item inventory
    public void ToggleItemInventory()
    {
        if (m_itemInvenvtory.activeSelf)
            m_itemInvenvtory.SetActive(false);
        else
            m_itemInvenvtory.SetActive(true);
    }
}
