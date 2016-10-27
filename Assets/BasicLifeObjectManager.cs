using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// BasicLifeObject の生成と参照を管理します。
    /// </summary>
    public class BasicLifeObjectManager : ObjectManager
    {
        #region Field

        /// <summary>
        /// BasicLifeObject のパラメータ設定。
        /// </summary>
        public BasicLifeObjectParameter basicLifeObjectParameter;

        #endregion Field

        #region Method

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
            float moveSpeed = BasicLifeObjectParameter.CalcParameter
                              (this.basicLifeObjectParameter.moveSpeed,
                               this.basicLifeObjectParameter.moveSpeedRandomness);

            float animationSpeed = BasicLifeObjectParameter.CalcParameter
                                   (this.basicLifeObjectParameter.animationSpeed,
                                    this.basicLifeObjectParameter.animationSpeedRandomness);

            float lifeTimeSec = BasicLifeObjectParameter.CalcParameter
                                (this.basicLifeObjectParameter.lifeTimeSec,
                                 this.basicLifeObjectParameter.lifeTimeSecRandomness);

            float scale = BasicLifeObjectParameter.CalcParameter
                          (this.basicLifeObjectParameter.scale,
                           this.basicLifeObjectParameter.scale);

            BasicLifeObject basicLifeObject = newObject.AddComponent<BasicLifeObject>();
            basicLifeObject.Initialize(moveSpeed, animationSpeed, lifeTimeSec);

            newObject.transform.localScale *= scale;
        }

        #endregion Method
    }
}