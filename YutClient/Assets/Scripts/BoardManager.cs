using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BoardNode
{
    public int nodeIndex;
    public Vector3 position;
    public int nextIndex;
    public int shortIndex = -1; // 10 에서 지름길을 탄 경우 1 , 1 일때 25, 26 ,22로 왓을대 shortIndex로 가게끔 처리 
    public GameObject visual; // 노드 GameObject

    public List<Piece> piecesOnThisNode = new List<Piece>();
}

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public GameObject nodePrefab;
    public List<BoardNode> boardNodes = new List<BoardNode>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        InitBoard();

    }

    void Start()
    {
    }

    void InitBoard()
    {
        // 경로
        for (int i = 0; i < 30; i++)
        {
            Vector3 pos = GetNodePosition(i);  // 좌표 생성
            GameObject go = Instantiate(nodePrefab, pos, Quaternion.identity);
            go.name = $"Node_{i}";

            BoardNode node = new BoardNode();
            node.nodeIndex = i;
            node.position = pos;
            if (i < 29)
                node.nextIndex = i + 1;
            if (i == 5)
                node.shortIndex = 20;
            if (i == 10)
                node.shortIndex = 25;
            if (i == 22)
                node.shortIndex = 27;
            if (i == 26)
                node.nextIndex = 22;


            if (i == 19)
                node.nextIndex = 29;
            if (i == 24)
                node.nextIndex = 15;

            if (i == 28)
                node.nextIndex = 29;
            if (i == 29)
                node.nextIndex = -1;

            node.visual = go;

            boardNodes.Add(node);
        }

       
        UnityEngine.Debug.Log(" Board Initialization Complete!");
    }

    void AddNode(int idx, Vector3 pos, int next)
    {
        GameObject go = Instantiate(nodePrefab, pos, Quaternion.identity);
        go.name = $"Node_{idx}";

        BoardNode node = new BoardNode();
        node.nodeIndex = idx;
        node.position = pos;
        node.nextIndex = next;
        node.visual = go;

        boardNodes.Add(node);
    }

    Vector3 GetNodePosition(int index)
    {
        // 간단한 예: 시계방향 배치
        //if (index < 5) return new Vector3(index, 5, 0);
        //if (index < 10) return new Vector3(5, 3 - (index - 5), 0);
        //if (index < 15) return new Vector3(5 - (index - 10), -2, 0);
        //return new Vector3(0, -2 + (index - 15), 0);
        if (index < 6) return new Vector3(2.5f , index, 0) / 1.3f;
        if (index < 11) return new Vector3(2.5f - 1 - (index - 6) , 5, 0) / 1.3f;
        if (index < 16) return new Vector3(-2.5f, 5 -1 -(index - 11 ), 0) / 1.3f;
        if (index < 20) return new Vector3(-2.5f +1 +(index -16), 0, 0) / 1.3f;


        // return new Vector3(0, -2 + (index - 15), 0);
        //  지름길 
        if(index == 20)
        {
            return new Vector3(1.6f ,2.5f + 1.6f, 0) / 1.3f;
        }
        if (index == 21)
        {
            return new Vector3(0.8f ,2.5f + 0.8f, 0) / 1.3f;
        }
        if (index == 22)
        {
            return new Vector3(0, 2.5f , 0) / 1.3f;
        }
        if (index == 23)
        {
            return new Vector3(-0.8f , 2.5f - 0.8f, 0) / 1.3f;
        }
        if (index == 24)
        {
            return new Vector3(-1.6f , 2.5f- 1.6f, 0) / 1.3f;
        }
        if (index == 25)
        {
            return new Vector3(-1.6f,  2.5f + 1.6f, 0) / 1.3f;
        }
        if (index == 26)
        {
            return new Vector3(-0.8f,  2.5f + 0.8f, 0) / 1.3f;
        }
        if (index == 27)
        {
            return new Vector3(0.8f,  2.5f - 0.8f, 0) / 1.3f;
        }
        if (index == 28)
        {
            return new Vector3(1.6f, 2.5f - 1.6f, 0) / 1.3f;
        }
        if ( index == 29)
        {
            return new Vector3(2.5f, 0, 0) / 1.3f;
        }

        return new Vector3();

    }
}