using System.Collections.Generic;
using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// オブジェクトの生成機能を持つ ObjectManager です。
    /// </summary>
    public abstract class ObjectManagerGenerate : ObjectManager
    {
        #region Field

        /// <summary>
        /// 生成するオブジェクトの親。
        /// </summary>
        public Transform objectParent;

        /// <summary>
        /// 生成するオブジェクト。生成されたオブジェクトではありません。
        /// </summary>
        public GameObject[] generateObjects;

        #endregion Field

        #region Method

        /// <summary>
        /// 指定した種類のオブジェクトを生成して追加します。
        /// 管理するオブジェクトの数が最大の場合などに失敗します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// 追加するオブジェクトのインデックス。
        /// </param>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        public virtual GameObject AddObject(int objectArrayIndex)
        {
            if (base.CheckManagedObjectCountIsMax())
            {
                return null;
            }

            GameObject newObject = GenerateObject(objectArrayIndex);
            base.AddManagedObject(newObject);

            return newObject;
        }

        /// <summary>
        /// 新しいオブジェクトをランダムに生成して追加します。
        /// 管理するオブジェクトの数が最大の場合などに失敗します。
        /// </summary>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        public virtual GameObject AddObject()
        {
            return AddObject(Random.Range(0, this.generateObjects.Length));
        }

        /// <summary>
        /// 指定した種類のオブジェクトを生成して返します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// オブジェクトの種類。
        /// </param>
        /// <returns>
        /// 新しいオブジェクトのインスタンス。
        /// </returns>
        protected virtual GameObject GenerateObject(int objectArrayIndex)
        {
            GameObject newObject = GameObject.Instantiate(this.generateObjects[objectArrayIndex]);
            newObject.transform.parent = this.objectParent;

            InitializeObject(objectArrayIndex, newObject);

            return newObject;
        }

        /// <summary>
        /// ランダムな種類の新しいオブジェクトを生成して返します。
        /// </summary>
        /// <returns>
        /// 新しいオブジェクトのインスタンス。
        /// </returns>
        protected virtual GameObject GenerateObject()
        {
            int objectIndex = Random.Range(0, this.generateObjects.Length);

            return GenerateObject(objectIndex);
        }

        /// <summary>
        /// 新しく生成されたオブジェクトを初期化します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// 何番目のオブジェクトが生成されたかを示すインデックス。
        /// </param>
        /// <param name="newObject">
        /// 新しく生成されたオブジェクト。
        /// </param>
        protected abstract void InitializeObject(int objectArrayIndex, GameObject newObject);

        #endregion Method
    }
}