# UnityLeakTracker

[Unity's addressable asset system](https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/index.html) uses ref counting to track when to unload asssets, however they dont expose the ref count on the given asset. These extension methods on `AssetReference<T>` are a thin layer atop the addressables system that tracks ref counters and also keeps a pointer to the object that requested the load.

API usage is non intrusive on existing project structures

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

# in project usage
## [Aceplay](https://apps.apple.com/us/app/ace-play/id1481622112)
![](https://image.winudf.com/v2/image1/Y29tLmp1bXBidXR0b25zdHVkaW8uYWNlcGxheV9pY29uXzE1ODE3Mzc0MzFfMDcw/icon.png?w=&fakeurl=1)

