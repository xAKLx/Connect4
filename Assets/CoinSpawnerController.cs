using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Spawns coins on top of the border to give a visual feedback of where the coin will be inserted
public class CoinSpawnerController : MonoBehaviour
{
  [Min(1)]
  public int columns = 7;
  public GameObject coinPrefab;
  GameObject spawnedCoin;

  private void OnDrawGizmos()
  {
    var start = transform.position;
    start.x -= (columns / 2.0f) - 0.5f;
    var end = transform.position;
    end.x += (columns / 2.0f) - 0.5f;
    Gizmos.color = Color.green;
    Gizmos.DrawLine(start, end);
  }

  public void SpawnCoin(int column, Color color)
  {
    if (Board.inWinScreen) return;
    var spawnPosition = transform.position;
    spawnPosition.x += -((columns / 2.0f) - 0.5f) + column;

    if (null == spawnedCoin)
    {
      spawnedCoin = Instantiate(coinPrefab);
      spawnedCoin.transform.parent = transform;
    }

    spawnedCoin.transform.position = spawnPosition;
    spawnedCoin.GetComponent<SpriteRenderer>().color = color;
  }

  public void RemoveCoin()
  {
    if (null == spawnedCoin) return;

    Destroy(spawnedCoin);
    spawnedCoin = null;

  }
}
