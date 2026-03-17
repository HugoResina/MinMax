using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    // 0 empty, 1 player, -1 machine

    public int TileValue;

    public int index;

    public Text text;

    public void UpdateVisual()
    {
        if (TileValue == 1)
            text.text = "X";
        else if (TileValue == -1)
            text.text = "O";
        else
            text.text = "";
    }
    //public void OnClick()
    //{
    //    GameManager.Instance.OnTileClicked(this);
    //}
    public void OnClick()
    {
        Debug.Log("CLICK en tile " + index);
        GameManager.Instance.OnTileClicked(this);
    }


}
