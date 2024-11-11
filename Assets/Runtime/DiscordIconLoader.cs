using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DiscordIconLoader : MonoBehaviour
{
    // Delegate for icon loaded event
    public Action<Sprite> OnIconLoaded;

    // Method to load Discord icon with a given URL
    public void LoadDiscordIcon(string guildId, string iconId, Action<Sprite> onIconLoaded)
    {
        string iconUrl = $"https://cdn.discordapp.com/icons/{guildId}/{iconId}.png?size=128";
        StartCoroutine(LoadIconCoroutine(iconUrl, onIconLoaded));
    }

    private IEnumerator LoadIconCoroutine(string iconUrl, Action<Sprite> onIconLoaded)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(iconUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load icon: " + request.error);
            onIconLoaded?.Invoke(null); // Invoke with null if loading fails
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite iconSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            onIconLoaded?.Invoke(iconSprite); // Invoke event with loaded sprite
        }
    }

}
