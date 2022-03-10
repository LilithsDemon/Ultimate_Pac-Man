using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveScript state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveScript>(state));
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveScript>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveScript();
            Save();
        }
    }

    public string Name()
    {
        return state.name;
    }

    public void SetName(string name)
    {
        state.name = name;
        Save();
    }

    public int Coins()
    {
        return state.coins;
    }

    public void AddCoins(int coinsToAdd)
    {
        if ((state.coins + coinsToAdd) > 0)
        {
            state.coins = state.coins + coinsToAdd;
        }
        else 
        {
            state.coins = 0;
        }
        Save();
    }

    public int CurrentSkin(int character)
    {
        return (state.currentSkins[character]);
    }

    public void ChangeCurrentSkin(int charVal, int index)
    {
        state.currentSkins[charVal] = index;
        Save();
    }

    public bool IsSkinOwned(int charVal, int index)
    {
        return(state.skinsOwned[charVal] & (1 << index)) != 0;
    }

    public bool BuySkin(int charVal, int index, int cost)
    {
        if (state.coins >= cost)
        {
            state.coins -= cost;
            UnlockSkin(charVal, index);

            Save();
            return true;
        }else
        {
            return false;
        }
    }

    public void UnlockSkin(int charVal, int index)
    {
        state.skinsOwned[charVal] |= 1 << index;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}