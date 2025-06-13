using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool DoorOpen = false;
    [SerializeField]
    string requiredKeyName;
    [SerializeField]
    float autoCloseDistance = 5f; // Distance at which the door will automatically close

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        if (requiredKeyName != null && requiredKeyName != "")
        {
            // Check if the player has the required key
            PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
            if (player != null && player.Keys.Contains(requiredKeyName))
            {
                ToggleDoor();
            }
            else
            {
                player.Popup("You need the key: " + requiredKeyName + " to open this door.");
            }
        }
        else
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
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

    void Update()
    {
        if (DoorOpen)
        {
            PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance > autoCloseDistance)
                {
                    ToggleDoor();
                }  
            }
            
        }
    }

}
