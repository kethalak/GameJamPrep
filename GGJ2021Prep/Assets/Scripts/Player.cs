using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private float moveSpeed;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private bool hitDetect = false;
    private bool isGrounded = false;
    
    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        DoJump();
        DoMove();
        
        isGrounded = CheckIsGrounded();
    }

    private void DoJump()
    {
        var jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);

        if (jump && isGrounded)
        {
            var vel = _rb.velocity;
            vel.y = jumpHeight;
            _rb.velocity = vel;
        }
    }

    private void DoMove()
    {
        var h = Input.GetAxis("Horizontal");
        var vel = _rb.velocity;
        vel.x = h * moveSpeed;
        _rb.velocity = vel;
    }
    
    private bool CheckIsGrounded()
    {
        var origin = new Vector2(transform.position.x, transform.position.y + transform.localScale.y/2);
        var size = new Vector2(transform.localScale.x, transform.localScale.y);
        var angle = 0f;
        var filter = new ContactFilter2D().NoFilter();
        
        var hits = new List<RaycastHit2D>();
        var hitCount = Physics2D.BoxCast(origin, size, angle, Vector2.down, filter, hits, transform.localScale.y/2);

        if (hitCount <= 0) return false;
        
        foreach (var hit in hits)
        {
            if (hit.collider == _boxCollider) continue;
            
            if(hit.point.y < transform.position.y)
                return true;
        }

        return false;
    }
}
