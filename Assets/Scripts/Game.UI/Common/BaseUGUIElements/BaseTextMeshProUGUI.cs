using UnityEngine;
using Game.Common;
using TMPro;

namespace Game.UI.Common.BaseUGUIElements
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class BaseTextMeshProUGUI : SaiMonoBehaviour
    {
        [Header("Base TextMeshPro")]
        [SerializeField] protected TextMeshProUGUI text;

        public TextMeshProUGUI TextMeshProUGUI => text;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.text);
        }
    }
}