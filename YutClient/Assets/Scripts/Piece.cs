using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int currentIndex = -1;         // 보드 상 위치
    public int ownerId;                   // 소유 플레이어 ID 추가 ✅
    private int previousIndex = -1; // 이전 노드를 추적


    private Queue<int> moveQueue = new Queue<int>();
    private bool isMoving = false;

    public List<Piece> stackedPieces = new List<Piece>(); // ✅ 같이 이동할 말들

    public System.Action onMoveEnd; // ✅ 이동 종료 콜백

    public void EnqueueMove(int dist)
    {
        List<Piece> allPieces = new List<Piece>(stackedPieces);
        if (!allPieces.Contains(this))
            allPieces.Add(this);

        foreach (var piece in allPieces)
        {
            piece.moveQueue.Enqueue(dist);

            if (!piece.isMoving)
                piece.StartCoroutine(piece.MoveRoutine());
        }

    }

    public void StartMoveFromStart(int dist)
    {
        currentIndex = 0;
        transform.position = BoardManager.Instance.boardNodes[0].position;
        EnqueueMove(dist);
    }

    private IEnumerator MoveRoutine()
    {
        isMoving = true;

        while (moveQueue.Count > 0)
        {
            int dist = moveQueue.Dequeue();

            for (int i = 0; i < dist; i++)
            {
                BoardNode currentNode = BoardManager.Instance.boardNodes[currentIndex];

                int nextIndex;

                currentNode.piecesOnThisNode.Remove(this);

                // ✅ 지름길 우선 처리
                if (currentNode.shortIndex != -1 && i == 0)
                        nextIndex = currentNode.shortIndex;
                    else
                        if (currentIndex == 22)
                        {
                        if (previousIndex == 26)
                            nextIndex = 27; // 26 → 22 → 27
                        else if (previousIndex == 21)
                            nextIndex = 23; // 21 → 22 → 23
                        else
                            nextIndex = currentNode.nextIndex; // fallback
                        }else
                    nextIndex = currentNode.nextIndex;


                if (nextIndex == -1)
                {
                    UnityEngine.Debug.Log($" 플레이어 {ownerId} 말 완주 성공!");
                    GameManager.Instance.OnPieceFinished(this); 
                    gameObject.SetActive(false);
                    break;
                }
                Vector3 target = BoardManager.Instance.boardNodes[nextIndex].position;

                // 이동 애니메이션
                while (Vector3.Distance(transform.position, target) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5f);
                    yield return null;
                }
                previousIndex = currentIndex;
                currentIndex = nextIndex;

                BoardNode nextNode = BoardManager.Instance.boardNodes[nextIndex];
                nextNode.piecesOnThisNode.Add(this);

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.2f);
        }

        isMoving = false;

        // ✅ 이동 끝났으면 콜백 실행
        onMoveEnd?.Invoke();
        onMoveEnd = null;
        UnityEngine.Debug.Log("콜백 실행");

        TryStackOnCurrentNode(); // ✅ 이동 후 스택 처리

    }

    void TryStackOnCurrentNode()
    {
        var node = BoardManager.Instance.boardNodes[currentIndex];

        foreach (var other in node.piecesOnThisNode)
        {
            if (other == this) continue;
            if (other.ownerId == this.ownerId && !stackedPieces.Contains(other))
            {
                // 서로 연결
                stackedPieces.Add(other);
                other.stackedPieces.Add(this);

                UnityEngine.Debug.Log($"[STACK] {this.name}와 {other.name}가 쌓임!");
            }
        }
    }


    private void OnMouseDown()
    {
        UnityEngine.Debug.Log($" Piece 클릭됨!  OwnerID: {ownerId}, CurrentIndex: {currentIndex}, Position: {transform.position}");

        // 예시: 클릭 시 GameManager에 선택 알림
        GameManager.Instance.selectedPiece = this;
    }


   

}