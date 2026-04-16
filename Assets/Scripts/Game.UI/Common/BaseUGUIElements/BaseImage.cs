using UnityEngine;
using UnityEngine.UI;
using Game.Common;

namespace Game.UI.Common.BaseUGUIElements
{
    [RequireComponent(typeof(Image))]
    public class BaseImage : SaiMonoBehaviour
    {
        [Header("Base Image")]
        [SerializeField] protected Image image;

        public Image Image => image;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.image);
        }
    }
}