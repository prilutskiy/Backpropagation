using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Backpropagation.Core;
using Backpropagation.Forms;

namespace Backpropagation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1;
            ICollection<INeuralImage> irises = Iris.GetImagesFromFile("iris.data");
            //выборка для обучения
            var trainIrises = new List<INeuralImage>();
            trainIrises.AddRange(irises.Where(i=>i.ClassId == 0).Take(count));
            trainIrises.AddRange(irises.Where(i => i.ClassId == 1).Take(count));
            trainIrises.AddRange(irises.Where(i => i.ClassId == 2).Take(count));
            
            //выборка для тестирования
            var testIrises = new List<INeuralImage>();
            testIrises.AddRange(irises.Where(i => i.ClassId == 0).Skip(count));
            testIrises.AddRange(irises.Where(i => i.ClassId == 1).Skip(count));
            testIrises.AddRange(irises.Where(i => i.ClassId == 2).Skip(count));

            var network = new NeuralNetwork(4, 3, NeuralNetwork.SigmoidalActivationFunction, 3);
            System.Console.WriteLine("ANN is being trained:");
            network.TrainNetwork(trainIrises, 50000, DisplayProgress);
            System.Console.WriteLine("");

            var recognAbility = 0;
            var generAbility = 0;
            foreach (var neuralImage in trainIrises)
            {
                var v = network.GetNetworkOutput(neuralImage);
                var result = network.GetClassIdOrDefault(neuralImage);
                recognAbility += result == neuralImage.ClassId ? 1 : 0;
            }
            foreach (var neuralImage in testIrises)
            {
                var v = network.GetNetworkOutput(neuralImage);
                var result = network.GetClassIdOrDefault(neuralImage);
                generAbility += result == neuralImage.ClassId ? 1 : 0;
            }
            System.Console.WriteLine("Распознающая способность: {0}%", GetPercentage(recognAbility, trainIrises.Count));
            System.Console.WriteLine("Обобщающая способность: {0}%", GetPercentage(generAbility, testIrises.Count));
            
            System.Console.ReadLine();
            XmlSerializer xser = new XmlSerializer(typeof(NeuralNetwork));
            var fs = new FileStream("ANN.xml", FileMode.Create);
            xser.Serialize(fs, network);
            fs.Close();

            System.Console.WriteLine("Data saved");
            System.Console.ReadLine();
        }

        public static Double GetPercentage(int count, int overall)
        {
            var percentage = (Math.Round((double)count / (double)overall, 4)) * 100;
            return percentage;
        }
        public static void DisplayProgress(int iterations)
        {
            System.Console.Write("\r{0}", iterations);
        }
        public static IrisClass GetClassByNetworkOutput(Double[] values)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                if ((int) values[i] == 1) return (IrisClass) i;
            }
            throw new FormatException("Bad network output");
        }
        
    }
}
