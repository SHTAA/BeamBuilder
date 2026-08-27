using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BeamBuild : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject objectsToSpawn;
    [SerializeField] private GameObject pivotPrefab;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private snapgrid snapTarget;

    [SerializeField] private float pivotSearchRadius = 0.05f;

    private bool hasSaved;

    private Vector3 startMousePosition;
    private Vector3 currentMousePosition;
    private void Update()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;

        if (Input.GetMouseButton(0))
        {
            if (!hasSaved)
            {
                startMousePosition = snapTarget.transform.position;
                hasSaved = true;
            }
            currentMousePosition = snapTarget.transform.position;
        }

        if (Input.GetMouseButtonUp(0) && hasSaved)
        {
            BuildBeam(startMousePosition, currentMousePosition);
            hasSaved = false;
        }
    }
    private void BuildBeam(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = endPosition - startPosition;
        float distance = direction.magnitude;

        if (distance <= 0.001f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Pivot startPivot = GetOrCreatePivot(startPosition);
        Pivot endPivot = GetOrCreatePivot(endPosition);

        GameObject beam = Instantiate(objectsToSpawn, (startPosition + endPosition) / 2f, Quaternion.Euler(0f, 0f, angle));

        Vector3 scale = beam.transform.localScale;
        scale.x = distance;
        beam.transform.localScale = scale;

        HingeJoint2D[] hinges = beam.GetComponents<HingeJoint2D>();

        if (hinges.Length < 2) 
        {
            hinges = new HingeJoint2D[2];
            if (beam.GetComponents<HingeJoint2D>().Length == 0) hinges[0] = beam.AddComponent<HingeJoint2D>();
            else hinges[0] = beam.GetComponents<HingeJoint2D>()[0];
            hinges[1] = beam.AddComponent<HingeJoint2D>();

        } 
        else 
        {
            hinges[0] = hinges[0];
            hinges[1] = hinges[1];

        }

        ConnectHinge(hinges[0], startPivot, startPosition, beam.transform);
        ConnectHinge(hinges[1], endPivot, endPosition, beam.transform);

        StructuralIntegrity structuralIntegrity = beam.GetComponent<StructuralIntegrity>();
        structuralIntegrity.CreateStructurePoints(distance);
    }
    private void ConnectHinge(HingeJoint2D hinge, Pivot pivot, Vector3 worldPosition, Transform beamTransform)
    {
        hinge.connectedBody = pivot.Body;
        hinge.autoConfigureConnectedAnchor = false;
        hinge.connectedAnchor = Vector2.zero;

        Vector3 localPosition = beamTransform.InverseTransformPoint(worldPosition);
        hinge.anchor = localPosition;
    }
    private Pivot GetOrCreatePivot(Vector3 position)
    {
        Pivot[] pivots = FindObjectsOfType<Pivot>();

        foreach (Pivot pivot in pivots) 
        {

            if (Vector2.Distance(pivot.transform.position, position) <= pivotSearchRadius) 
            {
                return pivot;

            }
        }

        GameObject newPivot = Instantiate(pivotPrefab, position, Quaternion.identity);
        return newPivot.GetComponent<Pivot>();
    }
}