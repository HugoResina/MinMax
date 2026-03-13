using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tile[,] map = new Tile[,] { };
    Tile one = new Tile();
    Tile zero = new Tile();
    Tile two = new Tile();
    private void Start()
    {
        one.TileValue = 1;
        zero.TileValue = 0;
        two.TileValue = 2;
        //decide quien empieza 50/50
        //turno maquina -> profundidad 3
        //turno player
        //tests de GetState
        /*
        Debug.Log("deberia ser 1: " + GetState(new Tile[,] { { one, one, one }, { zero, zero, zero }, { zero, zero, zero } }));
        Debug.Log("deberia ser 1: " + GetState(new Tile[,] { { zero, zero, zero }, { zero, zero, zero }, { one, one, one } }));
        Debug.Log("deberia ser 2: " + GetState(new Tile[,] { { zero, zero, zero }, { two, two, two }, { zero, zero, zero } }));
        Debug.Log("deberia ser 0: " + GetState(new Tile[,] { { zero, one, zero }, { two, zero, two }, { one, zero, two } }));
        Debug.Log("deberia ser 3: " + GetState(new Tile[,] { { one, two, one }, { two, two, one }, { one, one, two } }));
        */
        map = new Tile[,] { { zero, zero, zero }, { zero, zero, zero }, { zero, zero, zero } };

    }

    public void MinMax(Tile[,] mapState, int depth, bool MaximizingPlayer)
    {
        if(depth == 0 && GetState(mapState) != 0)
        {
            //
            return;
        }
    }

    public int GetState(Tile[,] mapState)
    {
        //returns 1 if the player wins, 2 for the machine, 0 if the game is ongoing and 3 if it's a draw

       
        for (int i = 0; i < mapState.GetLength(0); i++)
        {
            //comprova files
            if ((mapState[i, 0].TileValue & mapState[i, 1].TileValue & mapState[i, 2].TileValue) == mapState[i, 0].TileValue && mapState[i, 0].TileValue != 0)
            {
                return mapState[i, 0].TileValue;
            }
            //comprova columnes
            if ((mapState[0, i].TileValue & mapState[1, i].TileValue & mapState[2, i].TileValue) == mapState[0, i].TileValue && mapState[0, i].TileValue != 0)
            {
                return mapState[i, 0].TileValue;
            }
            

        }
        //comprova diagonals
        if((mapState[0, 0].TileValue & mapState[1, 1].TileValue & mapState[2, 2].TileValue) == mapState[1, 1].TileValue && mapState[1, 1].TileValue != 0)
        {
            return mapState[0, 0].TileValue;
        }
        if ((mapState[0, 2].TileValue & mapState[1, 1].TileValue & mapState[2, 0].TileValue) == mapState[1, 1].TileValue && mapState[1, 1].TileValue != 0)
        {
            return mapState[1, 1].TileValue;
        }

        //si no detecta cap victoria mira si queden posicions buides i si en troba es que el joc encara esta en curs
        for (int i = 0; i < mapState.GetLength(0); i++)
        {
            for(int j = 0; j < mapState.GetLength(1); j++)
            {
                if (mapState[i,j].TileValue == 0)
                {
                    return 0;
                }
            }
        }

        // si ningu ha guanyat i el joc encara no segueix en curs, es un empat
            return 3;
    }
}
