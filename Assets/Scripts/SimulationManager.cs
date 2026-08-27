using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private bool simulationStarted;
    [SerializeField] private ConstructionGrid grid;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSimulation();
            grid.gridParent.SetActive(false);
        }
    }

    private void StartSimulation()
    {
        simulationStarted = true;

        int buildingLayer = LayerMask.NameToLayer("Building");

        Rigidbody2D[] rigidbodies = FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D rb in rigidbodies)
        {
            if (rb.gameObject.layer == buildingLayer)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        Pivot[] pivots = FindObjectsOfType<Pivot>();

        foreach (Pivot pivot in pivots)
        {
            if (pivot.gameObject.layer == buildingLayer)
            {
                SpriteRenderer sprite = pivot.GetComponent<SpriteRenderer>();

                if (sprite != null)
                {
                    sprite.enabled = false;
                }
            }
        }
    }
}