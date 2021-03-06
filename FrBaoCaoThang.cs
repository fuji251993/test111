using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BUS;
using DTO;

namespace TangGiaoDien
{
    public partial class FrBaoCaoThang : Form
    {
        public FrBaoCaoThang()
        {
            InitializeComponent();
        } 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
        private void FrBaoCaoThang_Load(object sender, EventArgs e)
        {
            TaoBangThang();
            TaoBangNam();
            TaoBangLoaiTietKiem();            
        } 
        private void btthoat_Click(object sender, EventArgs e)
        {
            DialogResult t;
            t = MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == t)
            {
                this.Close();
            }
        }

        void TaoBangThang()
        {
            DataTable bang_chinh = new DataTable();
            bang_chinh.Columns.Add("Số tháng");
            bang_chinh.Columns.Add("Tên tháng");
            Object[] mang_thang = new Object[2] { "", "" };
            for (int i = 0; i < 12; i++)
            {
                int j = i + 1;
                mang_thang[0] = j;
                mang_thang[1] = "Tháng " + j;
                bang_chinh.Rows.Add(mang_thang);
            }
            lookUpEdit_Thang.Properties.DataSource = bang_chinh;
            lookUpEdit_Thang.Properties.DisplayMember = "Tên tháng";
            lookUpEdit_Thang.Properties.ValueMember = "Số tháng";
        }
        void TaoBangNam()
        {
            DataTable bang_chinh = new DataTable();
            bang_chinh.Columns.Add("Năm");
            bang_chinh.Columns.Add("Tên năm");
            Object[] mang_thang = new Object[2] { "", "" };
            int max_nam = 9998;
            int min_nam = 1752;
            for (int i = min_nam; i < max_nam; i++)
            {
                int j = i + 1;
                mang_thang[0] = j;
                mang_thang[1] = "Năm " + j;
                bang_chinh.Rows.Add(mang_thang);
            }
            lookUpEdit_nam.Properties.DataSource = bang_chinh;
            lookUpEdit_nam.Properties.DisplayMember = "Tên năm";
            lookUpEdit_nam.Properties.ValueMember = "Năm";
           // lookUpEdit_nam.SelectionStart = "2012";
        }
        void TaoBangLoaiTietKiem()
        {
            DataTable table = LoaiTietKiem_BUS.AllLoaiTietKiem();
            lookUpEdit_LoaiTietKiem.Properties.DataSource = table;
            lookUpEdit_LoaiTietKiem.Properties.DisplayMember = "TenLoaiTietKiem";
            lookUpEdit_LoaiTietKiem.Properties.ValueMember = "MaLoaiTietKiem"; 
        }
        public DataTable XuLy_SoDong(string ngay)
        {
            DataTable sodong = SoTietKiem_BUS.All_So_Dong(ngay);
            return sodong;
        }
        public void DanhSachSoTietKiem(string thang, string nam,string ma)
        {
            try
            {
                int temp = int.Parse(nam.ToString());
                string[] mangthang = new string[13] { "0", "31", "28", "31", "30", "31", "30", "31", " 31", " 30", "31", "30", "31" };
                if ((temp % 4 == 0 && temp % 100 != 0) || (temp % 400 == 0))
                {
                    mangthang = new string[13] { "0", "31", "28", "31", "30", "31", "30", "31", " 31", " 30", "31", "30", "31" };
                }
                DataTable bang_chinh = new DataTable();
                bang_chinh.Columns.Add("STT");
                bang_chinh.Columns.Add("Ngay");
                bang_chinh.Columns.Add("SoMo");
                bang_chinh.Columns.Add("SoDong");
                bang_chinh.Columns.Add("ChenhLech");
                int ngay = int.Parse(mangthang[int.Parse(thang.ToString())].ToString());
                string date = "";
                int mo = 0;
                int dong = 0;
                Object[] baocaothang = new Object[5] { "", "", "", "", "" };
                for (int i = 1; i <= ngay; i++)
                {
                    date = i.ToString() + "/" + thang + "/" + nam;
                    DataTable bang_somo = SoTietKiem_BUS.So_Mo(i.ToString(), thang, nam, ma);
                    DataTable bang_sodong = XuLy_SoDong(date);
                    mo = bang_somo.Rows.Count;
                    dong = bang_sodong.Rows.Count;
                    baocaothang[0] = i;
                    baocaothang[1] = date;
                    baocaothang[2] = mo;
                    baocaothang[3] = dong;
                    baocaothang[4] = mo - dong;
                    bang_chinh.Rows.Add(baocaothang);
                }
                gridControl_BaoCaoThang.DataSource = bang_chinh;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        public void BaoCaoThang()
        {
            try
            {

                int loi = 0;
                string chuoi = "";
                string thang;
                string nam;
                string ma;
                if (lookUpEdit_LoaiTietKiem.EditValue == null)
                {
                    loi++;
                    chuoi += loi + ". Xin chọn loại tiết kiệm \n";

                }
                if (lookUpEdit_Thang.EditValue == null)
                {
                    loi++;
                    chuoi += loi + ". Xin chọn tháng báo cáo \n";

                }
                if (lookUpEdit_nam.EditValue == null)
                {
                    loi++;
                    chuoi += loi + ". Xin chọn năm báo cáo \n";

                }
                if (loi > 0)
                {
                    MessageBox.Show(chuoi);
                }
                else
                {
                    thang = lookUpEdit_Thang.EditValue.ToString();
                    nam = lookUpEdit_nam.EditValue.ToString();
                    ma = lookUpEdit_LoaiTietKiem.EditValue.ToString();
                    DanhSachSoTietKiem(thang, nam, ma);
                    MessageBox.Show("Đã lập báo cáo tháng " + thang + " năm " + nam + " thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        public string ngay
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Ngay").ToString(); }

        }
        public string somo
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SoMo").ToString(); }

        }
        public string sodong
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SoDong").ToString(); }
        }
        public string chenhlech
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ChenhLech").ToString(); }
        } 
        private void btbaocao_Click(object sender, EventArgs e)
        {
            BaoCaoThang();
        } 
        public void Export_BaoCaoThang()
        {
            try
            {

                DialogResult t;
                t = MessageBox.Show("Bạn muốn export file này ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (DialogResult.OK == t)
                {
                    WCFDbService.ReportDB.ExportToXls("baocaothang", gridControl_BaoCaoThang);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Export_BaoCaoThang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            
        }

        private void FrBaoCaoThang_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BaoCaoThang();
                }
                else
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        DialogResult t;
                        t = MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (DialogResult.OK == t)
                        {
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btthoat_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DialogResult t;
                    t = MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (DialogResult.OK == t)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        } 
        private void simpleButton1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BaoCaoThang();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
           
            try
            {
                FrChiTietBaoCaoThang1 FormChiTietBaoCaoThang = new FrChiTietBaoCaoThang1();
                FormChiTietBaoCaoThang.ngay = ngay;
                FormChiTietBaoCaoThang.somo = somo;
                FormChiTietBaoCaoThang.sodong = sodong;
                FormChiTietBaoCaoThang.chechlech = chenhlech;
                FormChiTietBaoCaoThang.Show(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }           
        }
        public DataTable DanhSachSoTietKiem1(string thang, string nam, string ma)
        {
            //////////////
            DataTable bang_chinh = new DataTable();
            bang_chinh.Columns.Add("STT");
            bang_chinh.Columns.Add("Ngay");
            bang_chinh.Columns.Add("SoMo");
            bang_chinh.Columns.Add("SoDong");
            bang_chinh.Columns.Add("ChenhLech");
            /////////////////////
            DataTable bang = new DataTable();
            DataTable bang_ma = new DataTable();

            bang = SoTietKiem_BUS.SoTietKiem(thang, nam);
            bang_ma = SoTietKiem_BUS.BangLocNgay_TheoThang(thang, nam);
            Object[] mang_ruttien = new Object[5] { "", "", "", "", "" };
            for (int i = 0; i < bang_ma.Rows.Count; i++)
            { 
                mang_ruttien[0] = i + 1;
                string ngay = bang_ma.Rows[i][0].ToString();
                mang_ruttien[1] = ngay;
                int somo = 0;
                int sodong = 0;
                for (int j = 0; j < bang.Rows.Count; j++)
                {
                    string ma1 = bang.Rows[j]["LOAITIETKIEM.MaLoaiTietKiem"].ToString();
                    if (ma == ma1)
                    {
                        string ngay1 = bang.Rows[j]["NgayMoSo"].ToString();
                        bool tinhtrang;

                        if (ngay == ngay1)
                        {
                            tinhtrang = bool.Parse(bang.Rows[j]["TinhTrang"].ToString());
                            if (tinhtrang == true)
                            {
                                somo = somo + 1;
                            }
                            if (tinhtrang == false)
                            {
                                sodong = sodong + 1;
                            }

                        }
                    }
                }
                mang_ruttien[2] = somo;
                mang_ruttien[3] = sodong;
                mang_ruttien[4] = somo - sodong;
                bang_chinh.Rows.Add(mang_ruttien);
            }
            return bang_chinh;
        }
        public void BaoCaoThang1()
        {
            string thang;
            string nam;
            string ma;

            if (lookUpEdit_LoaiTietKiem.EditValue != null)
            {
                if (lookUpEdit_Thang.EditValue != null)
                {

                    if (lookUpEdit_nam.EditValue != null)
                    {
                        thang = lookUpEdit_Thang.EditValue.ToString();
                        nam = lookUpEdit_nam.EditValue.ToString();
                        ma = lookUpEdit_LoaiTietKiem.EditValue.ToString();
                        DataTable bang_chinh = new DataTable();
                        bang_chinh = DanhSachSoTietKiem1(thang, nam, ma);
                        if (bang_chinh.Rows.Count > 0)
                        {
                            gridControl_BaoCaoThang.DataSource = bang_chinh;
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }

                    }
                    else
                    {

                        MessageBox.Show("Bạn chưa chọn năm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn tháng ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn loại tiết kiệm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

        }
        private void simpleButton_locmatontai_Click(object sender, EventArgs e)
        {
            BaoCaoThang1();
        }
    }
}