using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Backpropagation.Core;
using Backpropagation.WinForms;

namespace Backpropagation.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var count = 20;
            var irises = Iris.GetImagesFromFile("iris.data");

            ShowImagesDistribution(irises); //Warning: Thread-blocking method

            //выборка для обучения
            var trainIrises = new List<INeuralImage>();
            trainIrises.AddRange(irises.Where(i => i.ClassId == 0).Take(count));
            trainIrises.AddRange(irises.Where(i => i.ClassId == 1).Take(count));
            trainIrises.AddRange(irises.Where(i => i.ClassId == 2).Take(count));

            //выборка для тестирования
            var testIrises = new List<INeuralImage>();
            testIrises.AddRange(irises.Where(i => i.ClassId == 0).Skip(count));
            testIrises.AddRange(irises.Where(i => i.ClassId == 1).Skip(count));
            testIrises.AddRange(irises.Where(i => i.ClassId == 2).Skip(count));

            var network = new NeuralNetwork(4, 3, NeuralNetwork.SigmoidalActivationFunction);
            System.Console.WriteLine("ANN is being trained:");
            network.TrainNetwork(trainIrises, 50000, DisplayProgress);
            System.Console.WriteLine("");

            var recognAbility = 0;
            var generAbility = 0;
            var startTime = DateTime.Now;
            foreach (var neuralImage in trainIrises)
            {
                var v = network.GetNetworkOutput(neuralImage);
                var result = network.GetClassIdOrDefault(neuralImage);
                recognAbility += result == neuralImage.ClassId ? 1 : 0;
            }
            var trainTime = DateTime.Now - startTime;
            System.Console.WriteLine("Time elapsed: {0}", trainTime);
            foreach (var neuralImage in testIrises)
            {
                var v = network.GetNetworkOutput(neuralImage);
                var result = network.GetClassIdOrDefault(neuralImage);
                generAbility += result == neuralImage.ClassId ? 1 : 0;
            }
            System.Console.WriteLine("Распознающая способность: {0}%", GetPercentage(recognAbility, trainIrises.Count));
            System.Console.WriteLine("Обобщающая способность: {0}%", GetPercentage(generAbility, testIrises.Count));

            System.Console.ReadLine();
        }

        private static void ShowImagesDistribution(ICollection<INeuralImage> irises)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Создание главного окна и его запуск
            Application.Run(new IrisForm(irises));
        }

        public static Double GetPercentage(int count, int overall)
        {
            var percentage = (Math.Round(count/(double) overall, 4))*100;
            return percentage;
        }

        public static void DisplayProgress(int iterations)
        {
            System.Console.Write("\r{0}", iterations);
        }

        public static IrisClass GetClassByNetworkOutput(Double[] values)
        {
            for (var i = 0; i < values.Count(); i++)
            {
                if ((int) values[i] == 1) return (IrisClass) i;
            }
            throw new FormatException("Bad network output");
        }
    }
}