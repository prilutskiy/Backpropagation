using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents neuron
    /// </summary>
    public class Neuron
    {
        private Func<Double[], Double[], Double> ActivationFunction { get; set; }
        private Double[] WeightFactors { get; set; }
        private Double _output;
        private readonly int _inputCount;
        static MT19937Generator r = new Troschuetz.Random.Generators.MT19937Generator();
        private void InitWeightFactors()
        {
            for (int i = 0; i < WeightFactors.Count(); i++)
            {
                WeightFactors[i] = ((double) r.Next(-5, 5))/10.0;
            }
        }

        /// <summary>
        /// Default ctor for creating neuron instance
        /// </summary>
        /// <param name="inputCount">Count of neuon inputs</param>
        /// <param name="activationFunc">Neuron activation function</param>
        public Neuron(int inputCount, Func<Double[], Double[], Double> activationFunc)
        {
            _inputCount = inputCount;

            WeightFactors = new double[inputCount];
            ActivationFunction = activationFunc;
            InitWeightFactors();
        }

        public void InitInputValues(Double[] inputValues)
        {
            Inputs = inputValues;
        }
        public Double[] Inputs { get; private set; }

        virtual public Double Output
        {
            get 
            {
                _output = ActivationFunction(Inputs, WeightFactors);
                return _output;
            }
            set { _output = value; }
        }
    }

}
