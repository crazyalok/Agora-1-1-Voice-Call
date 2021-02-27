using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using UnityEngine.Android;
public class ChannelManager : MonoBehaviour
{
    public Button joinChannel;
    public Button leaveChannel;
    public Button muteButton;
    private IRtcEngine mRtcEngine = null;
    private string appId = "6d825064795340f6bc050f2b7d805f5c";
    bool isMuted = false;

    void Start()
    {
        muteButton.gameObject.SetActive(false);
        leaveChannel.gameObject.SetActive(false);
        joinChannel.gameObject.SetActive(true);
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        mRtcEngine = IRtcEngine.GetEngine(appId);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_COMMUNICATION);

        mRtcEngine.OnJoinChannelSuccess += (string channelName, uint uid, int elapsed) =>
        {
            muteButton.gameObject.SetActive(true);
            leaveChannel.gameObject.SetActive(true);
            joinChannel.gameObject.SetActive(false);
        };
        mRtcEngine.OnLeaveChannel += (RtcStats stats) =>
        {
            muteButton.gameObject.SetActive(false);
            leaveChannel.gameObject.SetActive(false);
            joinChannel.gameObject.SetActive(true);
        };

    }
    public void MuteButtonTapped()
    {
        string labeltext = isMuted ? "Mute" : "Unmute";
        Text label = muteButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = labeltext;
        }
        isMuted = !isMuted;
        mRtcEngine.EnableLocalAudio(!isMuted);
    }
    public void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
    }
    public void JoinChannel()
    {
        string channelName = "unityagoravoicecall";
        mRtcEngine.JoinChannel(channelName, "extra", 0);
    }

}
