using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;
using System.Net.Http.Headers;

public class PlayerListItem : MonoBehaviour
{
    public string PlayerName;
    public int ConnectionID;
    public ulong PlayerSteamID; 
    private bool AvatarReceived;
    public TextMeshProUGUI PlayerNameText; 
    public TextMeshProUGUI PlayerReadyText;
    public bool Ready;
    public RawImage PlayerIcon;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Awake()
    {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    private void ChangeReadyStatus(){
        if(Ready)
        {
            PlayerReadyText.text = "Ready";
            PlayerReadyText.color = Color.green;
        } 
        else 
        {
            PlayerReadyText.text = "Unready";
            PlayerReadyText.color = Color.red;
        }
    }

    void GetPlayerIcon(){
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);

        if(ImageID == -1) { return; }
        PlayerIcon.texture = GetSteamImageAsTexture(ImageID);
    }

    public void SetPlayerValues(){
        PlayerNameText.text = PlayerName;
        ChangeReadyStatus();
        if(!AvatarReceived){
            GetPlayerIcon();
        }
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.m_SteamID == PlayerSteamID){
            PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        } 
        else 
        {
            return;
        }
    }


    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        Debug.Log("Is Valid: " + isValid);
        
        if(isValid){
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if(isValid){
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        AvatarReceived = true;
        return texture;
    }
}
