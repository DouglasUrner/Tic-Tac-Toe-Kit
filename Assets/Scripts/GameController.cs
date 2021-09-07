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

    // Playing mode:
    public bool misere = false;     // Invert win, k in a row loses.

    public int gridOriginX = 0;
    public int gridOriginY = 0;
    public Transform board;

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
        Restart();
    }

    void LayoutGrid(int rows, int cols)
    {
        // Center grid in game window.
        gridOriginX = Screen.width / 2 - (m / 2 * spaceW);
        gridOriginY = Screen.height / 2 - (n / 2 * spaceH);

        // Size the gameOverPanel.

        grid = new Space[rows][];
        for (int r = 0; r < rows; r++)
        {
            grid[r] = new Space[cols];
            for (int c = 0; c < cols; c++)
            {
                var loc = new Vector3(gridOriginX + spaceW * c, gridOriginY + spaceH * r, 0);
                grid[r][c] = Instantiate(spacePrefab, loc, Quaternion.identity, board).GetComponent<Space>();
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
                        break;
                    }
                    Debug.Log("[" + row + "][" + col + "]: " + grid[row][col].buttonText.text + " win: " + win);
                }
                if (win == true)
                {
                    GameOver(side); // XXX - can result in false draw, hence below.
                    return;         // XXX - we should return a result rather than calling GameOver().
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
                    return; // XXX
                }
            }
        }

        /*
        ** Check diagonals.
        **
        ** When k < m on n, we can start a check at any row or column where
        ** row < m - k and col < n - k
        */
        for (int baseR = 0; baseR <= m - k; baseR++)
        {
            for (int baseC = n - 1; baseC >= k - 1; baseC--)
            {
                bool win = true;
                for (int step = 0; step < k; step++)
                {
                    int row = baseR + step;
                    int col = baseC - step;
                    if (grid[row][col].buttonText.text != side)
                    {
                        win = false;
                    }
                }
                if (win == true)
                {
                    GameOver(side);
                    return; // XXX
                }
            }
        }
        
        for (int baseR = 0; baseR <= m - k; baseR++)
        {
            for (int baseC = 0; baseC <= n - k; baseC++)
            {
                bool win = true;
                for (int step = 0; step < k; step++)
                {
                    int row = baseR + step;
                    int col = baseC + step;
                    if (grid[row][col].buttonText.text != side)
                    {
                        win = false;
                    }
                }
                if (win == true)
                {
                    GameOver(side);
                    return; // XXX
                }
            }
        }

        // Check if there is still room to play.
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
        }
        else
        {
            if (misere)
            {
                message = winner + " Loses";
            }
            else
            {
                message = winner + " Wins";
            }
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
