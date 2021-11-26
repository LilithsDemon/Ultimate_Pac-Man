using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    LobbyManager manager;

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
