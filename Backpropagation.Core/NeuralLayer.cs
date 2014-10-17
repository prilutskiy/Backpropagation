using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents artificial neural network layer
    /// </summary>
    /// <typeparam name="T">Type of input value</typeparam>
    public class NeuralLayer<T>
    {
        private readonly int _inputCount;
        private readonly int _outputCount;
        private T[] Inputs;
        public NeuralLayer(int inputCount, int outputCount, Func<T[], T> activationFunc, T[] inputs)
        {
            _inputCount = inputCount;
            _outputCount = outputCount;

            Inputs = inputs;
            GenerateNeurons(_outputCount, activationFunc, inputs);
        }
        private void GenerateNeurons(int outputCount, Func<T[], T> activationFunc, T[] inputs)
        {
            for (int i = 0; i < outputCount; i++)
            {
                var n = new Neuron<T>(outputCount, activationFunc, inputs);
                Neurons.Add(n);
            }
        }

        private ICollection<Neuron<T>> Neurons { get; set; }

        public T[] GetOutputs()
        {
            var arr = new T[_outputCount];
            int i = 0;
            foreach (var neuron in Neurons)
            {
                arr[i] = neuron.Output;
            }
            return arr;
        }
    }
}
