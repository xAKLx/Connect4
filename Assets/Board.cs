using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum CellStatus
{
  Player1,
  Player2,
  Empty
}

public class Board : MonoBehaviour
{
  [Min(1)]
  public int columns = 4;
  [Min(1)]
  public int rows = 4;
  public GameObject cell;
  public CoinInstantiator coinInstantiator;
  CellStatus[,] gameBoard;
  
  private void printBoard()
  {
    var print = "";
    foreach (var row in Enumerable.Range(0, rows))
    {
      var rowPrint = "";
      foreach (var column in Enumerable.Range(0, columns))
      {
        rowPrint = rowPrint == "" ? gameBoard[column, row].ToString() : $"{rowPrint},{gameBoard[column, row].ToString()}";
      }
      print = print == "" ? rowPrint : $"{rowPrint}\n{print}";

    }

    Debug.Log("---------------------");
    print.Split('\n').ToList().ForEach(x => Debug.Log(x));
  }

  private void Start()
  {
    gameBoard = new CellStatus[columns, rows];
    foreach (var column in Enumerable.Range(0, columns))
    {
      foreach (var row in Enumerable.Range(0, rows))
      {
        gameBoard[column, row] = CellStatus.Empty;
      }
    }
  }

  private void OnValidate()
  {
    if (EditorApplication.isPlayingOrWillChangePlaymode) return;
    foreach (Transform child in this.transform)
    {
      UnityEditor.EditorApplication.delayCall += () =>
      {
        DestroyImmediate(child.gameObject);
      };
    }

    foreach (var rowIndex in Enumerable.Range(0, rows))
    {
      foreach (var columnIndex in Enumerable.Range(0, columns))
      {
        UnityEditor.EditorApplication.delayCall += () =>
        {
          var cellInstance = Instantiate(cell, new Vector3(columnIndex - (columns / 2.0f) + 0.5f, rowIndex - (rows / 2.0f) + 0.5f, 0.0f), Quaternion.identity);
          cellInstance.transform.parent = this.transform;
        };
      }
    }

  }

  public Vector2Int? AddCoin(int column, CellStatus type)
  {
    Debug.Log($"Column {column}");
    foreach (var row in Enumerable.Range(0, rows))
    {
      if (gameBoard[column, row] == CellStatus.Empty && type != CellStatus.Empty)
      {
        gameBoard[column, row] = type;
        CheckIfThereIsAWinner();
        return new Vector2Int(column, row);
      }
    }
    return null;
  }

  public void CheckIfThereIsAWinner() => IterateBoardFromRows((cellStatus, position) =>
  {
    var column = position.x;
    var row = position.y;
    if (
      cellStatus != CellStatus.Empty && (
        CheckWinConditionInDirectionAndMark(column, row, Vector2Int.up) ||
        CheckWinConditionInDirectionAndMark(column, row, Vector2Int.right) ||
        CheckWinConditionInDirectionAndMark(column, row, new Vector2Int(1, 1)) ||
        CheckWinConditionInDirectionAndMark(column, row, new Vector2Int(-1, 1))
      )
    )
    {
      Debug.Log($"WINNER {column}, {row}");
    }
  });

  void IterateBoardFromRows(Action<CellStatus, Vector2Int> callback)
  {
    foreach (var row in Enumerable.Range(0, rows))
    {
      foreach (var column in Enumerable.Range(0, columns))
      {
        callback(gameBoard[column, row], new Vector2Int(column, row));
      }
    }
  }

  public bool CheckWinConditionInDirectionAndMark(int column, int row, Vector2Int direction)
  {
    var originalPosition = new Vector2Int(column, row);
    var finalColumn = column + direction.x * 3;
    var finalRow = row + direction.y * 3;

    var winConditionIsOutOfBounds = finalColumn < 0 || finalColumn > columns - 1 || finalRow < 0 || finalRow > rows - 1;
    if (winConditionIsOutOfBounds) return false;

    var cells = Enumerable.Range(0, 4).ToList().Select(x => originalPosition + direction * x);
    var areAllCellsSelectedForTheSamePlayer = cells.All(x => gameBoard[x.x, x.y] == gameBoard[originalPosition.x, originalPosition.y]);

    if (areAllCellsSelectedForTheSamePlayer) InstantiateWinningCells(cells);
    return areAllCellsSelectedForTheSamePlayer;
  }

  void InstantiateWinningCells(IEnumerable<Vector2Int> cells)
  {
    cells.ToList().ForEach(cell =>
      coinInstantiator.InstantiateWinnerCoin(cell, gameBoard[cell.x, cell.y] == CellStatus.Player1 ? 1 : 2)
    );
  }

}
