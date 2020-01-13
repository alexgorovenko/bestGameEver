using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlangView : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;

    void Start()
    {
        Game game = new Game();
        Debug.Log(game.freeCommandors);
    }

    void Update()
    {

    }
}
