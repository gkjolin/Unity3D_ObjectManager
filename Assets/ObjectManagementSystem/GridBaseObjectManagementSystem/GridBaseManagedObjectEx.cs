using System.Collections.Generic;

namespace ObjectManagementSystem.GridBase
{
    /// <summary>
    /// GridBaseManagedObject の拡張メソッドやユーティリティメソッドを実装します。
    /// </summary>
    public static class GridBaseManagedObjectEx
    {
        #region Extensions

        /// <summary>
        /// 同じマネージャーで管理されるすべてのオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="self">
        /// GridBaseManagedObject.
        /// </param>
        /// <returns>
        /// 同じマネージャーで管理されるすべてのオブジェクト。
        /// </returns>
        public static List<ManagedObject> GetAllObjects(this GridBaseManagedObject self)
        {
            return self.GridBaseObjectManager.ManagedObjects;
        }

        /// <summary>
        /// 同じグリッドに所属するオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="self">
        /// GridBaseManagedObject.
        /// </param>
        /// <returns>
        /// 同じグリッドに所属するすべてのオブジェクト。
        /// </returns>
        public static List<GridBaseManagedObject> GetNearObjects(this GridBaseManagedObject self)
        {
            return self.GridBaseObjectManager.GetObjects(self.GridPosition);
        }

        /// <summary>
        /// 周辺のグリッドに所属するオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="self">
        /// GridBaseManagedObject.
        /// </param>
        /// <returns>
        /// 周辺のグリッドに所属するすべてのオブジェクト。
        /// </returns>
        public static List<List<GridBaseManagedObject>> GetAroundObjects(this GridBaseManagedObject self)
        {
            return self.GridBaseObjectManager.GetAroundObjects(self.GridPosition);
        }

        #endregion Extensions
    }
}