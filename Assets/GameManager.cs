using System.Net.NetworkInformation;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //public Tile[,] map = new Tile[,] { };
    public Tile[] map = new Tile[9];


    //max-eval
    public float Alpha;
    //min-eval
    public float Beta;
    //value
    public float Value;
    public bool playerTurn = true;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
       
       
       




    }


    public void OnTileClicked(Tile tile)
    {
        if (!playerTurn) return;
        if (tile.TileValue != 0) return;

        // player
        tile.TileValue = 1;
        map[tile.index] = tile;
        tile.UpdateVisual();
        playerTurn = false;

        if (CheckGame()) return;

        // IA
        Invoke(nameof(AITurn), 0.5f);
    }
    void AITurn()
    {
        int bestMove = -1;
        float bestValue = Mathf.Infinity;

        for (int i = 0; i < map.Length; i++)
        {
            if (map[i].TileValue == 0)
            {
                int[] move = new int[] { i, -1 };

                float moveValue = MinMax(Result(move, map), 9, true);

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

        playerTurn = true;

        CheckGame();
    }
    //void CheckGame()
    //{
    //    int state = GetState(map);

    //    if (state == 1)
    //    {
    //        Debug.Log("Guanya el jugador");
    //    }
    //    else if (state == -1)
    //    {
    //        Debug.Log("Guanya la IA");
    //    }
    //    else if (state == 3)
    //    {
    //        Debug.Log("Empat");
    //    }
    //}
    bool CheckGame()
    {
        int state = GetState(map);

        if (state != 0)
        {
            Debug.Log("Fin del juego");
            return true;
        }

        return false;
    }
    public float MinMax(Tile[] mapState, int depth, bool MaximizingPlayer)
    {
        int state = GetState(mapState);

        if (state != 0)
        {
            return state;
        }

        if (depth == 0)
        {
            return 0;
        }

        if (MaximizingPlayer)
        {
            Alpha = -Mathf.Infinity;
            for(int i = 0; i < mapState.Length; i++) 
            {
                if (mapState[i].TileValue == 0)
                {
                    int[] move = new int[] { i, -1 };
                    Value = MinMax(Result(move, mapState), depth - 1, false);
                    Alpha = Mathf.Max(Alpha, Value);
                }
            }
            return Alpha;
        }
        else
        {
            Beta = Mathf.Infinity;
            for (int i = 0; i < mapState.Length; i++)
            {
                if (mapState[i].TileValue == 0)
                {
                    int[] move = new int[] { i, 1 };
                    Value = MinMax(Result(move, mapState), depth - 1, true);
                    Beta = Mathf.Min(Beta, Value);
                }
            }
            return Beta;
        }
    }

    public int GetState(Tile[] mapState)
    {
        //returns 1 if the player wins, -1 if the machine wins, 0 if the game is ongoing and 3 if it's a draw

        for (int i = 0; i < 2; i++)
        {
            //filas
            if ((mapState[0 + i * 3].TileValue &
                 mapState[1 + i * 3].TileValue & 
                 mapState[2 + i * 3].TileValue)  == mapState[0 + i * 3].TileValue && mapState[0 + i * 3].TileValue != 0)
            {
                return mapState[0 + i * 3].TileValue;
            }

            //columnas
            if ((mapState[0 + i].TileValue &
                 mapState[3 + i].TileValue &
                 mapState[6 + i].TileValue) == mapState[0 + i].TileValue && mapState[0 + i].TileValue != 0)
            {
                return mapState[0 + i].TileValue;
            }
        }

        //diagonals
        if ((mapState[0].TileValue &
             mapState[4].TileValue &
             mapState[8].TileValue) == mapState[4].TileValue && mapState[4].TileValue != 0)
        {
            return mapState[4].TileValue;
        }

        if ((mapState[2].TileValue &
             mapState[4].TileValue &
             mapState[6].TileValue) == mapState[4].TileValue && mapState[4].TileValue != 0)
        {
            return mapState[4].TileValue;
        }


        //si no detecta cap victoria mira si queden posicions buides i si en troba es que el joc encara esta en curs
        foreach(Tile t in mapState)
        {
            if (t.TileValue == 0)
            {
                return 0;
            }
        }

        // si ningu ha guanyat i el joc no segueix en curs, es un empat
        return 3;
    }
    public Tile[] Result(int[] move, Tile[] map)
    {
        //move should be [(x*3+y), value -1/0/1]
        //resulting state of taking a move in a given map
        Tile[] mapClone = new Tile[map.Length];

        for (int i = 0; i < map.Length; i++)
        {
            mapClone[i] = new Tile();
            mapClone[i].TileValue = map[i].TileValue;
        }

        mapClone[move[0]].TileValue = move[1];

        return mapClone;
    }
}
