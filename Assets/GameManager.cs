using System.Net.NetworkInformation;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //public Tile[,] map = new Tile[,] { };
    public Tile[] map = new Tile[9];
    //player tile
    Tile one = new Tile();
    //null tile
    Tile zero = new Tile();
    //ai tile
    Tile minusOne = new Tile();

    //max-eval
    public float Alpha;
    //min-eval
    public float Beta;
    //value
    public float Value;
    private void Start()
    {
        one.TileValue = 1;
        zero.TileValue = 0;
        minusOne.TileValue = -1;
        //decide quien empieza 50/50
        //turno maquina -> profundidad 3
        //turno player
        //tests de GetState
        map = new Tile[] {  zero, zero, zero , zero, zero, zero , zero, zero, zero  };

    }

    Tile GetTile(int x, int y)
    {
        return map[x + y * 3];
    }
    public float MinMax(Tile[] mapState, int depth, bool MaximizingPlayer)
    {
        if(depth == 0 && GetState(mapState) != 0)
        {
            //
            return 0;
        }

        if (MaximizingPlayer)
        {
            Alpha = -Mathf.Infinity;
            foreach(Tile tile in mapState)
            {
                if(tile.TileValue == 0)
                {
                    Value = MinMax()
                }
            }
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
        Tile[] mapClone = (Tile[])map.Clone();
        mapClone[move[0]].TileValue = move[1];
        
        return mapClone;
    }
}
