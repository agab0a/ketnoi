using KETNOI_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KETNOI_EF
{
    /// <summary>
    /// Interaction logic for ManHinh2.xaml
    /// </summary>
    public partial class ManHinh2 : Window
    {
        public ManHinh2()
        {
            InitializeComponent();
        }

        QlbenhNhanContext db = new QlbenhNhanContext();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //dgKhoa.ItemsSource = (from x in db.BenhNhans
            //                      join y in db.KhoaKhams
            //                      on x.Makhoa equals y.Makhoa
            //                      group new {x, y} by x.Makhoa into g
            //select new
            //{
            //    Makhoa = g.Key,
            //    Tenkhoa = g.FirstOrDefault().y.Tenkhoa,
            //    Tong = g.Sum(k => k.x.SongayNv * 60000)
            //}
            //                      ).ToList();

            dgKhoa.ItemsSource = db.BenhNhans.Join(db.KhoaKhams, x => x.Makhoa, y => y.Makhoa, (x, y) => new {x, y}).GroupBy(g => g.x.Makhoa).Select(g =>
                new
            {
                Makhoa = g.Key,
                Tenkhoa = g.FirstOrDefault().y.Tenkhoa,
                Tong = g.Sum(k => k.x.SongayNv * 60000)
            }
                                  ).ToList();
        }
    }
}
