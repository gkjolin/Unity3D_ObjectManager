using UnityEngine;
using System.Collections.Generic;
using ObjectManagementSystem.GridBase;
using ObjectManagementSystem;

namespace GridBaseObjectManagementSystem
{
    public class GridBaseObjectManagerDebugAgent : MonoBehaviour
    {
        #region Field

        [HideInInspector]
        public GridBaseManagedObject gridBaseManagedObject;

        #endregion Field

        #region Meghod

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            PaintAllObjectColor(Color.blue);
            PaintAroundObjects(Color.yellow);
            PaintNearObjects(Color.red);
        }

        /// <summary>
        /// すべてのオブジェクトの色を変更します。
        /// </summary>
        /// <param name="color">
        /// 設定する色。
        /// </param>
        private void PaintAllObjectColor(Color color)
        {
            List<ManagedObject> allObjects = this.gridBaseManagedObject.GetAllObjects();

            foreach (ManagedObject managedObject in allObjects)
            {
                SetColor(managedObject.gameObject, color);
            }
        }

        /// <summary>
        /// 周辺のグリッドに所属するオブジェクトの色を変更します。
        /// </summary>
        /// <param name="color">
        /// 設定する色。
        /// </param>
        private void PaintAroundObjects(Color color)
        {
            List<List<GridBaseManagedObject>> aroundObjectList = this.gridBaseManagedObject.GetAroundObjects();

            foreach (List<GridBaseManagedObject> aroundObjects in aroundObjectList)
            {
                foreach (GridBaseManagedObject aroundObject in aroundObjects)
                {
                    SetColor(aroundObject.gameObject, color);
                }
            }
        }

        /// <summary>
        /// 同じグリッドに所属するオブジェクトの色を変更します。
        /// </summary>
        /// <param name="color">
        /// 設定する色。
        /// </param>
        private void PaintNearObjects(Color color)
        {
            List<GridBaseManagedObject> nearObjects = this.gridBaseManagedObject.GetNearObjects();

            foreach (GridBaseManagedObject nearObject in nearObjects)
            {
                SetColor(nearObject.gameObject, color);
            }
        }

        /// <summary>
        /// GameObject の色を設定します。
        /// </summary>
        /// <param name="gameObject">
        /// 色を設定する GameObject.
        /// </param>
        /// <param name="color">
        /// GameObject に設定する色。
        /// </param>
        private void SetColor(GameObject gameObject, Color color)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            renderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }

        #endregion Method
    }
}