using UnityEngine;

namespace ObjectManager
{
    /// <summary>
    /// BasicLifeObject の生成と参照を管理します。
    /// </summary>
    public class BasicLifeObjectManager : ObjectManager
    {
        #region Field

        public bool enableKeyControl = true;
        public KeyCode addNewObjectKey = KeyCode.Return;
        public KeyCode destroyAllObjectKey = KeyCode.Delete;

        public float animationSpeed;
        public float moveSpeed;
        public float lifeTimeSec;
        public float scale;

        public float animationSpeedRandomness;
        public float moveSpeedRandomness;
        public float lifeTimeSecRandomness;
        public float scaleRandomness;

        #endregion Field

        /// <summary>
        /// 新しく生成されたオブジェクトを初期化します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// 何番目のオブジェクトが生成されたかを示すインデックス。
        /// <param name="newObject">
        /// 新しく生成されたオブジェクト。
        /// </param>
        protected override void Initialize(int objectArrayIndex, GameObject newObject)
        {
            BasicLifeObject basicLifeObject = newObject.AddComponent<BasicLifeObject>();

            basicLifeObject.Initialize(CalcParameter(this.moveSpeed, this.moveSpeedRandomness),
                                       CalcParameter(this.animationSpeed, this.animationSpeedRandomness),
                                       CalcParameter(this.lifeTimeSec, this.lifeTimeSecRandomness));

            newObject.transform.localScale *= CalcParameter(this.scale, this.scaleRandomness);
        }

        /// <summary>
        /// 変動率を考慮したパラメータを算出します。randomness に 0.2 を設定するとき、
        /// parameter * (1 - 0.2) ~ parameter * (1 + 0.2) の範囲の値が得られます。
        /// </summary>
        /// <param name="parameter">
        /// 正規のパラメータ。
        /// </param>
        /// <param name="randomness">
        /// パラメータの変動率。
        /// </param>
        /// <returns>
        /// 変動率を考慮したパラメータ。
        /// </returns>
        protected virtual float CalcParameter(float parameter, float randomness)
        {
            return parameter * Random.Range(1 - randomness, 1 + randomness);
        }
    }
}