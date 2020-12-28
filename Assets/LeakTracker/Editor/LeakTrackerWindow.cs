using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace LeakTracker.Editor
{
    public class LeakTrackerWindow : EditorWindow
    {
        const string TRACK_LEAKS = "Track Leaks";
        [SerializeField]
        bool toggle;
        LeakTrackerTree leakTreeView;
        [MenuItem("Tools/LeakWindow")]
        private static void OpenWindow()
        {
            var leakWindow = GetWindow<LeakTrackerWindow>();
            leakWindow.titleContent = new GUIContent("Leak Tracker Window");
            leakWindow.Show();
        }

        protected void OnEnable()
        {
            toggle = EditorPrefs.GetBool("debugLeaks");
            leakTreeView = new LeakTrackerTree(new TreeViewState(), position.width);
        }

        protected void OnInspectorUpdate()
        {
            if (mouseOverWindow != this && Application.isPlaying)
                Repaint();
        }

        protected void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            LeakToggle();
            Refresh();
            GarbageCollect();
            EditorGUILayout.EndHorizontal();
            var rect = GUILayoutUtility.GetRect(position.width, position.height - 5);
            rect.y += 5;
            if (LeakTracker.update)
            {
                leakTreeView.Reload();
                LeakTracker.update = false;
            }

            leakTreeView.OnGUI(rect);
        }

        private void LeakToggle()
        {
            EditorGUI.BeginChangeCheck();
            toggle = EditorGUILayout.ToggleLeft(TRACK_LEAKS, toggle);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("debugLeaks", toggle);
                if (Application.isPlaying)
                {
                    var binding_flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
                    typeof(LeakTracker).GetField("debugLeaks", binding_flags).SetValue(toggle, null);
                }
            }
        }

        private void Refresh()
        {
            if (GUILayout.Button("Refresh"))
                leakTreeView?.Reload();
        }

        private void GarbageCollect()
        {
            if (GUILayout.Button("GC.collect") && Application.isPlaying)
                System.GC.Collect();
        }
    }
}