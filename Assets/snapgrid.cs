using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class snapgrid : MonoBehaviour
{
    [SerializeField] private ConstructionGrid gridManager;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float gridSize = 0.25f; // Setting 0.25 allows you to change it easily later

    private float leftEdge;
    private float bottomEdge;

    private void Start()
    {
        leftEdge = gridManager.leftEdge;
        bottomEdge = gridManager.bottomEdge;
    }

    void Update()
    {
        // 1. Get the mouse position in world space
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        float relativeX = mouseWorldPosition.x - leftEdge;
        float relativeY = mouseWorldPosition.y - bottomEdge;

        // 2. Calculate the snapped X and Y coordinates
        // (Dividing by 0.25 is mathematically identical to multiplying by 4)
        float snappedX = Mathf.Round(relativeX / gridSize) * gridSize;
        float snappedY = Mathf.Round(relativeY / gridSize) * gridSize;

        float finalX = snappedX + leftEdge;
        float finalY = snappedY + bottomEdge;

        // 3. Apply the snapped coordinates back to a Vector3 (keeping Z at 0)
        transform.position = new Vector3(finalX, finalY, 0f);
    }
}
