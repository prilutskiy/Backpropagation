using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Forms
{
    public enum IrisClass
    {
        Setosa, Virginica, Versicolor
    }
    public class Iris
    {
        private static IrisClass GetIrisClass(String classString)
        {
            switch (classString)
            {
                case "Iris-setosa": return IrisClass.Setosa;
                case "Iris-verginica": return IrisClass.Virginica;
                case "Iris-versicolor": return IrisClass.Versicolor;
                default:
                    throw new InvalidOperationException();
            }
        }
        #region Static methods

        public static ICollection<Iris> GetIrisesFromFile(string path)
        {
            var irisList = new List<Iris>();
            String rawData = File.ReadAllText(path);
            var irisLines = rawData.Split('\n');
            foreach (var irisLine in irisLines)
            {
                if (irisLine.Length < 1) break;
                var irProps = irisLine.Split(',');
                var i = new Iris(GetIrisClass(irProps[4]),
                    Double.Parse(irProps[0], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[1], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[2], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[3], CultureInfo.InvariantCulture));
                irisList.Add(i);
            }
            return irisList;
        }
        #endregion
        /// <summary>
        /// Default ctor
        /// </summary>
        public Iris()
        {

        }
        /// <summary>
        /// Parametrized ctor
        /// </summary>
        /// <param name="type">Iris type</param>
        /// <param name="x">First coordinate</param>
        /// <param name="y">Second coordinate</param>
        /// <param name="z">Third coordinate</param>
        /// <param name="t">Fourth coordinate</param>
        public Iris(IrisClass type, double x, double y, double z, double t)
        {
            Class = type;
            X = x;
            Y = y;
            Z = z;
            T = t;
        }
        /// <summary>
        /// Class of an iris
        /// </summary>
        public IrisClass Class { get; set; }
        /// <summary>
        /// First coordinate
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Second coordinate
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Third coordinate
        /// </summary>
        public double Z { get; set; }
        /// <summary>
        /// Fourth coordinate
        /// </summary>
        public double T { get; set; }

    }
}
