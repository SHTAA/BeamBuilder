using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePoint : MonoBehaviour
{
    [SerializeField] private float maxLoad = 100f;
    [SerializeField] private float neighbourRadius = 0.25f;
    [SerializeField] private float minimumLoadToTransfer = 0.1f;

    [SerializeField] private float currentLoad;
    [SerializeField] private bool canReceiveTestLoad;

    private StructurePoint[] neighbours;

    private void Update()
    {
        if (canReceiveTestLoad && Input.GetKeyDown(KeyCode.Space))
        {
            LoadEvent loadEvent = new LoadEvent();
            AddLoad(50f, loadEvent, null);
        }
    }

    public void AddLoad(float load, LoadEvent loadEvent, StructurePoint sender)
    {
        if (load < minimumLoadToTransfer)
        {
            return;
        }

        bool alreadyProcessed = loadEvent.ProcessedPoints.Contains(this);

        currentLoad += load;

        Debug.Log(gameObject.name + " received " + load + " load. Current Load: " + currentLoad);

        if (currentLoad >= maxLoad)
        {
            Break();
            return;
        }

        if (alreadyProcessed)
        {
            return;
        }

        loadEvent.ProcessedPoints.Add(this);

        FindNeighbours();
        DistributeLoad(load, loadEvent, sender);
    }

    private void FindNeighbours()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, neighbourRadius);

        List<StructurePoint> foundNeighbours = new List<StructurePoint>();

        foreach (Collider2D nearbyObject in nearbyObjects)
        {
            StructurePoint point = nearbyObject.GetComponent<StructurePoint>();

            if (point != null && point != this)
            {
                float distance = Vector2.Distance(transform.position, point.transform.position);

                if (distance <= neighbourRadius)
                {
                    foundNeighbours.Add(point);
                }
            }
        }

        neighbours = foundNeighbours.ToArray();
    }

    private void DistributeLoad(float receivedLoad, LoadEvent loadEvent, StructurePoint sender)
    {
        if (neighbours == null || neighbours.Length == 0)
        {
            return;
        }

        List<StructurePoint> availableNeighbours = new List<StructurePoint>();

        foreach (StructurePoint neighbour in neighbours)
        {
            if (neighbour != null && neighbour != sender)
            {
                availableNeighbours.Add(neighbour);
            }
        }

        if (availableNeighbours.Count == 0)
        {
            return;
        }

        float loadPerNeighbour = receivedLoad / availableNeighbours.Count;

        foreach (StructurePoint neighbour in availableNeighbours)
        {
            neighbour.AddLoad(loadPerNeighbour, loadEvent, this);
        }
    }

    private void Break()
    {
        Debug.Log(gameObject.name + " Structure Point Broken");
        Destroy(transform.parent.gameObject);
    }

    public class LoadEvent
    {
        public HashSet<StructurePoint> ProcessedPoints = new HashSet<StructurePoint>();
    }
}


