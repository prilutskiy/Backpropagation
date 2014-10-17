using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backpropagation.Core;
using Backpropagation.Forms;

namespace Backpropagation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 2;
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

            var network = new NeuralNetwork(4, 3, 2);
            network.ActivationFunction = NeuralNetwork.SigmoidalActivationFunction;
            System.Console.WriteLine("ANN is being trained:");
            network.TrainNetwork(trainIrises);

            System.Console.ReadLine();
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
