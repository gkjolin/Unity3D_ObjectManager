using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// 基本的な生命体のパラメータ。
    /// </summary>
    [System.Serializable]
    public class BasicLifeObjectParameter
    {
        #region Field

        #region Parameter

        /// <summary>
        /// 移動速度。
        /// </summary>
        public float moveSpeed = 1;

        /// <summary>
        /// 動作速度。
        /// </summary>
        public float animationSpeed = 1;

        /// <summary>
        /// 大きさ。
        /// </summary>
        public float scale = 1;

        /// <summary>
        /// 寿命。
        /// </summary>
        public float lifeTimeSec  = 30;

        #endregion Parameter

        #region Randomness

        /// <summary>
        /// 移動速度のランダム値。
        /// </summary>
        public float moveSpeedRandomness = 0.5f;

        /// <summary>
        /// 動作速度のランダム値。
        /// </summary>
        public float animationSpeedRandomness = 0.5f;

        /// <summary>
        /// 大きさのランダム値。
        /// </summary>
        public float scaleRandomness = 0.5f;

        /// <summary>
        /// 寿命のランダム値。
        /// </summary>
        public float lifeTimeSecRandomness = 0.5f;

        #endregion Randomness

        #endregion Field

        #region Method

        /// <summary>
        /// ランダム値を考慮したパラメータを算出します。
        /// parameter * (1 - randomness) ~ parameter * (1 + randomness) の値が得られます。
        /// </summary>
        /// <param name="parameter">
        /// 元のパラメータ。
        /// </param>
        /// <param name="randomness">
        /// パラメータのランダム値。
        /// </param>
        /// <returns>
        /// パラメータ。
        /// </returns>
        public static float CalcParameter(float parameter, float randomness)
        {
            return parameter * Random.Range(1 - randomness, 1 + randomness);
        }

        #endregion Method
    }
}