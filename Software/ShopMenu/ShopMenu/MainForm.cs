using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ShopMenu
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string path = string.Empty;
        private TempModel tempModel = new TempModel();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            tempModel = GetModel();            
        }

        /// <summary>
        /// 读取配置文件对象
        /// </summary>
        /// <returns></returns>
        private TempModel GetModel()
        {
            //能到MainForm，那么App / DNT.json肯定存在了(以防用户在运行到Main窗体的时候手动删掉)
            if (!File.Exists("App/DNT.json"))
            {
                MessageBox.Show("配置文件被删，正在自行恢复", "逆天友情提醒");
                Application.Exit();
            }
            return File.ReadAllText("App/DNT.json", Encoding.UTF8).JsonToModels<TempModel>();
        }

        /// <summary>
        /// 文件读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "打开文件。文件每行格式如下：苹果,4.98";
            dialog.Filter = "文本文件|*txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName;
                if (!string.IsNullOrEmpty(path))
                {
                    txtPath.Text = path;
                }
            }
            else
            {
                ShowLog("读取文件失败，请重新打开！");
            }
            BindTypInfos();
            shopMenuType.Enabled = true;
            shopType.Enabled = false;
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        private void BindTypInfos()
        {
            var list = ShopMenuHelper.GetTypeInfos(tempModel.ConnStr);
            if (list == null) { return; }
            shopMenuType.Items.Clear();
            foreach (var item in list)
            {
                shopMenuType.Items.Add(item.MType);
            }
        }

        /// <summary>
        /// 查询Shop
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="list"></param>
        private void BindShopInfos()
        {
            var list = ShopMenuHelper.GetShopInfos(tempModel.ConnStr);
            if (list == null) { return; }
            shopType.Items.Clear();
            foreach (var item in list)
            {
                shopType.Items.Add(item);
            }
        }

        /// <summary>
        /// 获取超市集合
        /// </summary>
        private void shopMenuType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShopInfos();
            shopType.Enabled = true;
        }

        /// <summary>
        /// 启用插入功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shopType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGo.Enabled = true;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确认批量插入数据？", "逆天友情提醒", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                var typeStr = shopMenuType.SelectedItem as string;
                var shopModel = shopType.SelectedItem as ShopModel;
                if (!string.IsNullOrEmpty(typeStr) && shopModel != null)
                {
                    ShopMenuModel model = new ShopMenuModel() { MType = typeStr, MShopId = shopModel.SId, MCityId = tempModel.CityId };
                    bool b = ShopMenuHelper.GetInsertCount(tempModel.ConnStr, path, model);
                    if (b) { ShowLog("批量插入成功！"); }
                    else { ShowLog("批量插入失败！"); }
                }
                else { ShowLog("请选择分类！"); }
            }
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="log"></param>
        /// <param name="b">是否是特殊的输出</param>
        private static void ShowLog(string log)
        {
            MessageBox.Show(log, "选择文件后记得选菜的分类和超市的分类哦~");
        }

        /// <summary>
        /// 使用说明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("文件文件每行的格式：菜名,价格", "逆天友情提醒");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="model"></param>
        private void label3_Click(object sender, EventArgs e)
        {
            File.Delete("App/DNT.json");
            MessageBox.Show("请重新打开程序", "逆天友情提醒");
            Application.Exit();
        }

        /// <summary>
        /// 应用退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}