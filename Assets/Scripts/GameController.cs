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

    public int gridOriginX = 0;
    public int gridOriginY = 0;
    public Transform gridCanvas;

    public Space[][] grid;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject spacePrefab;
    public int spaceW = 75; // Width of Space plus padding.
    public int spaceH = 75; // Height of Space plus padding.

    private string side;
    private int moves;
    
    // Start is called before the first frame update
    void Start()
    {
        LayoutGrid(m, n);
  
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);

        side = "X";
        moves = 0;
    }

    void LayoutGrid(int rows, int cols)
    {
        grid = new Space[rows][];
        for (int r = 0; r < rows; r++)
        {
            grid[r] = new Space[cols];
            for (int c = 0; c < cols; c++)
            {
                var loc = new Vector3(gridOriginX + spaceW * c, gridOriginY + spaceH * r, 0);
                grid[r][c] = Instantiate(spacePrefab, loc, Quaternion.identity, gridCanvas).GetComponent<Space>();
                grid[r][c].name = "Space(" + r + ", " + c + ")";
                grid[r][c].SetControllerReference(this);
                //grid[r][c].buttonText.text = "" + r + c;
            }
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
            for (int start = 0; start <= (n - k); start++)
            {
                bool win = true;
                for (int col = start; col < (start + k); col++)
                {
                    if (grid[row][col].buttonText.text != side)
                    {
                        win = false;
                    }
                }
                if (win == true)
                {
                    GameOver(side);
                }
            }
        }


        // Check columns.
        for (int col = 0; col < n; col++)
        {
            for (int start = 0; start <= (m - k); start++)
            {
                bool win = true;
                for (int row = start; row < start + k; row++)
                {
                    if (grid[row][col].buttonText.text != side)
                    {
                        win = false;
                        break;
                    }
                }
                if (win == true)
                {
                    GameOver(side);
                }
            }
        }

        // Check diagonals.
        // XXX: assumes that m == n.
        // TODO: handle boards where m != n and where k < m || k < n.
        {
            bool win = true;
            for (int i = 0; i < m; i++)
            {
                if (grid[i][i].buttonText.text != side)
                {
                    win = false;
                    break;
                }
            }
            if (win == true)
            {
                GameOver(side);
            }

            win = true;
            for (int i = 0; i < m; i++)
            {
                if (grid[i][(n-1) - i].buttonText.text != side)
                {
                    win = false;
                    break;
                }
            }
            if (win == true)
            {
                GameOver(side);
            }
        }

        if (moves >= m * n)
        {
            GameOver("draw");
        } else {
            ChangeSide();
        }
    }

    void GameOver(string winner)
    {
        string message;
        
        if (winner == "draw")
        {
            message = "Cat's Game";
        } else {
            message = winner + " Wins";
        }

        gameOverPanel.SetActive(true);
        gameOverText.text = message;
        restartButton.SetActive(true);
        SetInteractable(false);
    }

    void SetInteractable(bool toggle)
    {
        for (int row = 0; row < m; row++)
        {
            for (int col = 0; col < n; col++)
            {
                grid[row][col].button.interactable = toggle;
            }
        }
    }

    public void Restart()
    {
        side = "X";
        moves = 0;
        gameOverPanel.SetActive(false);
        SetInteractable(true);
        restartButton.SetActive(false);
        for (int r = 0; r < m; r++)
        {
            for (int c = 0; c < n; c++)
            {
                grid[r][c].buttonText.text = "";
            }
        }
    }
}
