using UnityEditor;
using UnityEngine;

namespace ObjectManagementSystem.GridBase
{
    /// <summary>
    /// GridBaseObjectManager で管理されるオブジェクトにアタッチされるコンポーネントです。
    /// </summary>
    public class GridBaseManagedObject : ManagedObject
    {
        #region Field

        /// <summary>
        /// Gizmo を描画するかどうか。
        /// </summary>
        public bool drawGizmos = false;

        /// <summary>
        /// Gizmo の色。
        /// </summary>
        public Color gizmosColor = Color.cyan;

        /// <summary>
        /// このオブジェクトの所属する Gird.
        /// </summary>
        protected Vector3Int gridPosition;

        /// <summary>
        /// GridBaseObjectManager.
        /// キャストによるパフォーマンスの低下を回避するために、
        /// base.ObjectManager とは別に参照を維持する。
        /// </summary>
        protected GridBaseObjectManager gridBaseObjectManager;

        #endregion Field

        #region Property

        /// <summary>
        /// このオブジェクトを管理するマネージャを取得します。
        /// </summary>
        public GridBaseObjectManager GridBaseObjectManager
        {
            get { return this.gridBaseObjectManager; }
        }

        /// <summary>
        /// このオブジェクトが所属する Grid 座標を取得します。
        /// </summary>
        public Vector3Int GridPosition
        {
            get { return this.gridPosition; }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// 更新の最後に呼び出されます。
        /// </summary>
        protected virtual void LateUpdate()
        {
            // グリッドを更新します。
            // 最後に一度だけ更新されることが重要です。

            this.gridBaseObjectManager.UpdateGridPosition(this);
        }

        /// <summary>
        /// Gizmo の描画時に呼び出されます。
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (!this.drawGizmos)
            {
                return;
            }

#if UNITY_EDITOR
            Handles.Label(base.transform.position, this.gridPosition.ToString());
#endif
        }

        /// <summary>
        /// 破棄されるときに呼び出されます。
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            ((GridBaseObjectManager)this.objectManager).Grid[this.gridPosition.x]
                                                            [this.gridPosition.y]
                                                            [this.gridPosition.z].Remove(this);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="objectManager">
        /// このインスタンスを管理する ObjectManager.
        /// </param>
        /// <returns>
        /// 初期化に成功するとき true, 失敗するとき false.
        /// </returns>
        public override bool Initialize(ObjectManager objectManager)
        {
            bool result = base.Initialize(objectManager);

            if (result)
            {
                this.gridBaseObjectManager = (GridBaseObjectManager)objectManager;
            }

            return result;
        }

        /// <summary>
        /// Grid 座標を更新します。
        /// このメソッドはこのオブジェクトを管理する GridBaseBoidManager から呼び出されます。 
        /// それ以外の目的で使用するべきではありません。
        /// </summary>
        /// <param name="manager">
        /// このオブジェクトを管理するマネージャ。
        /// </param>
        /// <param name="gridPosition">
        /// 新しい Grid 座標。
        /// </param>
        /// <returns>
        /// 更新に成功するとき true, 失敗するとき false.
        /// </returns>
        public virtual bool UpdateGridPosition(GridBaseObjectManager manager, Vector3Int gridPosition)
        {
            // このメソッドは GridBaseObjectManager の外からの破壊的操作を防ぐために実装されます。

            if (this.gridBaseObjectManager != manager)
            {
                return false;
            }

            this.gridPosition = gridPosition;

            return true;
        }

        #endregion Method
    }
}