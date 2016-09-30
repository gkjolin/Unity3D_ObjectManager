using UnityEngine;
using UnityEngine.Networking;

namespace ObjectManager.UNET
{
    /// <summary>
    /// あるオブジェクトの生成と参照を管理します。
    /// </summary>
    public abstract class ObjectManager : NetworkBehaviour
    {
        #region Field

        public Transform objectParent;

        public GameObject[] objectArray;

        protected ManagedObjectManager managedObjectManager;

        public int managedObjectMaxCount = 10;

        #endregion Field

        #region Method

        /// <summary>
        /// サーバーの開始時に呼び出されます。
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();

            this.managedObjectManager = new ManagedObjectManager();
            this.managedObjectManager.managedObjectMaxCount = this.managedObjectMaxCount;
        }

        /// <summary>
        /// 更新の度に呼び出されます。
        /// </summary>
        [ServerCallback]
        protected virtual void Update()
        {
            this.managedObjectManager.managedObjectMaxCount = this.managedObjectMaxCount;
        }

        /// <summary>
        /// 指定した種類のオブジェクトを生成して追加します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// 追加するオブジェクトのインデックス。
        /// </param>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        [Server]
        public virtual GameObject AddNewObject(int objectArrayIndex)
        {
            if (this.managedObjectManager.CheckManagedObjectCountIsMax())
            {
                return null;
            }

            GameObject newGameObject = GenerateObject(objectArrayIndex);
            this.managedObjectManager.AddManagedObject(newGameObject);

            return newGameObject;
        }

        /// <summary>
        /// 新しいオブジェクトをランダムに生成して追加します。
        /// </summary>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        [Server]
        public virtual GameObject AddNewObject()
        {
            return AddNewObject(Random.Range(0, this.objectArray.Length));
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
        [Server]
        protected virtual GameObject GenerateObject(int objectArrayIndex)
        {
            GameObject newObject = GameObject.Instantiate(this.objectArray[objectArrayIndex]);
            newObject.transform.parent = this.objectParent;

            Initialize(objectArrayIndex, newObject);

            NetworkServer.Spawn(newObject);

            return newObject;
        }

        /// <summary>
        /// ランダムな種類の新しいオブジェクトを生成して返します。
        /// </summary>
        /// <returns>
        /// 新しいオブジェクトのインスタンス。
        /// </returns>
        [Server]
        protected virtual GameObject GenerateObject()
        {
            int objectIndex = Random.Range(0, this.objectArray.Length);

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
        [Server]
        protected abstract void Initialize(int objectArrayIndex, GameObject newObject);

        #endregion Method
    }
}