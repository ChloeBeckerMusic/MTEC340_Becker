using UnityEngine;

public class ControlFlow : MonoBehaviour
{
    public bool flag = false;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (flag is true)
        {
            Debug.Log("Boolean flag is set");
        }
        else
        {
            Debug.Log("Boolean flag isn't set");
        }
        
        for (int x = 0; x <= 9; x++)
        {
           int y = (int)Mathf.Pow(2, x);
            Debug.Log($"The {x} power of 2 is {y}");
        }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
