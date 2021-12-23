using UnityEngine;

public class audioController : MonoBehaviour
{

    public static audioController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
