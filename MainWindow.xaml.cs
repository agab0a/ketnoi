using KETNOI_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace KETNOI_EF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

        }

        QlbenhNhanContext db = new QlbenhNhanContext();
        private void HienThi()
        {
            dgBenhNhan.ItemsSource = db.BenhNhans.Where(x => x.SongayNv <= 20).OrderByDescending(x => x.SongayNv).Select(x => new
            {
                x.Mabn,
                x.Hoten,
                x.Makhoa,
                x.Diachi,
                SoNgay = x.SongayNv,
                VienPhi = x.SongayNv * 60000
            }).ToList();
        }

        private void HienThiAll()
        {
            dgBenhNhan.ItemsSource = db.BenhNhans.Select(x => new
            {
                x.Mabn,
                x.Hoten,
                x.Makhoa,
                x.Diachi,
                SoNgay = x.SongayNv,
                VienPhi = x.SongayNv * 60000
            }).ToList();
        }
        private void HienThiCB()
        {
            cbbKhoaKham.ItemsSource = db.KhoaKhams.Select(x => x).ToList();
            cbbKhoaKham.DisplayMemberPath = "Tenkhoa";
            cbbKhoaKham.SelectedValuePath = "Makhoa";
            cbbKhoaKham.SelectedIndex = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi();
            HienThiCB();
        }

        private bool check()
        {
            if(txtMaBn.Text == "")
            {
                MessageBox.Show("Ma benh nhan khong dc de trong a!!!", "Thong Bao");
                return false;
            }else if (txtHoTen.Text == "")
            {
                MessageBox.Show("Ten benh nhan khong dc de trong a!!!", "Thong Bao");
                return false;
            }else if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Dia Chi benh nhan khong dc de trong a!!!", "Thong Bao");
                return false;
            }else if (txtSoNgayNhapVien.Text == "" || int.Parse(txtSoNgayNhapVien.Text) < 0 || !Regex.IsMatch(txtSoNgayNhapVien.Text, @"\d+"))
            {
                MessageBox.Show("O so ngay nhap vien bi sai a!!!", "Thong Bao");
                return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(db.BenhNhans.SingleOrDefault(x => x.Mabn == txtMaBn.Text) != null)
            {
                MessageBox.Show("Da co benh nhan day roi a!!!", "Thong bao");
            }
            else
            {
                try
                {
                    if (check())
                    {
                        BenhNhan bn = new BenhNhan();
                        bn.Mabn = txtMaBn.Text;
                        bn.Makhoa = ((KhoaKham)cbbKhoaKham.SelectedItem).Makhoa;
                        bn.SongayNv = int.Parse(txtSoNgayNhapVien.Text);
                        bn.Hoten = txtHoTen.Text;
                        bn.Diachi = txtDiaChi.Text;
                        db.BenhNhans.Add(bn);
                        db.SaveChanges();
                        HienThiAll();
                        MessageBox.Show("Da them thanh cong!!!");
                    }
                    else
                    {
                        MessageBox.Show("Du lieu nhap sai a!!!", "Thong bao");
                    }
                }catch(Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BenhNhan bn = db.BenhNhans.SingleOrDefault(x => x.Mabn == txtMaBn.Text);
            if (bn == null)
            {
                MessageBox.Show("Khong co benh nhan day a!!!", "Thong bao");
            }
            else
            {
                try
                {
                    if (check())
                    {
                        bn.Makhoa = ((KhoaKham)cbbKhoaKham.SelectedItem).Makhoa;
                        bn.SongayNv = int.Parse(txtSoNgayNhapVien.Text);
                        bn.Hoten = txtHoTen.Text;
                        bn.Diachi = txtDiaChi.Text;
                        db.SaveChanges();
                        HienThiAll();
                        MessageBox.Show("Da Sua thanh cong!!!");
                    }
                    else
                    {
                        MessageBox.Show("Du lieu nhap sai a!!!", "Thong bao");
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BenhNhan bn = db.BenhNhans.SingleOrDefault(x => x.Mabn == txtMaBn.Text);
            if (bn == null)
            {
                MessageBox.Show("Khong co benh nhan day de xoa a!!!", "Thong bao");
                return;
            }
            else
            {
                MessageBoxResult rs = MessageBox.Show("Co xac nhan xoa khong a!!!", "Thong bao xac nhan", MessageBoxButton.YesNo);
                if(rs == MessageBoxResult.Yes)
                {
                    db.BenhNhans.Remove(bn);
                    db.SaveChanges() ;
                    MessageBox.Show("Da xoa thanh cong a!!!");
                    HienThiAll();
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ManHinh2 x =  new ManHinh2();
            x.Show();
        }

        private void dgBenhNhan_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Type t = dgBenhNhan.SelectedItem.GetType();
            PropertyInfo[] p = t.GetProperties();
            txtMaBn.Text = p[0].GetValue(dgBenhNhan.SelectedValue).ToString();
            cbbKhoaKham.SelectedValue = p[2].GetValue(dgBenhNhan.SelectedValue).ToString();
            txtSoNgayNhapVien.Text = p[4].GetValue(dgBenhNhan.SelectedValue).ToString();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            dgBenhNhan.ItemsSource = db.BenhNhans.Where(x => x.Mabn == txtMaBn.Text).Select(x => new
            {
                x.Mabn,
                x.Hoten,
                x.Makhoa,
                x.Diachi,
                SoNgay = x.SongayNv,
                VienPhi = x.SongayNv * 60000
            }).ToList();
            if(db.BenhNhans.Count(x => x.Mabn == txtMaBn.Text) == 0)
            {
                MessageBox.Show("Khong tim thay a!!!");
            }
        }
    }
}
