using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Dissonity.Api;

public class UserDataGetter : MonoBehaviour
{
    private TMP_Text _userIdTextUI;
    private TMP_Text _currentChannelTextUI;
    private TMP_Text _currentServerTextUI;

    [SerializeField] private Transform serverInfoPanel;
    [SerializeField] private GameObject serverInfoPrefab;
    [SerializeField] private Transform currentInfoPanel;
    [SerializeField] private GameObject textPrefab;

    [SerializeField] private Image currentServerImageUI;
    [SerializeField] private DiscordIconLoader discordIconLoader;

    async void Start()
    {
        _currentServerTextUI = Instantiate(textPrefab, currentInfoPanel).GetComponent<TMP_Text>();
        _currentChannelTextUI = Instantiate(textPrefab, currentInfoPanel).GetComponent<TMP_Text>();
        _userIdTextUI = Instantiate(textPrefab, currentInfoPanel).GetComponent<TMP_Text>();

        var user = await GetUser();

        SubActivityInstanceParticipantsUpdate((data) => {
            Debug.Log("Received a participants update!");
        });

        var guildList = await GetGuildList();

        foreach (var guild in guildList.guilds)
        {
            ServerInfo serverInfo = Instantiate(serverInfoPrefab, serverInfoPanel).GetComponent<ServerInfo>();
            serverInfo.ServerNameUI.SetText(guild.name);

            if (string.IsNullOrWhiteSpace(guild.icon))
            {
                serverInfo.ServerIcon.enabled = false;
                continue;
            }

            LoadGuildIcon(guild.id, guild.icon, (sprite) =>
            {
                serverInfo.ServerIcon.sprite = sprite;
            });
        }

        string currentChannelId = await GetChannelId();
        var currentChannel = await GetChannel(currentChannelId);

        var currentGuildID = await GetGuildId();
        var currentGuild = guildList.guilds.First(g => g.id == currentGuildID);

        if (!string.IsNullOrWhiteSpace(currentGuild.icon))
        {
            LoadGuildIcon(currentGuild.id, currentGuild.icon, (sprite) =>
            {
                currentServerImageUI.sprite = sprite;
            });
        } else
        {
            currentServerImageUI.enabled = false;
        }


        _currentServerTextUI.SetText($"Current Server: {currentGuild.name}");
        _currentChannelTextUI.SetText($"Current Channel: {currentChannel.name}");
        _userIdTextUI.SetText($"Username: {user.username}");
    }

    private void LoadGuildIcon(string guildId, string iconId, System.Action<Sprite> callback)
    {
        discordIconLoader.LoadDiscordIcon(guildId, iconId, callback);
    }
}
