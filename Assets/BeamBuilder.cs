using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBuild : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Camera mainCamera;

    public GameObject objectsToSpawn;

    public Transform spawnPoint;

    private bool hasSaved = false;

    public Vector3 startMousePosition;
    public Vector3 currentMousePosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
        if (Input.GetMouseButton(0))
        {
            if (!hasSaved)
            {
                startMousePosition = mouseWorldPosition;
                hasSaved = true;
            }


            currentMousePosition = mouseWorldPosition;
            Debug.Log(startMousePosition);
            Debug.Log(currentMousePosition);

        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 direction = currentMousePosition - startMousePosition;
            float distance = direction.magnitude;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject spawnedobj = Instantiate(objectsToSpawn, spawnPoint.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D rb = spawnedobj.GetComponent<Rigidbody2D>();

            Vector3 currentScale = spawnedobj.transform.localScale;
            Vector3 positionObj = spawnedobj.transform.localPosition;

            // 3. Change only the X component
            currentScale.x = Vector3.Distance(startMousePosition, currentMousePosition); // Set your desired X scale here
            positionObj = (startMousePosition + currentMousePosition) / 2;
            // 4. Assign the modified vector back to the object
            spawnedobj.transform.localScale = currentScale;
            spawnedobj.transform.localPosition = positionObj;

            if (rb != null)
            {
                // Example: Push the object forward upon spawning
                rb.AddForce(spawnPoint.forward * 500f);
            }
            hasSaved = false;
        }

    }
}