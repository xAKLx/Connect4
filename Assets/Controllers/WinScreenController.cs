using UnityEngine;

public class WinScreenController : MonoBehaviour
{
  public GameObject scrim;
  public GameObject redPlayerWinsScreen;
  public GameObject yellowPlayerWinsScreen;


  public void ShowWinner(int winner)
  {
    scrim.SetActive(true);

    if (winner == 1)
    {
      redPlayerWinsScreen.SetActive(true);
    }
    else
    {
      yellowPlayerWinsScreen.SetActive(true);
    }
  }

}
