using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float cubeSize;
    // Start is called before the first frame update
    void Start()
    {
        cubeSize = GameHandler.CubeSize;
        Reset();

    }

    private void Reset()
    {
        transform.position = new Vector3(Random.Range(cubeSize * -1, cubeSize), Random.Range(cubeSize + 1, cubeSize + 20), Random.Range(cubeSize * -1, cubeSize));

    }

    private void OnTriggerEnter(Collider other)
    {
        Reset();
    }
}
