using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using LeakTracker;

public class LeakTester : MonoBehaviour
{
    [SerializeField]
    AssetReferenceT<UnityEngine.GameObject> asset;
    AsyncOperationHandle handle;
    int index = 0;
    List<List<GameObject>> instances;
    public UnityEvent v;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("doo stuff");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {

        }
    }
}