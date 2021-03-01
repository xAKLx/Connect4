using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BoardSelectionController : MonoBehaviour
{
  [Min(1)]
  public int columns = 7;
  public GameObject boardSelectionColumnPrefab;
  public GameObject coinPrefab;
  public CoinSpawnerController coinSpawner;
  public Board board;
  public CoinInstantiator coinInstantiator;
  public Color player1Color = Color.red;
  public Color player1WinColor = Color.red;
  public Color player2Color = Color.yellow;
  public Color player2WinColor = Color.yellow;
  public int currentPlayer = 1;
  int selectedColumn;


  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      var coinPosition = board.AddCoin(selectedColumn, currentPlayer == 1 ? CellStatus.Player1 : CellStatus.Player2);

      if (coinPosition != null)
      {
        coinInstantiator.InstantiatePlayCoin(coinPosition.Value, currentPlayer);
        SwitchPlayer();
        coinSpawner.SpawnCoin(coinPosition.Value.x, currentPlayer == 1 ? player1Color : player2Color);
      }
    }
  }

  private void SwitchPlayer()
  {
    currentPlayer = currentPlayer == 1 ? 2 : 1;
  }

  private void OnValidate()
  {
    if (EditorApplication.isPlayingOrWillChangePlaymode) return;

    foreach (Transform child in this.transform)
    {
      UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
    }

    foreach (var columnIndex in Enumerable.Range(0, columns))
    {
      UnityEditor.EditorApplication.delayCall += () =>
      {
        var boardSelectionColumn = Instantiate(
          boardSelectionColumnPrefab,
          new Vector3(columnIndex - (columns / 2.0f) + 0.5f, 0.0f, 0.0f),
          Quaternion.identity
        );
        SetupBoardSelectionColumn(boardSelectionColumn, columnIndex);
      };
    }

  }

  void SetupBoardSelectionColumn(GameObject boardSelectionColumn, int columnIndex)
  {
    boardSelectionColumn.transform.parent = transform;
    boardSelectionColumn.transform.localScale = new Vector3(1, 1, 1);
    var boardSelectionColumnController = boardSelectionColumn.GetComponent<BoardSelectionColumnController>();
    boardSelectionColumnController.column = columnIndex;
    boardSelectionColumnController.coinSpawner = coinSpawner;
    boardSelectionColumnController.boardSelectionController = this;
  }

  public void OnSelectColumn(int column)
  {
    selectedColumn = column;
  }
}
