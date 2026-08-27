using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCube : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool dragging;
    private Vector2 mouseOffset;
    private Vector2 previousMousePosition;
    private Vector2 mouseVelocity;

    [SerializeField] private float flingMultiplier = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);

            if (hit != null && hit.gameObject == gameObject)
            {
                dragging = true;

                mouseOffset = (Vector2)transform.position - mousePosition;
                previousMousePosition = mousePosition;

                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        if (dragging && Input.GetMouseButton(1))
        {
            Vector2 targetPosition = mousePosition + mouseOffset;

            mouseVelocity = (mousePosition - previousMousePosition) / Time.deltaTime;
            previousMousePosition = mousePosition;

            rb.MovePosition(targetPosition);
        }

        if (dragging && Input.GetMouseButtonUp(1))
        {
            dragging = false;

            rb.velocity = mouseVelocity * flingMultiplier;
        }
    }
}