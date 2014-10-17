using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents neuron
    /// </summary>
    /// <typeparam name="T">Type of input value</typeparam>
    public class Neuron<T>
    {
        private Func<T[], T> ActivationFunction { get; set; }
        private Double[] WeightFactors { get; set; }
        private T _output;
        private readonly int _inputCount;
        private void InitWeightFactors()
        {
            var r = new Random();
            for (int i = 0; i < WeightFactors.Count(); i++)
                WeightFactors[i] = ((double) r.Next(-5, 5))/10.0;
        }

        /// <summary>
        /// Default ctor for creating neuron instance
        /// </summary>
        /// <param name="inputCount">Count of neuon inputs</param>
        /// <param name="activationFunc">Neuron activation function</param>
        /// <param name="inputs">Neuron inputs</param>
        public Neuron(int inputCount, Func<T[],T> activationFunc, T[] inputs)
        {
            _inputCount = inputCount;

            Inputs = inputs;
            WeightFactors = new double[inputCount];
            ActivationFunction = activationFunc;
            InitWeightFactors();
        }

        public T[] Inputs { get; private set; }

        virtual public T Output
        {
            get 
            {
                _output = ActivationFunction(Inputs);
                return _output;
            }
            set { _output = value; }
        }
    }

}
