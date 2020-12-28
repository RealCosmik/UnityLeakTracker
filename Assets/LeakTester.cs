using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using LeakTracker;

public class LeakTester : MonoBehaviour
{
	[SerializeField] AssetReferenceT<UnityEngine.GameObject>[] assets;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.L))
	    {
	    	foreach(var a in assets)
	    		a.LoadAssetAsync(this);
        }

	    if (Input.GetKeyDown(KeyCode.R))
        {
		    foreach(var a in assets)
		    	a.ReleaseAsset(this);
        }
    }
}