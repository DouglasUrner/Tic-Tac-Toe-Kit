using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Board dimensions:
    public int m = 3; // Rows
    public int n = 3; // Columns
    // Winning run:
    public int k = 3;

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

    public string GetSide()
    {
        return side;
    }

    void ChangeSide()
    {
        side = (side == "X") ? "O" : "X";
    }

    public void EndTurn()
    {
        moves++;

        // Check rows.
        for (int row = 0; row < m; row++)
        {
            bool win = true;
            for (int col = 0; col < n; col++)
            {
                if (spaceList[row * m + col].text != side)
                {
                    win = false;
                    break;
                }
            }
            if (win == true)
            {
                GameOver();
            }
        }


        // Check columns.
        for (int col = 0; col < n; col++)
        {
            bool win = true;
            for (int row = 0; row < m; row++)
            {
                if (spaceList[row * m + col].text != side)
                {
                    win = false;
                    break;
                }
            }
            if (win == true)
            {
                GameOver();
            }
        }

        // Check diagonals.
        // TODO: handle boards where m != n and where k < m || k < n.
        {
            bool win = true;
            for (int i = 0; i < m; i++)
            {
                if (spaceList[i * m + i].text != side)
                {
                    win = false;
                    break;
                }
            }
            if (win == true)
            {
                GameOver();
            }

            win = true;
            for (int i = 0; i < m; i++)
            {
                if (spaceList[(m * i) + (m - 1 - i)].text != side)
                {
                    win = false;
                    break;
                }
            }
            if (win == true)
            {
                GameOver();
            }
        }

        if (moves >= m * n)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "Cat's Game";
            restartButton.SetActive(true);
        }
        ChangeSide();
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = side + " wins!";
        restartButton.SetActive(true);
        for (int i = 0; i < spaceList.Length; i++)
            SetInteractable(false);
    }

    void SetInteractable(bool setting)
    {
        for (int i = 0; i < spaceList.Length; i++)
            spaceList[i].GetComponentInParent<Button>().interactable = setting;
    }

    public void Restart()
    {
        side = "X";
        moves = 0;
        gameOverPanel.SetActive(false);
        SetInteractable(true);
        restartButton.SetActive(false);
        for (int i = 0; i < spaceList.Length; i++)
            spaceList[i].text = "";
    }
}
