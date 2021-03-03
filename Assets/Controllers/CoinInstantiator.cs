using UnityEngine;

public class CoinInstantiator : MonoBehaviour
{
  public GameObject coinPrefab;
  public Board board;
  public Color player1Color = Color.red;
  public Color player1WinColor = Color.red;
  public Color player2Color = Color.yellow;
  public Color player2WinColor = Color.yellow;

  public GameObject InstantiatePlayCoin(Vector2Int positionInBoard, int currentPlayer)
    => InstantiateCoin(positionInBoard, currentPlayer == 1 ? player1Color : player2Color);

  public GameObject InstantiateWinnerCoin(Vector2Int positionInBoard, int currentPlayer)
  {
    var coinInstance = InstantiateCoin(positionInBoard, currentPlayer == 1 ? player1WinColor : player2WinColor);
    coinInstance.GetComponent<SpriteRenderer>().sortingOrder = 2;
    return coinInstance;
  }

  public GameObject InstantiateCoin(Vector2Int positionInBoard, Color color)
  {
    var coinInstance = Instantiate(coinPrefab);

    coinInstance.transform.position = CalculateCoinPosition(positionInBoard);
    coinInstance.GetComponent<SpriteRenderer>().color = color;
    return coinInstance;
  }

  private Vector3 CalculateCoinPosition(Vector2Int positionInBoard) => new Vector3(
    positionInBoard.x - (board.columns / 2.0f) + 0.5f,
    positionInBoard.y - (board.rows / 2.0f) + 0.5f,
    0
  );
}