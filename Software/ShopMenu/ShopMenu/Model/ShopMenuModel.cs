namespace ShopMenu
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class ShopMenuModel
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        public int MCityId { get; set; }
        /// <summary>
        /// 类别：大荤，小荤，谷物，蔬菜，水果，零食等
        /// </summary>
        public string MType { get; set; }
        /// <summary>
        /// 超市Id
        /// </summary>
        public int MShopId { get; set; }
    }
}