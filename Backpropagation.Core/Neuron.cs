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
        private int _inputCount;
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
        public Neuron(int inputCount, Func<T[],T> activationFunc)
        {
            _inputCount = inputCount;
            Inputs = new T[inputCount];
            WeightFactors = new double[inputCount];
            ActivationFunction = activationFunc;
            InitWeightFactors();
        }

        public void InitializeInputs(T[] inputArray)
        {
            for (int i = 0; i < inputArray.Count(); i ++)
                this.Inputs[i] = inputArray[i];
        }
        public T[] Inputs { get; private set; }

        virtual public T Output
        {
            get 
            {
                if (Inputs.Count()!= _inputCount) 
                    throw new ArgumentOutOfRangeException("Input count does not match the value specified");
                _output = ActivationFunction(Inputs);
                return _output;
            }
            set { _output = value; }
        }
    }

}
