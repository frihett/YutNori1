using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameSceneController : MonoBehaviour
{
    public GameObject piecePrefab;
    public MoveUIController moveUI;
    public Button throwButton; // 윷 던지기 버튼
    public TMP_Text textPlayerId;
    public TMP_Text textTurnPlayer;
    public GameObject winPanel;
    public TMP_Text winText;


    void Start()
    {
        GameManager.Instance.moveUI = moveUI;

        CreatePlayerPieces(0, new Vector3(-2.0f, -3f, 0)); // Player 0
        CreatePlayerPieces(1, new Vector3(-2.0f, -4f, 0));  // Player 1

        throwButton.onClick.AddListener(OnClickThrowYut);
        GameManager.Instance.throwButton = throwButton;
        GameManager.Instance.textPlayerId = textPlayerId;
        GameManager.Instance.textTurnPlayer = textTurnPlayer;
        GameManager.Instance.winPanel = winPanel;
        GameManager.Instance.winText = winText;
        GameManager.Instance.UpdateInfo();


        GameManager.Instance.SetTurn(0); // 플레이어0부터 시작

    }

    void CreatePlayerPieces(int playerId, Vector3 startPos)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = startPos + new Vector3(i * 0.5f, 0, 0);
            GameObject go = Instantiate(piecePrefab, pos, Quaternion.identity);
            Piece piece = go.GetComponent<Piece>();
            piece.currentIndex = -1; // 아직 보드에 올라가기 전
            piece.ownerId = playerId;

            if (playerId == 0)
                go.GetComponent<SpriteRenderer>().color = Color.blue;
            else
                go.GetComponent<SpriteRenderer>().color = Color.red;

            GameManager.Instance.AddPiece(piece, playerId);
        }
    }
    //void OnThrowYut()
    //{
    //    List<int> results = ThrowYutLogic();

    //    UnityEngine.Debug.Log($" 윷 결과: {string.Join(", ", results)}");

    //    moveUI.ShowMoveOptions(results);
    //}

    //List<int> ThrowYutLogic()
    //{
    //    List<int> result = new List<int>();
    //    bool keepThrowing = true;

    //    while (keepThrowing)
    //    {
    //        int yut = UnityEngine.Random.Range(1, 6); // 1~5 랜덤
    //        result.Add(yut);

    //        // 도(1), 개(2), 걸(3) → 멈춤 / 윷(4), 모(5) → 한 번 더
    //        keepThrowing = (yut == 4 || yut == 5);
    //    }

    //    return result;
    //}

    void OnClickThrowYut()
    {
        UnityEngine.Debug.Log($" YutThrowButton Clicked");

        C_ThrowYut pkt = new C_ThrowYut();
        pkt.playerId = GameManager.Instance.PlayerId;
        NetworkManager.Instance.Send(pkt.Write()); 
    }
}