using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class snapgrid : MonoBehaviour
{
    [SerializeField] private ConstructionGrid gridManager;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float gridSize = 0.25f; 

    private float leftEdge;
    private float bottomEdge;

    private void Start()
    {
        leftEdge = gridManager.leftEdge;
        bottomEdge = gridManager.bottomEdge;
    }

    void Update()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        float relativeX = mouseWorldPosition.x - leftEdge;
        float relativeY = mouseWorldPosition.y - bottomEdge;

        float snappedX = Mathf.Round(relativeX / gridSize) * gridSize;
        float snappedY = Mathf.Round(relativeY / gridSize) * gridSize;

        float finalX = snappedX + leftEdge;
        float finalY = snappedY + bottomEdge;

        transform.position = new Vector3(finalX, finalY, 0f);
    }
}
