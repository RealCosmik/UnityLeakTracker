using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
namespace LeakTracker.Editor
{
    public class AssetItem : TreeViewItem
    {
        public readonly string guid;
        public readonly Object asset;
        public readonly System.Type assetType;
        public readonly bool isDeadAsset;
        public AssetItem(string newguid)
        {
            guid = newguid;
            asset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(newguid));
            isDeadAsset = asset == null;
            if (!isDeadAsset)
                assetType = asset.GetType();
        }
    }
}
