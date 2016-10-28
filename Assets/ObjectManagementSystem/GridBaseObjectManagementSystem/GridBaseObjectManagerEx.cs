using UnityEngine;

namespace ObjectManagementSystem.GridBase
{
    /// <summary>
    /// GridBaseObjectManager の拡張メソッドやユーティリティメソッドを実装します。
    /// </summary>
    public static class GridBaseObjectManagerEx
    {
        #region Extensions

        /// <summary>
        /// 指定した Grid の中からランダムな座標を取得します。
        /// </summary>
        /// <param name="self">
        /// Grid を管理するマネージャ。
        /// </param>
        /// <param name="gridPosition">
        /// グリッド座標。
        /// </param>
        /// <returns>
        /// Grid の中の座標。
        /// </returns>
        public static Vector3 GetRandomPositionInGrid
            (this GridBaseObjectManager self, Vector3Int gridPosition)
        {
            return GetRandomPositionInGrid(self, gridPosition.x, gridPosition.y, gridPosition.z);
        }

        /// <summary>
        /// 指定した Grid の中からランダムな座標を取得します。
        /// </summary>
        /// <param name="self">
        /// Grid を管理するマネージャ。
        /// </param>
        /// <param name="gridPositionX">
        /// グリッド座標 X.
        /// </param>
        /// <param name="gridPositionY">
        /// グリッド座標 Y.
        /// </param>
        /// <param name="gridPositionZ">
        /// グリッド座標 Z.
        /// </param>
        /// <returns>
        /// Grid の中の座標。
        /// </returns>
        public static Vector3 GetRandomPositionInGrid(this GridBaseObjectManager self,
                                                      int gridPositionX,
                                                      int gridPositionY,
                                                      int gridPositionZ)
        {
            Bounds gridBounds = self.GetGridBounds(gridPositionX, gridPositionY, gridPositionZ);

            return new Vector3()
            {
                x = Random.Range(gridBounds.min.x, gridBounds.max.x),
                y = Random.Range(gridBounds.min.y, gridBounds.max.y),
                z = Random.Range(gridBounds.min.z, gridBounds.max.z),
            };
        }

        /// <summary>
        /// すべての Grid の中からランダムな座標を取得します。
        /// </summary>
        /// <param name="self">
        /// Grid を管理するマネージャ。
        /// </param>
        /// <returns>
        /// Grid の中の座標。
        /// </returns>
        public static Vector3 GetRandomPositionInGrids(this GridBaseObjectManager self)
        {
            return new Vector3()
            {
                x = Random.Range(self.transform.position.x, self.gridSize.x),
                y = Random.Range(self.transform.position.y, self.gridSize.y),
                z = Random.Range(self.transform.position.z, self.gridSize.z),
            };
        }

        #endregion Extensions
    }
}