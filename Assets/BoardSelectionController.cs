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
  int selectedColumn;


  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      var coinPosition = board.AddCoin(selectedColumn);

      if (coinPosition != null)
      {
        Debug.Log(coinPosition);
        var coinInstance = Instantiate(coinPrefab);
        coinInstance.transform.position = new Vector3(
          coinPosition.Value.Item1 - (columns / 2.0f) + 0.5f,
          coinPosition.Value.Item2 - (board.rows / 2.0f) + 0.5f,
          0
        );
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

    foreach (var columnIndex in Enumerable.Range(0, columns))
    {
      UnityEditor.EditorApplication.delayCall += () =>
      {
        var boardSelectionColumn = Instantiate(boardSelectionColumnPrefab, new Vector3(columnIndex - (columns / 2.0f) + 0.5f, 0.0f, 0.0f), Quaternion.identity);
        boardSelectionColumn.transform.parent = transform;
        boardSelectionColumn.transform.localScale = new Vector3(1, 1, 1);
        var boardSelectionColumnController = boardSelectionColumn.GetComponent<BoardSelectionColumnController>();
        boardSelectionColumnController.column = columnIndex;
        boardSelectionColumnController.coinSpawner = coinSpawner;
        boardSelectionColumnController.boardSelectionController = this;
      };
    }

  }

  public void OnSelectColumn(int column)
  {
    selectedColumn = column;
  }
}
