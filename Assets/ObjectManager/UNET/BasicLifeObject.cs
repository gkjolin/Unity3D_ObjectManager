using UnityEngine;
using UnityEngine.Networking;

namespace ObjectManager.UNET
{
    /// <summary>
    /// 基本的な生命体を表します。
    /// </summary>
    public class BasicLifeObject : NetworkBehaviour
    {
        #region Field

        /// <summary>
        /// 移動速度。
        /// </summary>
        protected float moveSpeed;

        /// <summary>
        /// 動く速度。
        /// </summary>
        protected float animationSpeed;

        /// <summary>
        /// 寿命。
        /// </summary>
        protected float lifeTimeSec;

        /// <summary>
        /// ターゲットの座標。
        /// </summary>
        private Vector3 targetPosition;

        #endregion Field

        #region Method

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        [ServerCallback]
        protected virtual void Update()
        {
            this.transform.position = Vector3.MoveTowards
                (this.transform.position,
                 this.targetPosition,
                 Time.deltaTime * this.moveSpeed);

            this.transform.Rotate(this.animationSpeed,
                                  this.animationSpeed,
                                  this.animationSpeed);

            if (this.transform.position == this.targetPosition)
            {
                SetTargetPosition();
            }
        }

        /// <summary>
        /// パラメータを初期化します。
        /// </summary>
        /// <param name="moveSpeed">
        /// 移動速度。
        /// </param>
        /// <param name="animationSpeed">
        /// アニメーション速度。
        /// </param>
        /// <param name="lifeTimeSec">
        /// 寿命(sec)。
        /// </param>
        public void Initialize(float moveSpeed, float animationSpeed, float lifeTimeSec)
        {
            this.moveSpeed = moveSpeed;
            this.animationSpeed = animationSpeed;
            this.lifeTimeSec = lifeTimeSec;

            Destroy(this.gameObject, this.lifeTimeSec);

            SetTargetPosition();
        }

        /// <summary>
        /// 進行方向を決定します。
        /// </summary>
        private void SetTargetPosition()
        {
            this.targetPosition = Random.onUnitSphere * 8;
        }

        #endregion Method
    }
}