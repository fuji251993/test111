using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BUS;
using DTO;
using System.IO;
using DevExpress.XtraGrid;

namespace TangGiaoDien
{
    public partial class FrBaoCaoNgay : Form
    {
        public FrBaoCaoNgay()
        {
            InitializeComponent();
        }
      
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        } 
        public DataTable Xuly3(DataTable table_form)
        {

             DataTable bang_form2 = new DataTable();
             bang_form2 = table_form;
            //bang loai tat ca
            DataTable bang_LoaiTietKiem = new DataTable();
            bang_LoaiTietKiem = Table_All_LoaiTietKiem();
            Object[] mang_A = new Object[6] { "", "", "", "", "", "" };
            DataTable table_form1 = new DataTable();
            table_form1.Columns.Add("STT");
            table_form1.Columns.Add("LoaiTietKiem");
            table_form1.Columns.Add("TongThu");
            table_form1.Columns.Add("TongChi");
            table_form1.Columns.Add("ChenhLech");
            table_form1.Columns.Add("MaLoaiTietKiem");
            ///////////////////////////////////////////////////////////
            for (int i = 0; i < bang_LoaiTietKiem.Rows.Count; i++)
            {
                mang_A[0] = i + 1;
                string ma_bang_LoaiTietKiem = bang_LoaiTietKiem.Rows[i]["MaLoaiTietKiem"].ToString();
                mang_A[1] = bang_LoaiTietKiem.Rows[i]["TenLoaiTietKiem"].ToString();
                int tongthu = 0;
                int tongchi = 0;
                int chenhlech = 0;
                for (int j = 0; j < bang_form2.Rows.Count; j++)
                {
                    string ma_bang_LoaiTietKiem_1 = bang_form2.Rows[j]["LoaiTietKiem"].ToString();

                    if (ma_bang_LoaiTietKiem == ma_bang_LoaiTietKiem_1)
                    {
                        tongthu = tongthu + int.Parse(bang_form2.Rows[j]["TongThu"].ToString());
                        tongchi = tongchi + int.Parse(bang_form2.Rows[j]["TongChi"].ToString());
                        chenhlech = chenhlech + int.Parse(bang_form2.Rows[j]["ChenhLech"].ToString());
                    }
                }
                mang_A[2] = tongthu;
                mang_A[3] = tongchi;
                mang_A[4] = chenhlech;
                mang_A[5] = bang_LoaiTietKiem.Rows[i]["MaLoaiTietKiem"].ToString();
                table_form1.Rows.Add(mang_A);
            }
            return table_form1;
        }
        public DataTable XuLy(DataTable bang_rut, DataTable bang_goi)
        {
            //tao bang du lieu
            DataTable table_form = new DataTable();
            table_form.Columns.Add("STT");
            table_form.Columns.Add("LoaiTietKiem");
            table_form.Columns.Add("TongThu");
            table_form.Columns.Add("TongChi");
            table_form.Columns.Add("ChenhLech");
            table_form.Columns.Add("MaLoaiTietKiem");
            string[] mang_chung1 = new string[6] { "", "", "", "", "","" };
            DataTable bang_ltk = Table_All_LoaiTietKiem();
            long tong = 0;
            for (int i = 0; i < bang_ltk.Rows.Count; i++)
            {
                mang_chung1[0] = i.ToString();
                mang_chung1[1] = bang_ltk.Rows[i]["TenLoaiTietKiem"].ToString();
                mang_chung1[2] = bang_goi.Rows[i]["TongThu"].ToString();
                mang_chung1[3] = bang_rut.Rows[i]["TongChi"].ToString();
                tong = long.Parse(bang_goi.Rows[i]["TongThu"].ToString()) - long.Parse(bang_rut.Rows[i]["TongChi"].ToString());
                mang_chung1[4] = tong.ToString();
                mang_chung1[5] = bang_ltk.Rows[i]["MaLoaiTietKiem"].ToString();
                table_form.Rows.Add(mang_chung1);
            }
            return table_form;  
        }

