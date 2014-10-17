using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backpropagation.Core;

namespace Backpropagation.Forms
{
    public enum IrisClass
    {
        Setosa, Virginica, Versicolor
    }
    public class Iris : INeuralImage
    {
        #region Static methods
        private static IrisClass GetIrisClass(String classString)
        {
            switch (classString)
            {
                case "Iris-setosa": return IrisClass.Setosa;
                case "Iris-virginica": return IrisClass.Virginica;
                case "Iris-versicolor": return IrisClass.Versicolor;
                default:
                    throw new InvalidOperationException();
            }
        }
        public static ICollection<Iris> GetIrisesFromFile(string path)
        {
            var irisList = new List<Iris>();
            String rawData = File.ReadAllText(path);
            var irisLines = rawData.Split('\n');
            foreach (var irisLine in irisLines)
            {
                if (irisLine.Length < 1) break;
                var irProps = irisLine.Split(',');
                var i = new Iris(GetIrisClass(irProps[4]), new[] {
                    Double.Parse(irProps[0], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[1], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[2], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[3], CultureInfo.InvariantCulture)});
                irisList.Add(i);
            }
            return irisList;
        }
        
        #endregion

        public Iris(IrisClass _class, double[] values)
        {
            Class = _class;
            Values = values;
        }
        public IrisClass GetClass()
        {
            return (IrisClass) Class;
        }

        private IrisClass _irisClass;
        public IrisClass Class
        {
            get { return _irisClass; }

            set { ClassId = (int) value;
                _irisClass = value;
            }
        }
        public int ClassId { get; set; }
        public double[] Values { get; set; }
    }

}
