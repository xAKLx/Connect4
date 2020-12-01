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
    coinSpawner.SpawnCoin(column);
    boardSelectionController.OnSelectColumn(column);
  }
}
