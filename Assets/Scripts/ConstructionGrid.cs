using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ConstructionGrid : MonoBehaviour
{

    public float gridSpacing = 0.25f;
    public float gridHeight;
    public float gridWidth;

    public float leftEdge;
    public float bottomEdge;

    void Awake()
    {
        Camera cam = Camera.main;

        float totalHeight = 2f * cam.orthographicSize;
        float totalWidth = totalHeight * cam.aspect;

        gridHeight = totalHeight / gridSpacing;
        gridWidth = totalWidth / gridSpacing;

        leftEdge = cam.transform.position.x - (totalWidth / 2);
        float rightEdge = cam.transform.position.x + (totalWidth / 2);
        bottomEdge = cam.transform.position.y - (totalHeight / 2);
        float topEdge = cam.transform.position.y + (totalHeight / 2);

        for (int y = 0; y < gridHeight; y++)
        {
            float currentY = bottomEdge + y * gridSpacing;

            if (y % 4 == 0)
            {
                makeLine(new Vector3(leftEdge, currentY, 0f), new Vector3(rightEdge, currentY, 0f), 0.11f);
            }

            if (y % 20 == 0)
            {
                makeLine(new Vector3(leftEdge, currentY, 0f), new Vector3(rightEdge, currentY, 0f), 0.13f);
            }

            makeLine(new Vector3(leftEdge, currentY, 0f), new Vector3(rightEdge, currentY, 0f), 0.1f);

            Debug.Log(y);

        
        }

        for (int x = 0; x < gridWidth; x++)
        {
            float currentX = leftEdge + x * gridSpacing;

            if (x % 4 == 0)
            {
                makeLine(new Vector3(currentX, bottomEdge, 0f), new Vector3(currentX, topEdge, 0f), 0.11f);
            }

            if (x % 20 == 0)
            {
                makeLine(new Vector3(currentX, bottomEdge, 0f), new Vector3(currentX, topEdge, 0f), 0.13f);
            }


            makeLine(new Vector3(currentX, bottomEdge, 0f), new Vector3(currentX, topEdge, 0f), 0.1f);

            //Debug.Log(x);
        }


    }

    public void makeLine(Vector3 startPos, Vector3 endPos, float opacity)
    {
        GameObject lineObject = new GameObject("LineHolder", typeof(LineRenderer));

        LineRenderer lineRenderer1 = lineObject.GetComponent<LineRenderer>();

        // Set the material
        lineRenderer1.material = new Material(Shader.Find("Sprites/Default"));

        Color lineColor = new Color(0.0f, 0.0f, 0.0f, opacity);

        // Set the color
        lineRenderer1.startColor = lineColor;
        lineRenderer1.endColor = lineColor;

        // Set the width
        lineRenderer1.startWidth = 0.04f;
        lineRenderer1.endWidth = 0.04f;

        // Set the number of vertices
        lineRenderer1.positionCount = 2;

        // Set the positions of the vertices
        lineRenderer1.SetPosition(0, startPos);
        lineRenderer1.SetPosition(1, endPos);
    }
}

