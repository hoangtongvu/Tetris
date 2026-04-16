using UnityEngine;
using Game.Common;

namespace Game.UI.Common.MyCanvas
{
    public class CanvasesCtrl : SaiMonoBehaviour
    {
        [SerializeField] private OverlayCanvasManager overlayCanvasManager;
        [SerializeField] private WorldSpaceCanvasManager worldSpaceCanvasManager;

        public OverlayCanvasManager OverlayCanvasManager => overlayCanvasManager;
        public WorldSpaceCanvasManager WorldSpaceCanvasManager => worldSpaceCanvasManager;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInChildren(out this.overlayCanvasManager);
            this.LoadComponentInChildren(out this.worldSpaceCanvasManager);
        }
    }
}