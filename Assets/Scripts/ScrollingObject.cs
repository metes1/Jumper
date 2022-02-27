using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameController.instance.gameOver == true || GameController.instance.titleScreen == true)
        {
            // Stop the scrolling when the game is over or we are on the title screen
            rb.velocity = Vector2.zero;
        } else
        {
            rb.velocity = new Vector2(GameController.instance.scrollSpeed, 0);
        }
    }
}
