using UnityEngine;

namespace ObjectManager
{
    public class BasicLifeObject : MonoBehaviour
    {
        #region Field

        protected float animationSpeed;
        protected float moveSpeed;
        protected float lifeTimeSec;

        private Vector3 targetPosition;

        #endregion Field

        #region Method

        /// <summary>
        /// ゲームの更新の度に呼び出されます。
        /// </summary>
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
        /// <param name="animationSpeed">
        /// アニメーション速度。
        /// </param>
        /// <param name="moveSpeed">
        /// 移動速度。
        /// </param>
        /// <param name="lifeTimeSec">
        /// 寿命(sec)。
        /// </param>
        public void Initialize(float animationSpeed, float moveSpeed, float lifeTimeSec)
        {
            this.animationSpeed = animationSpeed;
            this.moveSpeed = moveSpeed;
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