# UnityLeakTracker
Extension methods to track memory leaks when using unity Addressable Asset System

[Unity's addressable asset system](https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/index.html) uses ref counting to track when to unload asssets. Only annoying thing is that they dont expose the ref count of the given asset. This set of extension methods is a thin layer atop the addressables that tracks a ref count of assets & and also keeps a pointer to the object that requested the load.

usage is simple:

```cs
public class AMonobehavior
{
    AssetReference<UnityEngine.Object> someAsset;
    public void Start()
    {
      someAsset.LoadAssetAsync(this)
    }
}
```

Access to leak tracker window can be found inside the tools Menu
![LeakTrackerInfo](https://user-images.githubusercontent.com/26536123/150235470-84d2c7a1-386f-4b3b-9ba6-3cfefdd5ebbb.png)

easy to tell that each asset in the image wont be released until both of those gameobject release their handles.

Optional Gc.Collect button because when debugging assets I sometime manually call the GC to see if/why a pointer is dangling or not
