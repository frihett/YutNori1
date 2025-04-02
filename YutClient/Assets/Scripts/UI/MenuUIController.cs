using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    GameObject menuSet;

    Button menuButton;
    Button singlePlayerButton;
    Button multiplayerButton;

    GameObject multiplayerOptionsPanel;
    Button randomMatchButton;
    Button createRoomButton;
    Button backButton;

    void Start()
    {
        
        menuSet = GameObject.Find("MenuSet");

        menuButton = menuSet.transform.Find("MenuButton").GetComponent<Button>();
        singlePlayerButton = menuSet.transform.Find("SinglePlayerButton").GetComponent<Button>();
        multiplayerButton = menuSet.transform.Find("MultiplayerButton").GetComponent<Button>();

        multiplayerOptionsPanel = menuSet.transform.Find("MultiplayerOptionsPanel").gameObject;
        UnityEngine.Debug.Log("multiplayerOptionsPanel: " + multiplayerOptionsPanel);

        randomMatchButton = multiplayerOptionsPanel.transform.Find("RandomMatchButton").GetComponent<Button>();
        createRoomButton = multiplayerOptionsPanel.transform.Find("CreateRoomButton").GetComponent<Button>();
        backButton = multiplayerOptionsPanel.transform.Find("BackButton").GetComponent<Button>();

        
        multiplayerButton.onClick.AddListener(OnClickMultiplayer);
        backButton.onClick.AddListener(OnClickBack);
        randomMatchButton.onClick.AddListener(OnClickRandomMatch);



        multiplayerOptionsPanel.SetActive(false);


    }

    void OnClickMultiplayer()
    {
        singlePlayerButton.gameObject.SetActive(false);
        multiplayerButton.gameObject.SetActive(false);
        multiplayerOptionsPanel.SetActive(true);
    }

    void OnClickBack()
    {
        singlePlayerButton.gameObject.SetActive(true);
        multiplayerButton.gameObject.SetActive(true);
        multiplayerOptionsPanel.SetActive(false);
    }

    void OnClickRandomMatch()
    {
        UnityEngine.Debug.Log("RandomMatch ¹öÆ° Å¬¸¯µÊ");

        C_MatchRequest pkt = new C_MatchRequest();
        NetworkManager.Instance.Send(pkt.Write());
    }
}