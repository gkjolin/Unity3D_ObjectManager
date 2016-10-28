using UnityEngine;

namespace XJ.Unity3D.Utility.ObjectController
{
    /// <summary>
    /// 直方体の中をランダムに移動させます。
    /// </summary>
    public class RandomWalkInRectangular : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 移動範囲の最小座標(直方体の前方左下)。
        /// </summary>
        public Vector3 minMovingRange = Vector3.zero;

        /// <summary>
        /// 移動範囲の最大座標(直方体の後方右上)。
        /// </summary>
        public Vector3 maxMovingRange = Vector3.one * 10f;

        /// <summary>
        /// 移動速度。
        /// </summary>
        public float moveSpeed = 5;

        /// <summary>
        /// 移動先。
        /// </summary>
        private Vector3 targetPosition;

        #endregion Field

        #region Method

        /// <summary>
        /// 開始時に呼び出されます。
        /// </summary>
        protected virtual void Start()
        {
            UpdateTargetPosition();
        }

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            this.transform.position
            = Vector3.MoveTowards(this.transform.position,
                                  this.targetPosition,
                                  Time.deltaTime * this.moveSpeed);

            if (this.transform.position == this.targetPosition)
            {
                UpdateTargetPosition();
            }
        }

        /// <summary>
        /// 進行方向を決定します。
        /// </summary>
        private void UpdateTargetPosition()
        {
            this.targetPosition = new Vector3()
            {
                x = Random.Range(this.minMovingRange.x, this.maxMovingRange.x),
                y = Random.Range(this.minMovingRange.y, this.maxMovingRange.y),
                z = Random.Range(this.minMovingRange.z, this.maxMovingRange.z)
            };
        }

        #endregion Method
    }
}