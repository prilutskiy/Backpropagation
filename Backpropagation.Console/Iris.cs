using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Backpropagation.Core;

namespace Backpropagation.Console
{
    public enum IrisClass
    {
        Setosa,     //0-0-1
        Virginica,  //0-1-0
        Versicolor, //1-0-0
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

        private static String GetClassNameById(IrisClass id)
        {
            switch (id)
            {
                case IrisClass.Setosa:
                    return "Iris-setosa";
                case IrisClass.Virginica:
                    return "Iris-virginica";
                case IrisClass.Versicolor:
                    return "Iris-versicolor";
                default:
                    return "Error getting class name";
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
        public static ICollection<INeuralImage> GetImagesFromFile(string path)
        {
            var irisList = new List<INeuralImage>();
            String rawData = "";
            if (File.Exists(path))
                rawData = File.ReadAllText(path);
            else
                rawData = GetDeafultSetFromResources();
            var irisLines = rawData.Split('\n');
            foreach (var irisLine in irisLines)
            {
                if (irisLine.Length < 1) break;
                var irProps = irisLine.Split(',');
                INeuralImage i = new Iris(GetIrisClass(irProps[4]), new[] {
                    Double.Parse(irProps[0], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[1], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[2], CultureInfo.InvariantCulture),
                    Double.Parse(irProps[3], CultureInfo.InvariantCulture)});
                irisList.Add(i);
            }
            return irisList;
        }

        private static string GetDeafultSetFromResources()
        {

            string rawData = Encoding.Default.GetString(Backpropagation.Console.Properties.Resources.iris);
            return rawData;
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

        public string ClassName
        {
            get { return GetClassNameById((IrisClass)ClassId); }
        }

        public double[] Values { get; set; }
    }

}
