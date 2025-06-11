using UnityEngine;

public class custom : MonoBehaviour
{
    string index = "";

    int integer1 = 16;
    int integer2 = 31;
    int counter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug.Log("Are you sure" + demoString);
        // Debug.Log(isAlive);
        for (int i = 1; i <= 10; i++)
        {
            index += i.ToString() + " ";
        }
        Debug.Log(index);

        while (integer1 < integer2)
        {
            integer1 += 1;
            counter += 1;
        }
        Debug.Log(counter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
