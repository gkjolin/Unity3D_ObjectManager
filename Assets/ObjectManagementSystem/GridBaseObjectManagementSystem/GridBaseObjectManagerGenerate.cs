using UnityEngine;
using UnityEngine.Networking;

namespace ObjectManagementSystem.GridBase
{
    /// <summary>
    /// オブジェクトの生成機能を持つ GridBaseObjectManager です。
    /// </summary>
    public abstract class GridBaseObjectManagerGenerate : GridBaseObjectManager
    {
        #region Field

        /// <summary>
        /// UNET を使った同期のために生成したオブジェクトをスポーンするかどうか。
        /// true のときスポーンします。
        /// </summary>
        public bool enableSpawn;

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
        /// <param name="position">
        /// 追加するオブジェクトの座標。
        /// </param>
        /// <param name="rotation">
        /// 追加するオブジェクトの回転。
        /// </param>
        /// <param name="scale">
        /// 追加するオブジェクトの大きさ。
        /// </param>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        public virtual GameObject AddObject
            (int objectArrayIndex, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (base.CheckManagedObjectCountIsMax())
            {
                return null;
            }

            GameObject newObject = GenerateObject(objectArrayIndex, position, rotation, scale);
            base.AddManagedObject(newObject);

            return newObject;
        }

        /// <summary>
        /// 新しいオブジェクトをランダムに生成して追加します。
        /// 管理するオブジェクトの数が最大の場合などに失敗します。
        /// </summary>
        /// <param name="position">
        /// 追加するオブジェクトの座標。
        /// </param>
        /// <param name="rotation">
        /// 追加するオブジェクトの回転。
        /// </param>
        /// <param name="scale">
        /// 追加するオブジェクトの大きさ。
        /// </param>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        public virtual GameObject AddObject
            (Vector3 position, Vector3 rotation, Vector3 scale)
        {
            return AddObject(Random.Range(0, this.generateObjects.Length),
                             position,
                             rotation,
                             scale);
        }

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
            return AddObject(objectArrayIndex,
                             Vector3.zero,
                             Vector3.zero,
                             Vector3.one);
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
        /// <param name="position">
        /// オブジェクトの座標。
        /// </param>
        /// <param name="rotation">
        /// オブジェクトの回転。
        /// </param>
        /// <param name="scale">
        /// オブジェクトの大きさ。
        /// </param>
        /// <returns>
        /// 生成したオブジェクトのインスタンス。
        /// </returns>
        protected virtual GameObject GenerateObject
            (int objectArrayIndex, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            // スポーン前に設定されたパラメータは同期します。

            GameObject newObject = GameObject.Instantiate(this.generateObjects[objectArrayIndex]);

            newObject.transform.position = position;
            newObject.transform.rotation = Quaternion.Euler(rotation);
            newObject.transform.localScale = scale;
            newObject.transform.parent = this.objectParent;

            InitializeObject(objectArrayIndex, newObject);

            if (this.enableSpawn)
            {
                NetworkServer.Spawn(newObject);
            }

            return newObject;
        }

        /// <summary>
        /// 新しく生成されたオブジェクトを初期化します。
        /// enableSpawn が有効なとき、スポーンするよりも前に呼び出される点に注意してください。
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