using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuralIntegrity : MonoBehaviour
{
    [SerializeField] private GameObject structurePointPrefab;
    [SerializeField] private float pointSpacing = 0.25f;

    public void CreateStructurePoints(float beamLength)
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        float originalWidth = spriteRenderer.sprite.bounds.size.x;
        float scaleMultiplier = beamLength / originalWidth;

        int numberOfPoints = Mathf.FloorToInt(beamLength / pointSpacing) + 1;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float distanceAlongBeam = i * pointSpacing;
            float normalizedPosition = distanceAlongBeam / beamLength;

            float localX = Mathf.Lerp(-originalWidth / 2f, originalWidth / 2f, normalizedPosition);

            Vector3 localPosition = new Vector3(localX, 0f, 0f);

            GameObject point = Instantiate(structurePointPrefab, transform);
            point.transform.localPosition = localPosition;

            SpriteRenderer pointRenderer = point.GetComponent<SpriteRenderer>();
            pointRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;

            Vector3 pointScale = point.transform.localScale;
            pointScale.x /= scaleMultiplier;
            pointScale.y /= transform.localScale.y;
            point.transform.localScale = pointScale;
        }
    }
}