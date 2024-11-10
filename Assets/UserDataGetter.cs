using TMPro;
using UnityEngine;
using static Dissonity.Api;

public class UserDataGetter : MonoBehaviour
{
    [SerializeField] private TMP_Text userIdTextUI;
    [SerializeField] private TMP_Text serverInfoTextUI;

    async void GetUserData()
    {
        var user = await GetUser();
        userIdTextUI.SetText(user.username);
        
        SubActivityInstanceParticipantsUpdate((data) => {
            Debug.Log("Received a participants update!");
        });

        var channel = await GetChannel(await GetChannelId());
        serverInfoTextUI.SetText(channel.name);
    }
}
