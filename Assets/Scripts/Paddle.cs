﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Ball Ball;
    private float mouseXPos;
    private AudioManager audioManager;
    // Variable for automated play testing
    private float ballPos;
    [SerializeField]
    private bool autoPlay;
    private float z_distance, leftCorner, rightCorner;
    // Start is called before the first frame update
    void Start()
    {
        Ball = FindObjectOfType<Ball>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        // Restrict paddle position
        z_distance = transform.position.z - Camera.main.transform.position.z;
        // Add 1 to left and substract 1 from right to prevent paddle from overflowing at the left & right corners
        leftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z_distance)).x + 1f;
        rightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, z_distance)).x - 1f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only play sound of collision between ball and paddle when the ball has already been served
        if (Ball.IsServed())
            audioManager.PlayUnbreakableHitAudio();
    }
    // Update is called once per frame
    private void MoveWithMouse()
    {
        // Main gameplay
        // Move paddle with mouse
        mouseXPos = Input.mousePosition.x / Screen.width * (rightCorner - leftCorner) + leftCorner;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(mouseXPos, leftCorner, rightCorner);
        gameObject.transform.position = paddlePos;
    }
    private void AutomatedPlay()
    {
        // Automated play testing

        ballPos = Ball.transform.position.x;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(ballPos, leftCorner, rightCorner);
        gameObject.transform.position = paddlePos;
    }
    void Update()
    {
       
        if (autoPlay)
            AutomatedPlay();
        else
            MoveWithMouse();
    }
}
