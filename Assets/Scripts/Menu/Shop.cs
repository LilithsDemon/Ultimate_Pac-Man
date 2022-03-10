using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    private int tabSelected = 0;
    private int skinSelected = 0;
    public GameObject[] shopButtons;    
    public GameObject[] shopPanels;
    public Text selectButtonText;

    // Start is called before the first frame update
    void Start()
    {
        selectTab(0);
    }

    public void selectTab(int tab)
    {
        tabSelected = tab;
        for(int x = 0; x < 4; x++)
        {
            if(x == tabSelected)
            {
                shopPanels[x].SetActive(true);
                for(int i = 0; i < shopPanels[x].transform.childCount; i++)
                {
                    bool owned = FindObjectOfType<SaveManager>().IsSkinOwned(x, i);
                    GameObject child = shopPanels[x].transform.GetChild(i).gameObject;
                    if (owned == true){
                        child.GetComponent<Image>().color = new Color(0f, 1f, 0.4f, 1f);
                        if (i != 0)
                        {
                            child.transform.GetChild(1).gameObject.SetActive(false);
                        }
                        if(i == FindObjectOfType<SaveManager>().CurrentSkin(x))
                        {
                            child.GetComponent<Image>().color = new Color(0.2f, 1f, 1f, 1f);
                        }
                    }
                    else
                    {
                        child.GetComponent<Image>().color = Color.grey;
                    }
                }
                shopButtons[x].GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
            }
            else
            {
                shopPanels[x].SetActive(false);
                shopButtons[x].GetComponent<Image>().color = new Color(0.996649916f, 0f, 0f, 1f);
            }
        }
    }

    public void selectSkin(int skinVal)
    {
        skinSelected = skinVal;
        bool owned = FindObjectOfType<SaveManager>().IsSkinOwned(tabSelected,skinVal);
        if (owned == true)
        {
            selectButtonText.text = "SELECT";
            if (skinVal == FindObjectOfType<SaveManager>().CurrentSkin(tabSelected))
            {
                selectButtonText.text = "CURRENT SKIN";
            }
        }
        else   
        {
            selectButtonText.text = "BUY";
        }
    }

    public void useButton()
    {
        bool owned = FindObjectOfType<SaveManager>().IsSkinOwned(tabSelected,skinSelected);
        if (owned == true)
        {
            FindObjectOfType<SaveManager>().ChangeCurrentSkin(tabSelected, skinSelected);
            selectSkin(skinSelected);
            selectTab(tabSelected);
        }
        else  
        {
            int cost = 0;
            switch (skinSelected / 2)
            {
                case 0:
                    cost = 100;
                    break;
                case 1:
                    cost = 200;
                    break;
                case 2:
                    cost = 500;
                    break;
            }
            if (FindObjectOfType<SaveManager>().Coins() >= cost)
            {
                FindObjectOfType<SaveManager>().BuySkin(tabSelected, skinSelected, cost);
                FindObjectOfType<MenuUi>().UpdateCoins();
                selectSkin(skinSelected);
                selectTab(tabSelected);
            }
        }
    }

}
