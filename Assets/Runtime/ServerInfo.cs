using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text serverNameUI;
    [SerializeField] private Image serverIcon;

    public TMP_Text ServerNameUI { get { return serverNameUI; } }
    public Image ServerIcon { get { return serverIcon; } }
}
