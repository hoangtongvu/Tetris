using Game.Common;
using Game.UI.Common;
using Game.UI.Common.MyCanvas;
using Game.UI.Common.Pooling;
using Game.UI.Common.UIRendering;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Mono.UI
{
    public class UISetup : SaiMonoBehaviour
    {
        private const string uiPrefabLabel = "UI Prefab";

        private void Awake()
        {
            var poolMap = SharedUICtrlPoolMap.Instance;
            var prefabs = Addressables.LoadAssetsAsync<GameObject>(uiPrefabLabel).WaitForCompletion();

            this.CreateUIToolkitHolders(out var uitkOverlayHolder, out var uitkWorldSpaceHolder);

            foreach (var prefab in prefabs)
            {
                var uiCtrl = prefab.GetComponent<BaseUICtrl>();
                var uiType = uiCtrl.GetUIType();
                var uiRenderingConfig = uiCtrl.UIRenderingConfig;

                var defaultHolderTransform = this.GetDefaultTransformHolder(
                    uiRenderingConfig.RenderingType,
                    uiRenderingConfig.RenderingSpace,
                    uiType,
                    uitkOverlayHolder,
                    uitkWorldSpaceHolder);

                poolMap.Pools.Add(uiType, new()
                {
                    Prefab = prefab,
                    DefaultHolderTransform = defaultHolderTransform,
                });
            }

            Destroy(gameObject);
        }

        private void CreateUIToolkitHolders(out Transform uitkOverlayHolder, out Transform uitkWorldSpaceHolder)
        {
            var overlayHolderGO = new GameObject("[UI Toolkit] OverlayUIsHolder");
            var worldSpaceHolderGO = new GameObject("[UI Toolkit] WorldSpaceUIsHolder");

            DontDestroyOnLoad(overlayHolderGO);
            DontDestroyOnLoad(worldSpaceHolderGO);

            uitkOverlayHolder = overlayHolderGO.transform;
            uitkWorldSpaceHolder = worldSpaceHolderGO.transform;
        }

        private Transform GetDefaultTransformHolder(
            RenderingType renderingType,
            RenderingSpace renderingSpace,
            UIType uiType,
            Transform uitkOverlayHolder,
            Transform uitkWorldSpaceHolder)
        {
            return renderingType switch
            {
                RenderingType.UIToolkit => renderingSpace switch
                {
                    RenderingSpace.Overlay => this.CreateUIToolkitPool(uiType, uitkOverlayHolder),
                    RenderingSpace.WorldSpace => this.CreateUIToolkitPool(uiType, uitkWorldSpaceHolder),
                    _ => null,
                },
                //RenderingType.UGUI => this.CreateUGUIPool(
                //    canvasesCtrl,
                //    uiType,
                //    renderingSpace,
                //    canvasAnchorPreset),
                //_ => null,
            };
        }

        private Transform CreateUIToolkitPool(
            UIType uiType,
            Transform baseHolder)
        {
            GameObject newGameObject = new($"{uiType}_Pool");
            newGameObject.transform.SetParent(baseHolder);

            return newGameObject.transform;
        }

        private Transform CreateUGUIPool(
            CanvasesCtrl canvasesCtrl,
            UIType uiType,
            RenderingSpace renderingSpace,
            CanvasAnchorPreset canvasAnchorPreset)
        {
            GameObject newGameObject = new($"{uiType}_Pool");
            newGameObject.AddComponent<RectTransform>();

            newGameObject.transform.SetParent(this.GetUGUIParentTransform(canvasesCtrl, renderingSpace, canvasAnchorPreset));
            newGameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            return newGameObject.transform;
        }

        private Transform GetUGUIParentTransform(
            CanvasesCtrl canvasesCtrl,
            RenderingSpace renderingSpace,
            CanvasAnchorPreset canvasAnchorPreset)
        {
            switch (renderingSpace)
            {
                case RenderingSpace.WorldSpace:
                    return canvasesCtrl.WorldSpaceCanvasManager.transform;
                case RenderingSpace.Overlay:
                    int index = (int)canvasAnchorPreset;
                    return canvasesCtrl.OverlayCanvasManager.GetAnchorPresetTransform(index);
                default:
                    return null;
            }
        }
    }
}