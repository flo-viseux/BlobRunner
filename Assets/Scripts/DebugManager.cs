using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private static DebugManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void CreateDebugSphere(Vector3 position, float duration = 2f, float radius = 2f)
    {
        if (instance == null)
        {
            Debug.LogWarning("DebugManager instance not found. Please make sure there is a DebugManager in the scene.");
            return;
        }

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(radius, radius, radius);

        Destroy(sphere, duration);
    }
}
