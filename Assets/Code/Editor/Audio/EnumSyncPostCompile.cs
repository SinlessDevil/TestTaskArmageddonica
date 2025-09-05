using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Code.Editor.AudioVibration
{
    [InitializeOnLoad]
    public static class EnumSyncPostCompile
    {
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Debug.Log("Scripts recompiled. Trying to sync enums...");

            SoundLibraryEditorWindow soundWindow = Resources.FindObjectsOfTypeAll<SoundLibraryEditorWindow>().FirstOrDefault();
            if (soundWindow == null) 
                return;
            
            soundWindow.UpdateSoundTypesAfterReload();
            Debug.Log("SoundTypes synced!");
        }
    }
}