using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Backpropagation.Core;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting.Native;

namespace Backpropagation.WinForms
{
    public partial class IrisForm : Form
    {
        public IrisForm(ICollection<INeuralImage> images)
        {
            InitializeComponent();
            Data = images;
            InitializeChart(CalcImgCoordinatesByMultiplying);
        }

        private ICollection<INeuralImage> Data { get; set; }

        private double[] CalcImgCoordinatesByMultiplying(INeuralImage img)
        {
            var x_pos = img.Values.Take(img.Values.Length/2).Multiply();
            var y_pos = img.Values.Skip(img.Values.Length/2).Multiply();

            return new[]
            {
                x_pos,
                y_pos
            };
        }

        private void InitializeChart(Func<INeuralImage, double[]> calcAction)
        {
            var dataTables = CreateChartData();
            for (var i = 0; i < dataTables.Count; i++)
            {
                imageChart.Series[i].DataSource = dataTables[i];
                imageChart.Series[i].ArgumentScaleType = ScaleType.Numerical;
                imageChart.Series[i].ArgumentDataMember = "Sepal width*lentgth";
                imageChart.Series[i].ValueScaleType = ScaleType.Numerical;
                imageChart.Series[i].ValueDataMembers.AddRange("Petal width*lentgth");
            }
        }

        private List<DataTable> CreateChartData()
        {
            var headers = GetHeaders();
            imageChart.Series.Clear();

            var tables = new List<DataTable>();
            foreach (var header in headers)
            {
                tables.Add(new DataTable(header.ClassName));
                imageChart.Series.Add(header.ClassName, ViewType.Point);
            }
            foreach (var dataTable in tables)
            {
                dataTable.Columns.Add("Sepal width*lentgth", typeof (Double));
                dataTable.Columns.Add("Petal width*lentgth", typeof (Double));
            }
            foreach (var image in Data)
            {
                var curTable = tables[image.ClassId];
                var calcs = CalcImgCoordinatesByMultiplying(image);
                var row = curTable.NewRow();
                row["Sepal width*lentgth"] = calcs[0];
                row["Petal width*lentgth"] = calcs[1];
                curTable.Rows.Add(row);
            }
            return tables;
        }

        private List<INeuralImage> GetHeaders()
        {
            return (Data.OrderBy(o => o.ClassId)
                .ThenByDescending(o => o.ClassName)
                .GroupBy(o => o.ClassId)
                .Select(g => g.First())
                ).ToList();
        }
    }

    public static class DoubleHelper
    {
        public static double Multiply(this double[] arr)
        {
            double result = 1;
            arr.ForEach(d => result *= d);
            return result;
        }

        public static double Multiply(this IEnumerable<double> arr)
        {
            double result = 1;
            arr.ForEach(d => result *= d);
            return result;
        }
    }
}