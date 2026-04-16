using UnityEngine;
using UnityEngine.UI;
using Game.Common;

namespace Game.UI.Common.BaseUGUIElements
{
    [RequireComponent(typeof(Text))]
    public class BaseTextLegacy : SaiMonoBehaviour
    {
        [Header("Base Text Legacy")]
        [SerializeField] protected Text text;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.text);
        }
    }
}