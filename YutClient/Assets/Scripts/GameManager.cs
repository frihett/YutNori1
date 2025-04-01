using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerType { PlayerA, PlayerB }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int PlayerId;
    public int CurrentTurnPlayerId;


    public List<Piece> myPieces = new List<Piece>();
    public List<Piece> enemyPieces = new List<Piece>(); // ✅ 상대방 말도 저장

    public Piece selectedPiece;

    public Button throwButton; // 윷 버튼 참조

    public MoveUIController moveUI;

    public TMP_Text textPlayerId;
    public TMP_Text textTurnPlayer;

    public int finishCount = 0; // 도착한 말 개수

    public GameObject winPanel;
    public TMP_Text winText;





    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

   

    public void UpdateInfo()
    {
        if (textPlayerId != null)
            textPlayerId.text = $"My ID: {PlayerId}";
        if (winPanel != null)
            winPanel.SetActive(false); // 승리창 시작 시 숨김
    }

    public void AddPiece(Piece piece, int ownerId)
    {
        if (ownerId == PlayerId)
            myPieces.Add(piece);
        else
            enemyPieces.Add(piece); // ✅ 상대 말도 저장

    }

    public void SetTurn(int playerId)
    {
        CurrentTurnPlayerId = playerId;
        UnityEngine.Debug.Log($" 현재 턴 플레이어: {playerId}");

        // UI 버튼 활성화/비활성화
        throwButton.interactable = (PlayerId == playerId);

        if (textTurnPlayer != null)
            textTurnPlayer.text = $"{CurrentTurnPlayerId}'s Turn";
    }

    


    public void OnMoveSelected(int dist)
    {
        if (selectedPiece == null) return;

        int pieceIndex = myPieces.IndexOf(selectedPiece);
        int startIndex = selectedPiece.currentIndex;

        // 🟨 1. 서버로 이동 패킷 보내기
        C_MovePiece pkt = new C_MovePiece();
        pkt.playerId = PlayerId;
        pkt.pieceIndex = pieceIndex;
        pkt.startIndex = startIndex;
        pkt.distance = dist;

        NetworkManager.Instance.Send(pkt.Write());


        //if (selectedPiece.currentIndex == -1)
        //    selectedPiece.StartMoveFromStart(dist);
        //else
        //    selectedPiece.EnqueueMove(dist);

        // 👉 말이 움직이고 나면 자동으로 턴 넘기기
        selectedPiece.onMoveEnd += () =>
        {
            // 1. 서버에 턴 변경 요청 보내기
            C_TurnChange pkt = new C_TurnChange();
            pkt.playerId = GameManager.Instance.PlayerId;
            NetworkManager.Instance.Send(pkt.Write());

            // (선택) 2. 클라이언트 로컬에서는 UI 잠깐 비활성화
            throwButton.interactable = false;

        };
    }

    public Piece GetPiece(int playerId, int pieceIndex)
    {
        if (playerId == PlayerId)
        {
            if (pieceIndex >= 0 && pieceIndex < myPieces.Count)
                return myPieces[pieceIndex];
        }
        else
        {
            if (pieceIndex >= 0 && pieceIndex < enemyPieces.Count)
                return enemyPieces[pieceIndex];
        }

        return null;
    }

    public void OnPieceFinished(Piece piece)
    {
        if (piece.ownerId != PlayerId)
            return;

        finishCount++;

        UnityEngine.Debug.Log($" 도착한 말 개수: {finishCount}");

        if (finishCount >= 4)
        {
            UnityEngine.Debug.Log($" 플레이어 {PlayerId} 승리!");
            C_GameWin pkt = new C_GameWin();
            pkt.playerId = PlayerId;
            NetworkManager.Instance.Send(pkt.Write());
            // TODO: 승리 UI 띄우기, 게임 종료 처리 등
        }
    }

    public void ShowWinUI(int winnerId)
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        if (winText != null)
            winText.text = $" Player {winnerId} win!";
    }


}