using Game.Common;
using Game.Domain;
using Reflex.Core;
using SaintsField.Playa;
using UnityEngine;

namespace Game.Mono
{
    [RequireComponent(typeof(BoardConfigHolder))]
    public class BoardCellArrayHolder : SaiMonoBehaviour, IInstaller
    {
        [SerializeField] private BoardConfigHolder config;
        [ShowInInspector] private BoardCellArray value;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.config);
        }

        public void InstallBindings(ContainerBuilder builder)
        {
            this.InitBoardCells();
            builder.RegisterValue(this.value);
        }

        private void InitBoardCells()
        {
            var cellArray = new CellData[this.config.Value.Width][];

            for (int i = 0; i < this.config.Value.Width; i++)
            {
                cellArray[i] = new CellData[this.config.Value.Height];
            }

            this.value = new()
            {
                Value = cellArray,
            };
        }
    }
}