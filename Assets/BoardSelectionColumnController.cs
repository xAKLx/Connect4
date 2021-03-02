using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSelectionColumnController : MonoBehaviour
{
  public int column;
  public CoinSpawnerController coinSpawner;
  public BoardSelectionController boardSelectionController;

  private void OnMouseEnter()
  {
    if(Board.inWinScreen) return;
    coinSpawner.SpawnCoin(column, boardSelectionController.currentPlayer == 1 ? boardSelectionController.player1Color : boardSelectionController.player2Color);
    boardSelectionController.OnSelectColumn(column);
  }
}
