using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;

    public void SetControllerReference(GameController controller)
    {
        gameController = controller;
    }

}
