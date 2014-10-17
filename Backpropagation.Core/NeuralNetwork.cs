using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents artificial neural network
    /// </summary>
    public class NeuralNetwork
    {
        #region Static members
        public static Double SigmoidalActivationFunction(Double[] inputValues, Double[] weights)
        {
            if (inputValues == null) throw new ArgumentException("Input values is not defined");
            var count = inputValues.Count();
            Double sum = 0;
            for (int i = 0; i < count; i++)
                sum += inputValues[i]*weights[i];
            var sigm = Math.Pow(Math.Pow(Math.E, -sum) + 1.0, -1);
            return sigm;
        }
        #endregion

        private int LayerCount;
        public NeuralNetwork(int inputCount, int outputCount, int layerCount = 2)
        {
            if (layerCount < 2) throw new ArgumentException("Layer count cannot be less than 2.");
            InputCount = inputCount;
            OutputCount = outputCount;
            LayerCount = layerCount;
        }

        public Func<Double[], Double[], Double> ActivationFunction { get; set; }
        public Int32 InputCount { get; private set; }
        public Int32 OutputCount { get; private set; }
        public ICollection<NeuralLayer> NeuralLayers { get; set; }

        private void InitNeuralLayers()
        {
            NeuralLayers = new List<NeuralLayer>();
            if (ActivationFunction == null) throw new NullReferenceException("Activation function is not defined");
            for (int i = 0; i < LayerCount; i ++)
            {
                var l = new NeuralLayer(InputCount, i == LayerCount-1 ? OutputCount : InputCount, ActivationFunction);
                NeuralLayers.Add(l);
            }
        }

        public Double[] GetNetworkOutput(INeuralImage image)
        {
            if (NeuralLayers == null) throw new NullReferenceException("Neural layers are not initialized");
            //input layer
            var inputLayer = GetInputNeuralLayer();
            inputLayer.InitInputValues(image.Values);
            var tempOutValues = inputLayer.GetLayerOutput();
            //all hidden layers
            for (int i = 1; i < NeuralLayers.Count() - 1; i ++)
            {
                var hiddenLayer = NeuralLayers.ToList()[i];
                hiddenLayer.InitInputValues(tempOutValues);
                tempOutValues = hiddenLayer.GetLayerOutput();
            }
            //output layer
            var outputLayer = GetOutputNeuralLayer();
            outputLayer.InitInputValues(tempOutValues);
            var resultValues = outputLayer.GetLayerOutput();

            return resultValues;
        }

        public Double[] RoundValues(Double[] values)
        {
            var rounded = new double[values.Count()];
            for (int i = 0; i < rounded.Count(); i++)
                rounded[i] = Math.Round(values[i]);
            return rounded;
        }
        
        public Double[] GetOuputRule(INeuralImage img)
        {
            var rule = new double[OutputCount];
            rule[img.ClassId] = 1.0;
            return rule;
        }
        public int TrainNetwork(ICollection<INeuralImage> imgs)
        {
            InitNeuralLayers();
            int iterations = 0;
            bool noErrors;
            do
            {
                noErrors = true;
                foreach (var img in imgs)
                {
                    var recResult = GetNetworkOutput(img);
                    var outputRule = GetOuputRule(img);
                    var roundedOutput = RoundValues(recResult);
                    var outputDeviation = new double[recResult.Count()];
                    if (!CheckResult(img, roundedOutput)) noErrors = false;
                    for (int i = 0; i < recResult.Count(); i++)
                        outputDeviation[i] = outputRule[i] - roundedOutput[i];
                    SetNeuronErrorSignals(outputDeviation);//вычислить коэф. ошибки для каждого нейрона каждого слоя (начиная с выходного слоя
                    AdjustNeuronsWeights(); //корректируем весовые коэф-ты начиная со входного слоя
                    iterations++;
                    ClearCurrentConsoleLine();
                    Console.Write(iterations);
                }
            } while (noErrors != true);
            return iterations;
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, currentLineCursor);
        }
        private void AdjustNeuronsWeights()
        {
            foreach (var neuralLayer in NeuralLayers)
                foreach (var neuron in neuralLayer.Neurons)
                {
                    neuron.AdjustWeights();
                    neuron.ErrorSignal = default(double);
                }
        }

        private void SetNeuronErrorSignals(double[] outputDeviation)
        {
            var outputLayer = GetOutputNeuralLayer();
            int x = 0;
            foreach (var neuron in outputLayer.Neurons)//устанавливаем коэф-ты ошибки нейронов в выходном слое, основываясь на полученных выходных значениях
                neuron.ErrorSignal = outputDeviation[x++];
            for (int nc = NeuralLayers.Count - 1; nc > 0; nc--)//проходим по всем слоям, начиная с выходного
            {
                var layer = NeuralLayers.ToList()[nc];//текущий слой
                var subLayer = NeuralLayers.ToList()[nc - 1]; // предыдущий слой
                for (int i = 0; i < layer.Neurons.Count; i++)//проходим по всем нейронам в выбранном слое
                {
                    for (int j = 0; j < subLayer.Neurons.Count; j++)//проходим по всем нейронам в предыдущем слое
                    {
                        var neuron = layer.Neurons.ToList()[i]; //нейрон в основном слое
                        var subNeuron = subLayer.Neurons.ToList()[j]; // нейрон в подслое
                        subNeuron.ErrorSignal += neuron.ErrorSignal*neuron.WeightFactors[j]; //устанавливаем коэф. ошибки нейрона в подслое, основываясь на коэф.-те ошибки нейрона в основном слое
                    }
                }
            }
        }


        public Boolean CheckResult(INeuralImage img, Double[] values)
        {
            bool result = true;
            for (int i = 0; i < values.Count(); i++)
            {
                if (i == img.ClassId)
                {
                    result &= (int)values[img.ClassId] == 1;
                }
                else
                {
                    result &= (int) values[i] == 0;
                }
            }
            return result;
        }
        public NeuralLayer GetInputNeuralLayer()
        {
            return NeuralLayers.First();
        }

        public NeuralLayer GetOutputNeuralLayer()
        {
            return NeuralLayers.Last();
        }
    }
}
