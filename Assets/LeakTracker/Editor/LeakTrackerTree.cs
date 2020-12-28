using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace LeakTracker.Editor
{
    class LeakTrackerTree : TreeView
    {
        static GUIStyle errorStyle;
        static LeakTrackerTree()
        {
            errorStyle = new GUIStyle();
            errorStyle.normal.textColor = Color.red;
        }

        public LeakTrackerTree(TreeViewState state, in float initalWidth) : base(state, CreateTableHeaders(initalWidth / 3f))
        {
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var rootItem = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };
            if (LeakTracker.Loadtable.Count > 0)
                CreateTreeItems(rootItem);
            else
                rootItem.AddChild(new TreeViewItem());
            SetupDepthsFromParentsAndChildren(rootItem);
            return rootItem;
        }

        private void CreateTreeItems(TreeViewItem root)
        {
            int id = 0;
            var table = LeakTracker.Loadtable;
            foreach (var assetslist in table)
            {
                id++;
                var assetItem = new AssetItem(assetslist.Key)
                { id = id };
                foreach (var item in assetslist.Value)
                {
                    id++;
                    var loaderItem = new LoaderItem(item.Key, item.Value)
                    { id = id };
                    assetItem.AddChild(loaderItem);
                }

                root.AddChild(assetItem);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            int visibleCollums = args.GetNumVisibleColumns();
            for (int i = 0; i < visibleCollums; i++)
            {
                DrawTreeElement(in i, in args);
            }
        }

        private void DrawTreeElement(in int collumnNumber, in RowGUIArgs args)
        {
            Rect cellrect = args.GetCellRect(collumnNumber);
            switch (collumnNumber)
            {
                case 0:
                    if (args.item is AssetItem)
                        DrawAssetItem(cellrect, args.item as AssetItem);
                    break;
                case 1:
                    if (args.item is LoaderItem)
                        DrawLoaderItem(cellrect, args.item as LoaderItem);
                    break;
                case 2:
                    if (args.item is LoaderItem)
                        DrawLoaderCount(cellrect, args.item as LoaderItem);
                    break;
                default:
                    break;
            }
        }

        private void DrawAssetItem(Rect cell, AssetItem item)
        {
            GUI.enabled = false;
            cell.width -= 15f;
            cell.x += 15;
            if (!item.isDeadAsset)
                EditorGUI.ObjectField(cell, string.Empty, item.asset, item.assetType, true);
            else
            {
                var red = Color.red;
                red.a = .3f;
                EditorGUI.DrawRect(cell, red);
                EditorGUI.ObjectField(cell, string.Empty, null, item.assetType, true);
            }

            GUI.enabled = true;
        }

        private void DrawLoaderItem(Rect cell, LoaderItem item)
        {
            GUI.enabled = false;
            if (!item.isDeadLoder)
                EditorGUI.ObjectField(cell, string.Empty, item.loader, item.loaderType, true);
            else
            {
                var red = Color.red;
                red.a = .3f;
                EditorGUI.DrawRect(cell, red);
                EditorGUI.ObjectField(cell, string.Empty, null, item.loaderType, true);
            }

            GUI.enabled = true;
        }

        private void DrawLoaderCount(Rect cell, LoaderItem item)
        {
            if (!item.isDeadLoder)
                EditorGUI.LabelField(cell, item.loadCount);
            else
                EditorGUI.LabelField(cell, item.loadCount, errorStyle);
        }

        private static MultiColumnHeader CreateTableHeaders(in float width)
        {
            var collumns = new MultiColumnHeaderState.Column[]
            {
            new MultiColumnHeaderState.Column()
                {   headerContent = new GUIContent("Asset"),
                    autoResize = true,
                    allowToggleVisibility = false,
                    width = width
                 },
            new MultiColumnHeaderState.Column()
                {   headerContent = new GUIContent("Loaders"),
                    autoResize = true,
                    allowToggleVisibility = false,
                    width = width
                },

            new MultiColumnHeaderState.Column()
            {
                headerContent = new GUIContent("LoadCount"),
                autoResize = true,
                allowToggleVisibility = false,
                width = width

            }};
            return new MultiColumnHeader(new MultiColumnHeaderState(collumns));
        }
    }
}