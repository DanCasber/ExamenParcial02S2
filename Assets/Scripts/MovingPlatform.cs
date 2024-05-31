using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 1f;

    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = endPoint.position;
    }

    private void Update()
    {
        platform.position = Vector3.Lerp(platform.position, targetPosition, speed * Time.deltaTime);

        // Move the platform towards the target position
        //platform.position = Vector3.MoveTowards(platform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the target position
        if (Vector3.Distance(platform.position, targetPosition) < 0.1f)
        {
            // Swap the target position
            targetPosition = targetPosition == endPoint.position ? startPoint.position : endPoint.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a line in the editor to visualize the movement path
        // Only draw when the game is not running
        if (!Application.isPlaying)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.transform.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger Exit Player");
            other.transform.parent = null;
        }
    }
}
