using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Board : MonoBehaviour
{
  [Min(1)]
  public int columns = 4;
  [Min(1)]
  public int rows = 4;
  public GameObject cell;
  bool[,] gameBoard;

  private void Update()
  {
return;
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
    gameBoard = new bool[columns, rows];
    foreach (var column in Enumerable.Range(0, columns))
    {
      foreach (var row in Enumerable.Range(0, rows))
      {
        gameBoard[column, row] = false;
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

  public Nullable<(int, int)> AddCoin(int column)
  {
    Debug.Log($"Column {column}");
    foreach (var row in Enumerable.Range(0, rows))
    {
      if (!gameBoard[column, row])
      {
        gameBoard[column, row] = true;
        return (column, row);
      }
    }
    return null;
  }
}
