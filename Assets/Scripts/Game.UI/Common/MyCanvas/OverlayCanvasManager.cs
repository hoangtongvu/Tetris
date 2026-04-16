using System.ComponentModel;
using UnityEngine;
using Game.Common;
using Game.UI.Common.UIRendering;

namespace Game.UI.Common.MyCanvas
{
	public class OverlayCanvasManager : SaiMonoBehaviour
    {
		[SerializeField, ReadOnly(true)]
        private Transform[] anchorPresetTransforms = new Transform[CanvasAnchorPreset_Length.Value];

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadAnchorPresets();
        }

        private void LoadAnchorPresets()
        {
            for (int i = 0; i < CanvasAnchorPreset_Length.Value; i++)
            {
                var preset = (CanvasAnchorPreset)i;
                string gameObjName = preset.ToString();
                this.LoadTransformInChildrenByName(out this.anchorPresetTransforms[i], gameObjName);
            }
        }

        public Transform GetAnchorPresetTransform(int index)
        {
            var anchorPreset = anchorPresetTransforms[index];
            var presetType = (CanvasAnchorPreset)index;

            if (anchorPreset == null)
            {
                Debug.LogError($"Can't find CanvasAnchorPreset: <b>{presetType}</b>", this);
            }

            return anchorPreset;
        }
    }
}