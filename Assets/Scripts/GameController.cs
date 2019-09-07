using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] spaceList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;

    private string side;
    private int moves;
    
    // Start is called before the first frame update
    void Start()
    {
        SetGameControllerReferenceForButtons();
  
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);

        side = "X";
        moves = 0;
    }

    void SetGameControllerReferenceForButtons()
    {
        for (int i = 0; i < spaceList.Length; i++)
        {
            spaceList[i].GetComponentInParent<Space>().SetControllerReference(this);
        }
    }
}
