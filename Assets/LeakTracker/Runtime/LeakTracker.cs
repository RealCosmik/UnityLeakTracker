using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement;

namespace LeakTracker
{
    public static partial class LeakTracker
    {
        public static Dictionary<string, Dictionary<int, int>> Loadtable { get; private set; }

        private static
        bool debugLeaks;
        /// <summary>
        /// used for editor window to know when a repaint is required
        /// </summary>
	    public
        static bool update;
        static LeakTracker()
        {
            Loadtable = new Dictionary<string, Dictionary<int, int>>();
#if UNITY_EDITOR
						debugLeaks=UnityEditor.EditorPrefs.GetBool(nameof(debugLeaks));
#endif
        }

        /// <summary>
        /// Adds the loaded asset to the table along with its coresponding loader. If both already exist incements the count
        /// </summary>
        /// <param name = "guid"></param>
        /// <param name = "loadOperation"></param>
        /// <param name = "loaderID"></param>
	    private static void UpdateLoadTable(string guid, in int loaderID)
        {
            Dictionary<int, int> allLoaders;
            if (!Loadtable.TryGetValue(guid, out allLoaders))
            {
                allLoaders = new Dictionary<int, int>();
                Loadtable.Add(guid, allLoaders);
            }

            if (!allLoaders.ContainsKey(loaderID))
            {
                allLoaders.Add(loaderID, 0);
            }

            Loadtable[guid][loaderID] += 1;
            update = true;
        }

        /// <summary>
        /// LoadAssetAsync wrapper that also updates load table
        /// </summary>
        /// <typeparam name = "TObject"></typeparam>
        /// <param name = "assetref"></param>
        /// <param name = "loader"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(this AssetReference assetref, Object loader)
            where TObject : UnityEngine.Object
        {
            var loadOperation = assetref.LoadAssetAsync<TObject>();
            if (debugLeaks && loadOperation.IsValid())
                UpdateLoadTable(assetref.AssetGUID, loader.GetInstanceID());
            return loadOperation;
        }

        /// <summary>
        /// LoadAssetAsync wrapper that also updates load table
        /// </summary>
        /// <typeparam name = "TObject"></typeparam>
        /// <param name = "assetref"></param>
        /// <param name = "loader"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(this AssetReferenceT<TObject> assetref, Object loader)
            where TObject : UnityEngine.Object
        {
            var loadOperation = assetref.LoadAssetAsync();
            if (debugLeaks && loadOperation.IsValid())
                UpdateLoadTable(assetref.AssetGUID, loader.GetInstanceID());
            return loadOperation;
        }

        /// <summary>
        /// ReleaseAsset wrapper that updates the leak tracker load table
        /// </summary>
        /// <param name = "assetref"></param>
        /// <param name = "unloader"></param>
        public static void ReleaseAsset(this AssetReference assetref, Object unloader)
        {
            assetref.ReleaseAsset();
            if (debugLeaks)
            {
                int instanceID = unloader.GetInstanceID();
                Dictionary<int, int> allLoaders;
                if (Loadtable.TryGetValue(assetref.AssetGUID, out allLoaders) && allLoaders.ContainsKey(instanceID))
                {
                    var newLoadCount = allLoaders[instanceID] - 1;
                    allLoaders[instanceID] = newLoadCount;
                    if (newLoadCount == 0)
                    {
                        Debug.Log("do stuff");
                        // removes this loader from the asset if it has no more loads
                        allLoaders.Remove(unloader.GetInstanceID());
                        // if the asset doesnt have anymore loaders then we also remove it from the dict
                        if (allLoaders.Count == 0)
                            Loadtable.Remove(assetref.AssetGUID);
                    }
                }

                update = true;
            }
        }
    }
}