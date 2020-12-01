using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  public void SpawnCoin(int column)
  {
    var spawnPosition = transform.position;
    spawnPosition.x += -((columns / 2.0f) - 0.5f) + column;

    if (null == spawnedCoin)
    {
      spawnedCoin = Instantiate(coinPrefab);
      spawnedCoin.transform.parent = transform;
    }

    spawnedCoin.transform.position = spawnPosition;
  }
}
