using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
namespace LeakTracker.Editor
{
    public class LoaderItem : TreeViewItem
    {
        public readonly int ID;
        public readonly Object loader;
        public readonly System.Type loaderType;
        public readonly string loadCount;
        public readonly bool isDeadLoder;
        public LoaderItem(int newID, int newloadCount)
        {
            ID = newID;
            loader = EditorUtility.InstanceIDToObject(newID);
            isDeadLoder = loader == null;
            if (!isDeadLoder)
                loaderType = loader.GetType();
            loadCount = newloadCount.ToString();
        }
    }
}
