namespace ShopMenu
{
    public class CityModel
    {
        /// <summary>
        /// 城市Id
        /// </summary>
        public int CId { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public int CPId { get; set; }

        public override string ToString()
        {
            return CName;
        }
    }
}
