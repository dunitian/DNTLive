using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ShopMenu
{
    public partial class CityForm : Form
    {
        public CityForm()
        {
            InitializeComponent();
        }
        private TempModel tempModel = new TempModel() { ConnStr = "data Source =.; Initial Catalog = 数据库名; user id = sa; password = 密码" };
        /// <summary>
        /// 初始化的时候数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CityForm_Load(object sender, EventArgs e)
        {
            var model = new TempModel() { ConnStr = tempModel.ConnStr };
            Init(model);//为了让用户可以手动设置connstr

            var jsonStr = File.ReadAllText("App/DNT.json", Encoding.UTF8);
            model = jsonStr.JsonToModels<TempModel>();
            if (model != null && !string.IsNullOrEmpty(model.ConnStr))
            {
                if (model.CityId != 0)
                {
                    ShowMainForm();
                }
                else
                {
                    tempModel.ConnStr = model.ConnStr;
                    try
                    {
                        BindCityInfoById(cityName1, 1);
                    }
                    catch
                    {
                        MessageBox.Show("请查看App文件夹下的DNT.json文件", "是不是连接字符串有问题呢？");
                        Application.Exit();
                    }
                    JiangSu();
                }
            }
        }

        /// <summary>
        /// 查询CityInfo
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="pid"></param>
        private void BindCityInfoById(ComboBox comboBox, int pid)
        {
            var list = ShopMenuHelper.GetCityInfoList<CityModel>(tempModel.ConnStr, pid);
            if (list == null) { return; }
            comboBox.Items.Clear();
            foreach (var item in list)
            {
                comboBox.Items.Add(item);
            }
        }

        /// <summary>
        /// 初始化江苏数据
        /// </summary>
        private void JiangSu()
        {
            cityName1.SelectedIndex = 9;//江苏省
            cityName2.SelectedIndex = 1;//无锡市
            cityName3.SelectedIndex = 6;//滨湖区
        }

        /// <summary>
        /// 准备初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInit_Click(object sender, EventArgs e)
        {
            var cityModel = cityName3.SelectedItem as CityModel;
            if (cityModel != null)
            {
                tempModel.CityId = cityModel.CId;
            }
            File.WriteAllText("App/DNT.json", tempModel.ObjectToJson(), Encoding.UTF8);
            ShowMainForm();
        }

        /// <summary>
        /// 显示主窗体
        /// </summary>
        private static void ShowMainForm()
        {
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="model"></param>
        private void Init(TempModel model)
        {
            var msg = ShopMenuHelper.Init(model);
            if (msg != "初始化成功")
            {
                MessageBox.Show(msg);
            }
        }

        private void cityName1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) { return; }
            var model = comboBox.SelectedItem as CityModel;
            if (model == null) { return; }
            BindCityInfoById(cityName2, model.CId);
            cityName3.Items.Clear();//记得清一下
        }

        private void cityName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) { return; }
            var model = comboBox.SelectedItem as CityModel;
            if (model == null) { return; }
            BindCityInfoById(cityName3, model.CId);
        }

        private void cityName3_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnInit.Enabled = true;
        }
    }
}
