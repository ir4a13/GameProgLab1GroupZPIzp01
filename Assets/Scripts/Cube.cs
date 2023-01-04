using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cube : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 14f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private readonly float topBoundary = 4.6f;
    [SerializeField] private readonly float leftBoundary = -8.5f;
    [SerializeField] private readonly float bottomBoundary = -5f;
    [SerializeField] private readonly float rightBoundary = 8.5f;
    [SerializeField] private readonly float groundLevel = -4f;
    [SerializeField] private readonly Vector2 startPosition = new Vector2(-7.15f, -3.7f);
    [SerializeField] private readonly Vector2 endPosition = new Vector2(7.3f, 1.2f);

    //Telephortation
    [SerializeField] private readonly Vector2 teleportationBottomTunnel = new Vector2(8.4f, -3.6f);
    [SerializeField] private readonly Vector2 teleportationTopTunnel = new Vector2(-5.4f, 1.06f);

    // Win and Lose
    [SerializeField] private GameObject winSprite;
    private bool isVictory = false;

    void Start() {
        winSprite.SetActive(false);
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        HandleJumping();
        HandleTelephort();
        CheckBoundnaries();
        HandleFailure();
        HandleWin();
    }

    private void HandleWin()
    {
        if (transform.position.x > endPosition.x && transform.position.y > endPosition.y)
        {
            winSprite.SetActive(true);
            isVictory = true;
        }
        
        if (isVictory && Input.GetButtonDown("Jump"))
        {
            winSprite.SetActive(false);
            transform.position = startPosition;
            isVictory = false;
        }
    }

    private void HandleFailure() {
        if (transform.position.y < groundLevel)
        {
            transform.position = startPosition;
        }
    }

    private void HandleJumping() {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void CheckBoundnaries() 
    {
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary),
            Mathf.Clamp(transform.position.y, bottomBoundary, topBoundary)
        );
    }

    private void HandleTelephort() {
        if (transform.position.x > teleportationBottomTunnel.x && transform.position.y < teleportationBottomTunnel.y)
        {
            transform.position = teleportationTopTunnel;
        }

        if(transform.position.x < teleportationTopTunnel.x && transform.position.y > teleportationTopTunnel.y)
        {
            transform.position = teleportationBottomTunnel;
        }
    }
}