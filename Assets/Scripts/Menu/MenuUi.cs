using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuUi : MonoBehaviour
{
    public Animator shopMenu;
    public Animator playMenu;
    private bool playMenuOpen = false;

    public void OpenPlayMenu()
    {
        playMenu.SetTrigger("OpenPlayMenu");
        playMenuOpen = true;
    }

    private void ClosePlayMenu()
    {
        playMenu.SetTrigger("ClosePlayMenu");
    }

    public void OpenShopMenu()
    {
        shopMenu.SetTrigger("OpenMenu");
    }

    public void CloseShopMenu()
    {
        shopMenu.SetTrigger("CloseMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {  
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject.tag == "gameMenu")
            {
                return;
            }
            else
            {
                if (playMenuOpen == true)
                {
                    ClosePlayMenu();
                    playMenuOpen = false;
                }
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {  
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && EventSystem.current.currentSelectedGameObject.tag == "gameMenu")
            {
                return;
            }
            else
            {
                if (playMenuOpen == true)
                {
                    ClosePlayMenu();
                    playMenuOpen = false;
                }
            }
        } 
    }
}