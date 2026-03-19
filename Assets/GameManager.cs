using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Tile[] map = new Tile[9];

    public bool playerTurn = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Restart()
    {
        for (int i = 0; i < map.Length; i++)
        {
            map[i].index = i;
            map[i].TileValue = 0;
            map[i].UpdateVisual();
        }
        playerTurn = true;
    }

    public void OnTileClicked(Tile tile)
    {
        if (!playerTurn || tile.TileValue != 0) return;

        tile.TileValue = 1;
        tile.UpdateVisual();

        if (CheckGame()) return;

        playerTurn = false;
        Invoke(nameof(AITurn), 0.5f);
    }

    bool CheckGame()
    {
        int[] currentBoard = new int[9];
        for (int i = 0; i < 9; i++) currentBoard[i] = map[i].TileValue;

        int state = GetState(currentBoard);
        if (state != 0)
        {
            if (state == 1) Debug.Log("Guanya el jugador");
            else if (state == -1) Debug.Log("Guanya la IA");
            else if (state == 3) Debug.Log("Empat");
            return true;
        }
        return false;
    }

   
    public float MinMax(int[] board, int depth, float alpha, float beta, bool isMaximizing)
    {
        int state = GetState(board);
        //si hi ha un guanyador o hi ha un empat es retorna el estat 
        if (state != 0) return (state == 3) ? 0 : state;

        if (isMaximizing)
        {
            float bestEval = -Mathf.Infinity;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = 1;
                    float eval = MinMax(board, depth - 1, alpha, beta, false);
                    board[i] = 0;

                    bestEval = Mathf.Max(bestEval, eval);
                    alpha = Mathf.Max(alpha, eval);

                    
                    if (beta <= alpha) break;
                }
            }
            return bestEval;
        }
        else
        {
            float bestEval = Mathf.Infinity;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = -1;
                    float eval = MinMax(board, depth - 1, alpha, beta, true);
                    board[i] = 0;

                    bestEval = Mathf.Min(bestEval, eval);
                    beta = Mathf.Min(beta, eval);

                    
                    if (beta <= alpha) break;
                }
            }
            return bestEval;
        }
    }

    void AITurn()
    {
        int bestMove = -1;
        float bestValue = Mathf.Infinity;

        int[] currentBoard = new int[9];
        for (int i = 0; i < 9; i++) currentBoard[i] = map[i].TileValue;

        for (int i = 0; i < 9; i++)
        {
            if (currentBoard[i] == 0)
            {
                currentBoard[i] = -1;
                
                float moveValue = MinMax(currentBoard, 9, -Mathf.Infinity, Mathf.Infinity, true);
                currentBoard[i] = 0;

                if (moveValue < bestValue)
                {
                    bestValue = moveValue;
                    bestMove = i;
                }
            }
        }

        if (bestMove != -1)
        {
            map[bestMove].TileValue = -1;
            map[bestMove].UpdateVisual();
        }

        if (!CheckGame()) playerTurn = true;
    }

    public int GetState(int[] mapState)
    {
        for (int i = 0; i < 3; i++)
        {
            if (mapState[i * 3] != 0 && mapState[i * 3] == mapState[i * 3 + 1] && mapState[i * 3] == mapState[i * 3 + 2])
                return mapState[i * 3];
            if (mapState[i] != 0 && mapState[i] == mapState[i + 3] && mapState[i] == mapState[i + 6])
                return mapState[i];
        }
        if (mapState[4] != 0)
        {
            if (mapState[0] == mapState[4] && mapState[4] == mapState[8]) return mapState[4];
            if (mapState[2] == mapState[4] && mapState[4] == mapState[6]) return mapState[4];
        }
        foreach (int val in mapState) if (val == 0) return 0;
        return 3;
    }
}