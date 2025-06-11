using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool DoorOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        if (DoorOpen == false)
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y += 90f; // Rotate the door by 90 degrees
            transform.eulerAngles = doorRotation; // Apply the new rotation to the door
            DoorOpen = true; // Set the door state to open
        }
        else if (DoorOpen == true)
        {
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y -= 90f; // Rotate the door back by 90 degrees
            transform.eulerAngles = doorRotation; // Apply the new rotation to the door
            DoorOpen = false; // Set the door state to closed
        }
        
    }


}
