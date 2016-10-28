using UnityEngine;
using XJ.Unity3D.Utility.ObjectController;
using ObjectManagementSystem;
using ObjectManagementSystem.GridBase;

namespace GridBaseObjectManagementSystem
{
    public class GridBaseObjectManagerDebugger : MonoBehaviour
    {
        #region Field

        public GridBaseObjectManager gridBaseObjectManager;

        public GridBaseObjectManagerDebugAgent debugAgent;

        public KeyCode addManagedObjectsToManagerKey = KeyCode.Return;

        #endregion Field

        /// <summary>
        /// 開始時に呼び出されます。
        /// </summary>
        protected virtual void Start()
        {
            // 任意のタイミングで初期化します。
            // 初期化が完了するまでオブジェクトを追加することができないためです。

            gridBaseObjectManager.Initialize();

            // ランダムに生成したオブジェクトをマネージャーに追加します。

            AddManagedObjectsToManager();

            // エージェントをマネージャーに追加します。

            this.debugAgent.gridBaseManagedObject = (GridBaseManagedObject)this.gridBaseObjectManager
                                                    .AddManagedObject(debugAgent.gameObject);
            this.debugAgent.gridBaseManagedObject.drawGizmos = true;
        }

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            if (Input.GetKeyDown(this.addManagedObjectsToManagerKey))
            {
                AddManagedObjectsToManager();
            }
        }

        /// <summary>
        /// Grid の中にランダムに球を生成します。
        /// </summary>
        protected virtual void AddManagedObjectsToManager()
        {
            for (int x = 0; x < this.gridBaseObjectManager.gridDivide.x; x++)
            {
                for (int y = 0; y < this.gridBaseObjectManager.gridDivide.y; y++)
                {
                    for (int z = 0; z < this.gridBaseObjectManager.gridDivide.z; z++)
                    {
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                        sphere.transform.position = this.gridBaseObjectManager
                                                        .GetRandomPositionInGrid(x, y, z);

                        RandomWalkInRectangular randomwalk
                            = sphere.AddComponent<RandomWalkInRectangular>();
                        randomwalk.minMovingRange = this.gridBaseObjectManager.transform.position;
                        randomwalk.maxMovingRange = this.gridBaseObjectManager.gridSize;

                        this.gridBaseObjectManager.AddManagedObject(sphere);
                    }
                }
            }
        }
    }
}