        public DataTable Table_RutTien(string ngay, string thang, string nam)
        {
            int maloai = 0;
            long tongtien = 0;
            DataTable bang_ltk = Table_All_LoaiTietKiem();
            DataTable table_BangChung = new DataTable();
            table_BangChung.Columns.Add("TenLoaiTietKiem");
            table_BangChung.Columns.Add("TongChi");
            string[] mang_chung1 = new string[2] { "", "" };
            for (int i = 0; i < bang_ltk.Rows.Count; i++)
            {
                maloai = int.Parse(bang_ltk.Rows[i]["MaLoaiTietKiem"].ToString());
                DataTable table_goi = PhieuRutTien_BUS.danhsachphieuruttien_ngay_locmaso(maloai, ngay, thang, nam);
                string ma = bang_ltk.Rows[i]["TenLoaiTietKiem"].ToString();
                mang_chung1[0] = ma;
                if (table_goi.Rows[0]["Tong"].ToString() != "")
                {
                    tongtien = long.Parse(table_goi.Rows[0]["Tong"].ToString());
                }
                else
                {
                    tongtien = 0;
                }
                mang_chung1[1] = tongtien.ToString();
                table_BangChung.Rows.Add(mang_chung1);
            }
            return table_BangChung;
        }  
        public DataTable Table_LoaiTietKiem(string a)
        {
            string ma = Table_SoTietKiem(a);

            DataTable ten = new DataTable();
            if (ma != "-1")
            {
                ten = LoaiTietKiem_BUS.LayTenLoaiTietKiem1(ma);
                return ten;
            }
            return ten;
            
        }
        public string Table_SoTietKiem(string a)
        {
            DataTable sotiekiem = new DataTable();
            sotiekiem = SoTietKiem_BUS.SoTietKiem(a);
            if (sotiekiem.Rows.Count > 0)
            {
                return sotiekiem.Rows[0]["MaLoaiTietKiem"].ToString();
            } 
            return "-1";
        }
        public DataTable Table_GoiTien(string ngay, string thang, string nam)
        {
            int maloai = 0;
            long tongtien = 0;
            DataTable bang_ltk = Table_All_LoaiTietKiem();
            DataTable table_BangChung = new DataTable();
            table_BangChung.Columns.Add("TenLoaiTietKiem");
            table_BangChung.Columns.Add("TongThu");
            string[] mang_chung1 = new string[2] { "", "" };
            for (int i = 0; i < bang_ltk.Rows.Count; i++)
            {
                maloai = int.Parse(bang_ltk.Rows[i]["MaLoaiTietKiem"].ToString());
                DataTable table_goi = PhieuGoiTien_BUS.danhsachphieugoitien_ngay_locmaso(maloai, ngay, thang, nam);
                string ma = bang_ltk.Rows[i]["TenLoaiTietKiem"].ToString();
                mang_chung1[0] = ma;
                if (table_goi.Rows[0]["Tong"].ToString() != "")
                {
                    tongtien = long.Parse(table_goi.Rows[0]["Tong"].ToString());
                }
                else
                {
                    tongtien = 0;
                }
                mang_chung1[1] = tongtien.ToString();
                table_BangChung.Rows.Add(mang_chung1);
            }
            return table_BangChung;
        } 

        public DataTable Table_All_LoaiTietKiem()
        {

            DataTable ten = new DataTable();
            ten = LoaiTietKiem_BUS.AllLoaiTietKiem();
            return ten;

        } 
        public void BaoCaoNgay()
        {
            int ngay = dateTimebaocaongay.Value.Day;
            int thang = dateTimebaocaongay.Value.Month;
            int nam = dateTimebaocaongay.Value.Year;
            string a = ngay + "/" + thang + "/" + nam;
            DataTable bang_goi = new DataTable();
            bang_goi = Table_GoiTien(ngay.ToString(), thang.ToString(), nam.ToString());
            DataTable bang_rut = new DataTable();
            bang_rut = Table_RutTien(ngay.ToString(), thang.ToString(), nam.ToString());
            DataTable table_form1 = new DataTable();
            table_form1 = XuLy(bang_rut, bang_goi);
            if (table_form1.Rows.Count > 0)
            {
                gridControl_baocaongay.DataSource = table_form1;                 
                MessageBox.Show("Đã lập báo cáo " + a + " thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không có dữ liệu ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }
        private void btbaocao_Click(object sender, EventArgs e)
        {
            BaoCaoNgay();
           
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
        public void ExportExcel()
        {
            DialogResult t;
            t = MessageBox.Show("Bạn muốn export file này ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == t)
            {
                WCFDbService.ReportDB.ExportToXls("baocaongay", gridControl_baocaongay);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ExportExcel();
        } 
       
        private void FrBaoCaoNgay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BaoCaoNgay();
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
        private void btbaocao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BaoCaoNgay();
            }
        }

        private void btthoat_KeyDown(object sender, KeyEventArgs e)
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
        public string MaLoaiTietKiem
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaLoaiTietKiem").ToString(); }
          
        }
        public string LoaiTietKiem
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "LoaiTietKiem").ToString(); } 
        }
        public string TongThu
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TongThu").ToString(); } 
        } 
        public string TongChi
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TongChi").ToString(); }           
        } 
        public string ChenhLech
        {
            get { return gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ChenhLech").ToString(); } 
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
                FrChiTietBaoCaoNgay FormChiTietBaoCaoNgay = new FrChiTietBaoCaoNgay(); 
                FormChiTietBaoCaoNgay.MaLoaiTietKiem = MaLoaiTietKiem;
                FormChiTietBaoCaoNgay.LoaiTietKiem = LoaiTietKiem;
                FormChiTietBaoCaoNgay.TongThu = TongThu;
                FormChiTietBaoCaoNgay.TongChi = TongChi;
                FormChiTietBaoCaoNgay.ChenhLech = ChenhLech;
                FormChiTietBaoCaoNgay.Show(); 
        } 
        private void FrBaoCaoNgay_Load(object sender, EventArgs e)
        {

        } 
    }
}