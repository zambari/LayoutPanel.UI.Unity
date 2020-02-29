using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Z.LayoutPanel
{
    [RequireComponent(typeof(LayoutNameHelper))]
    [DisallowMultipleComponent]
    public class LayoutCreator : MonoBehaviourWithBg, IProvideLayoutNameHelperSettings, IProvideLayoutGroupSettings
    {
        [Header("Control object naming here:")]
        public LayoutNameHelperSettings namingSettings = new LayoutNameHelperSettings();
        public LayoutGroupSettings groupSettings = new LayoutGroupSettings();
        [ExposeMethodInEditor]
        void AddEasyLayoutCreator()
        {
            gameObject.AddOrGetComponent<LayoutCreatorEasy>();

        }
        [ExposeMethodInEditor]
        void AddAdvancedLayoutCreator()
        {
            gameObject.AddOrGetComponent<LayoutCreatorAdvanced>();
            var easy = gameObject.GetComponent<LayoutCreatorEasy>();
            if (easy != null) DestroyImmediate(easy);
        }

        public LayoutNameHelperSettings GetSettings()
        {
            return namingSettings;
        }
        void OnValidate()
        {
            if (isActiveAndEnabled)
            {
                // BroadcastMessage("UpdateName");
                var names = GetComponentsInChildren<LayoutNameHelper>();
                foreach (var n in names) n.UpdateName();
                var panels = GetComponentsInChildren<LayoutPanel>();
                foreach (var p in panels) p.SetGroupSettings(groupSettings);
                // BroadcastMessage("ApplyGroupSettings");
            }
        }

        LayoutGroupSettings IProvideLayoutGroupSettings.GetGroupSettings()
        {
            return groupSettings;
        }
    }
}