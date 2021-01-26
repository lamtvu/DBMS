using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanXe
{
    public partial class Form1 : Form
    {
        string ConnectionString = "";
        SqlConnection conn = null;
        String authUser;
        bool login = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dgvXe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNguoiDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDaGiaoDich.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            btApply.Enabled = false;


        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            ConnectionString = "Network Library=dbmssocn;Network Address=" + textBox3.Text + "; Initial Catalog = DBQLXe;" + "User ID=" + textBox1.Text + ";Password=" + textBox2.Text + ";";
            MessageBox.Show(ConnectionString);
            try
            {
                conn = new SqlConnection(ConnectionString);
                dgvXe.DataSource = GetTableInDataBase("Select * from Xe");
                login = true;
                authUser = textBox1.Text;
                cbbHang.DataSource = GetTableInDataBase("select tenHang from Hang");
                cbbHang.ValueMember = "tenHang";
            }
            catch (Exception)
            {
                MessageBox.Show("Dang nhap that bai");
                login = false;
            }

        }
        private DataTable GetTableInDataBase(string sql)
        {

            //SqlConnection connection = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = null;
                DataTable data = null;
                sqlDataAdapter = new SqlDataAdapter(sql, connection);
                data = new DataTable();
                data.Clear();
                sqlDataAdapter.Fill(data);
                return data;
            }
        }
        private void runQuery(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = conn;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = sql;
                    sqlCommand.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void tbtn_change(object sender, EventArgs e)
        {
            if (login == false) return;
            RadioButton selectedbtn = null;
            foreach (RadioButton radioButton in pnRadioPage1.Controls)
            {
                if (radioButton.Checked == true)
                {
                    selectedbtn = radioButton;
                    break;
                }
            }
            switch (selectedbtn.Name)
            {

                case "rbtnTatCa":
                    dgvXe.DataSource = GetTableInDataBase("Select * from Xe");
                    break;
                case "rbtnGiamDan":
                    dgvXe.DataSource = GetTableInDataBase("exec sp_SortLonDenBe");
                    break;
                case "rbtnTangDan":
                    dgvXe.DataSource = GetTableInDataBase("exec sp_SortBeDenLon");
                    break;
                case "rbtnTrongKhoang":
                    dgvXe.DataSource = GetTableInDataBase("exec sp_XeTrongKhoan " + 0 + "," + 0);
                    break;
                case "rbtnBanChay":
                    dgvXe.DataSource = GetTableInDataBase("exec sp_XeBanChayToanHT");
                    break;
                default:
                    break;
            }
        }

        private void txbGiaTu_TextChanged(object sender, EventArgs e)
        {
            int tu, den;
            if (int.TryParse(txbGiaTu.Text, out tu) && int.TryParse(txbGiaDen.Text, out den))
            {
                dgvXe.DataSource = GetTableInDataBase("exec sp_XeTrongKhoan " + tu + "," + den);
            }
        }

        private void dgvXe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txbGia.Text = dgvXe.CurrentRow.Cells["gia"].Value.ToString();
            txbTen.Text = dgvXe.CurrentRow.Cells["tenXe"].Value.ToString();
            txbTonKho.Text = dgvXe.CurrentRow.Cells["SLTonKho"].Value.ToString();
            lbSoLuongBan.Text = dgvXe.CurrentRow.Cells["SLDaBan"].Value.ToString();
            try
            {
                lbSao.Text = GetTableInDataBase("select* from Fn_SaoTrungBinh(" + dgvXe.CurrentRow.Cells["ID"].Value + ")").Rows[0].Field<object>("DiemDanhGiaTrungBinh").ToString();
            }
            catch (Exception)
            {
                lbSao.Text = "";
            }
            cbbHang.Text = GetTableInDataBase("select* from Hang where ID=" + dgvXe.CurrentRow.Cells["IDhang"].Value).Rows[0].Field<object>("tenHang").ToString();
            //IDhang

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txbGia.Text = "";
            txbTen.Text = "";
            txbTonKho.Text = "";
            lbSao.Text = "";
            lbSoLuongBan.Text = "";
            btApply.Enabled = true;
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            try
            {
                int idHang = Convert.ToInt32(GetTableInDataBase("select* from Hang where tenHang=" + "'" + cbbHang.Text + "'").Rows[0].Field<object>("ID"));
                runQuery("insert into Xe(tenXe,gia,IDhang,SLDaBan,SLTonKho) values(" + "'" + txbTen.Text + "'" + "," + txbGia.Text + "," + idHang + "," + 0 + "," + txbTonKho.Text + ")");
                dgvXe.DataSource = GetTableInDataBase("Select * from Xe");

            }
            catch (Exception)
            {
                MessageBox.Show("Ban Khong Co Quyen");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int idHang = Convert.ToInt32(GetTableInDataBase("select* from Hang where tenHang=" + "'" + cbbHang.Text + "'").Rows[0].Field<object>("ID"));
                MessageBox.Show("update Xe set tenXe=+" + "'" + txbTen.Text + "'" + ",gia=" + txbGia.Text + ",IDhang=" + idHang + ",SLTonKho=" + txbTonKho.Text + " where ID=" + dgvXe.CurrentRow.Cells["ID"]);
                runQuery("update Xe set tenXe=+" + "'" + txbTen.Text + "'" + ",gia=" + txbGia.Text + ",IDhang=" + idHang + ",SLTonKho=" + txbTonKho.Text + " where ID=" + dgvXe.CurrentRow.Cells["ID"].Value);
                dgvXe.DataSource = GetTableInDataBase("Select * from Xe");
            }
            catch (Exception)
            {
                MessageBox.Show("Ban Khong Co Quyen");
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (authUser == null) return;
            try
            {
                switch (TabControl.SelectedIndex)
                {

                    case 0:
                        dgvXe.DataSource = GetTableInDataBase("Select * from Xe");
                        cbbHang.DataSource = GetTableInDataBase("select tenHang from Hang");
                        cbbHang.ValueMember = "tenHang";
                        break;
                    case 1:
                        dgvNguoiDung.DataSource = GetTableInDataBase("Select * from NguoiDung");
                        break;
                    case 2:
                        dgvHang.DataSource = GetTableInDataBase("Select* from Hang");
                        break;
                    case 3:
                        dgvDaGiaoDich.DataSource = GetTableInDataBase("Select* from DaGiaoDich");
                        break;
                    case 4:
                        dgvDanhGia.DataSource = GetTableInDataBase("Select* from DanhGia");
                        break;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền truy cập");
                TabControl.SelectedIndex = 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txbGia.Text = "";
            txbTen.Text = "";
            txbTonKho.Text = "";
            lbSoLuongBan.Text = "";
            lbSao.Text = "";
            btApply.Enabled = false;
        }

        private void dgvNguoiDung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbTenNguoiDung.Text = dgvNguoiDung.CurrentRow.Cells["ten"].Value.ToString();
            txbTaiKhoan.Text = dgvNguoiDung.CurrentRow.Cells["taiKhoan"].Value.ToString();
            cbbChucVu.Text = dgvNguoiDung.CurrentRow.Cells["ChucVu"].Value.ToString();
        }

        private void btnThemNguoiDung_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("create login " + txbTaiKhoan.Text + " with password = " + "'" + txbMatKhau.Text + "'" + ";");
                runQuery("create user " + txbTaiKhoan.Text + " for login " + txbTaiKhoan.Text + ";");
                MessageBox.Show("insert into NguoiDung(taiKhoan,Ten,ChucVu) values(" + "'" + txbTaiKhoan.Text + "'" + "," + "'" + tbTenNguoiDung.Text + "'" + "," + "'" + cbbChucVu.Text.Trim() + "'" + ")");
                runQuery("insert into NguoiDung(taiKhoan,ten,ChucVu) values(" + "'" + txbTaiKhoan.Text + "'" + "," + "'" + tbTenNguoiDung.Text + "'" + "," + "'" + cbbChucVu.Text.Trim() + "'" + ")");

            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Tai khoan da ton tai");
            }
            switch (cbbChucVu.Text.Trim())
            {
                case "admin":
                    runQuery("alter role admin_role add member[" + txbTaiKhoan.Text + "];");
                    break;
                case "quanly":
                    runQuery("alter role quanly_role add member[" + txbTaiKhoan.Text + "];");
                    break;
                default:
                    runQuery("alter role nguoidung_role add member[" + txbTaiKhoan.Text + "];");
                    break;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("insert into DanhGia(taiKhoan,IDxe,SoSao) values(" + "'" + authUser + "'" + "," + dgvXe.CurrentRow.Cells["ID"].Value + "," + numericUpDown1.Value + ")");
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("insert into DanhGia(taiKhoan,IDxe,NgayGiaDich) values(" + "'" + authUser + "'" + "," + dgvXe.CurrentRow.Cells["ID"].Value + "," + DateTime.Now + ")");
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");
            }
        }

        private void btnXoaNguoiDung_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("DROP user " + dgvNguoiDung.CurrentRow.Cells["taiKhoan"].Value);
                runQuery("DROP Login " + dgvNguoiDung.CurrentRow.Cells["taiKhoan"].Value);
                runQuery("DROP Login " + dgvNguoiDung.CurrentRow.Cells["taiKhoan"].Value);
                runQuery("Delete from NguoiDung where taiKhoan=" + dgvNguoiDung.CurrentRow.Cells["taiKhoan"].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("Delete from Xe where ID=" + dgvXe.CurrentRow.Cells["ID"].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("insert into Hang(tenHang) values(" + "'" + textBox6.Text + "'" + ")");
            }
            catch(Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("Delete from Hang where ID=" + dgvHang.CurrentRow.Cells["ID"].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            try
            {
                runQuery("Update Hang set tenHang="+"'"+textBox6.Text+"'"+"where ID="+dgvHang.CurrentRow.Cells["ID"]);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                runQuery("Delete from DaGiaoDich where taiKhoan=" + "'"+ dgvDaGiaoDich.CurrentRow.Cells["taiKhoan"].Value+"'"+" and IDxe="+dgvDaGiaoDich.CurrentRow.Cells["IDxe"].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

            try
            {
                runQuery("Delete from DanhGia where taiKhoan="+"'" + dgvDanhGia.CurrentRow.Cells["taiKhoan"].Value+"'"+" and IDxe="+dgvDanhGia.CurrentRow.Cells["IDxe"].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn Không Có Quyền");

            }
        }

        private void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox6.Text = dgvHang.CurrentRow.Cells["tenHang"].Value.ToString();
        }
    }
}
