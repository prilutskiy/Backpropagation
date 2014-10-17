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
            ICollection<Iris> irises = Iris.GetIrisesFromFile("iris.data");
            NeuralNetwork network = new NeuralNetwork(4, 3);
            network.ActivationFunction = NeuralNetwork.SigmoidalActivationFunction;
            network.InitNeuralLayers();
            var output = network.GetNetworkOutput(irises.First());
        }
    }
}
