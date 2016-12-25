namespace WMAPP.Models
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月26日 06:40
    /// 标题：微软人脸识别API的脸部矩阵
    /// 描述：顶部左侧点坐标（left,top）,脸宽Width，脸高Height
    /// </summary>
    public partial class FaceRectangleModel
    {
        private int _width;
        /// <summary>
        /// 脸的宽度
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (value < 0)
                {
                    _width = 0;
                }
                else
                {
                    _width = value;
                }
            }
        }
       
        private int _height;
        /// <summary>
        /// 脸的高度
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (value < 0)
                {
                    _height = 0;
                }
                else
                {
                    _height = value;
                }
            }
        }
       
       
        private int _left;
        /// <summary>
        /// 顶部左侧点-x坐标
        /// </summary>
        public int Left
        {
            get
            {
                return _left;
            }

            set
            {
                if (value < 0)
                {
                    _left = 0;
                }
                else
                {
                    _left = value;
                }
            }
        }
        
        private int _top;
        /// <summary>
        /// 顶部左侧点-Y坐标
        /// </summary>
        public int Top
        {
            get
            {
                return _top;
            }

            set
            {
                if (value < 0)
                {
                    _top = 0;
                }
                else
                {
                    _top = value;
                }
            }
        }
    }
}