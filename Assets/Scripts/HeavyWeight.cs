using System.Collections.Generic;
using UnityEngine;

public class HeavyWeight : MonoBehaviour
{
    [SerializeField] private float load = 100f;
    [SerializeField] private float pointDetectionRadius = 0.12f;

    [SerializeField] private int maximumContactPoints = 3;
    [SerializeField] private float singlePointLoadMultiplier = 0.5f;

    private HashSet<GameObject> contactedBeams = new HashSet<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StructuralIntegrity structuralIntegrity = collision.gameObject.GetComponent<StructuralIntegrity>();

        if (structuralIntegrity == null)
        {
            return;
        }

        if (contactedBeams.Contains(collision.gameObject))
        {
            return;
        }

        contactedBeams.Add(collision.gameObject);

        ApplyLoadToBeam(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StructuralIntegrity structuralIntegrity = collision.gameObject.GetComponent<StructuralIntegrity>();

        if (structuralIntegrity != null)
        {
            contactedBeams.Remove(collision.gameObject);
        }
    }

    private void ApplyLoadToBeam(GameObject beam)
    {
        StructurePoint[] points = beam.GetComponentsInChildren<StructurePoint>();

        if (points.Length == 0)
        {
            Debug.Log("Beam has no Structure Points.");
            return;
        }

        Collider2D weightCollider = GetComponent<Collider2D>();

        if (weightCollider == null)
        {
            Debug.Log("Heavy Weight has no Collider2D.");
            return;
        }

        Vector2 weightPosition = weightCollider.bounds.center;

        List<StructurePoint> hitPoints = new List<StructurePoint>();

        foreach (StructurePoint point in points)
        {
            float distance = Vector2.Distance(weightPosition, point.transform.position);

            if (distance <= pointDetectionRadius)
            {
                hitPoints.Add(point);
            }
        }

        Debug.Log("Heavy Weight found " + hitPoints.Count + " Structure Points on " + beam.name);

        if (hitPoints.Count == 0)
        {
            return;
        }

        int contactPointCount = Mathf.Min(hitPoints.Count, maximumContactPoints);

        float loadToApply = load;

        if (hitPoints.Count == 1)
        {
            loadToApply *= singlePointLoadMultiplier;
        }

        float loadPerPoint = load / hitPoints.Count;

        foreach (StructurePoint point in hitPoints)
        {
            StructurePoint.LoadEvent loadEvent = new StructurePoint.LoadEvent();

            point.AddLoad(loadPerPoint, loadEvent, null);
        }
    }
}