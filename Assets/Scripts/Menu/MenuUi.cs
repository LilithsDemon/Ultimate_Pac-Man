using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuUi : MonoBehaviour
{
    public Animator shopMenu;
    public Animator accountMenu;

    public Text coinText;

    public InputField nameInput;

    public void Start()
    {
        nameInput.text = FindObjectOfType<SaveManager>().Name();
        UpdateCoins();
    }

    public void UpdateCoins()
    {
        coinText.text = FindObjectOfType<SaveManager>().Coins().ToString();
    }

    public void OpenAccountMenu()
    {
        accountMenu.SetTrigger("OpenMenu");
    }

    public void CloseAccountMenu()
    {
        accountMenu.SetTrigger("CloseMenu");
    }

    public void UpdatePlayerSettings()
    {
        FindObjectOfType<SaveManager>().SetName(nameInput.text);
    }

    public void OpenShopMenu()
    {
        shopMenu.SetTrigger("OpenMenu");
        FindObjectOfType<Shop>().selectTab(0);
    }

    public void CloseShopMenu()
    {
        shopMenu.SetTrigger("CloseMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(3);
    }

    public void Multiplayer()
    {
        SceneManager.LoadScene(1);
    }
}