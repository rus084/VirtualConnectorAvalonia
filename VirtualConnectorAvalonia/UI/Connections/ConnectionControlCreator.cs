using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualConnectorAvalonia.Connections;
using VirtualConnectorAvalonia.Connections.Params;

namespace VirtualConnectorAvalonia.UI.Connections
{

    class ChanelParamsControl : Panel
    {
        const int StartVerticalPosition = 3;
        const int StartHorizontalPosition = 6;

        const int VerticalSpaceBetweenParam = 25;
        const int HorizontalSpaceBetweenCollums = 64;

        public string ConnectionType => connection.Name;

        public IControl control => this;

        public IConnection CreateConnection(IConnector network)
        {
            connection.SetCallbackHandler(network);

            IConnection result = connection;
            connection = connection.GetNewInstance();
            return result;
        }

        IConnection connection;

        Label GetParamLabel(string ParamName, int paramNumber)
        {
            Label label = new Label();
            label.Content = ParamName;
            label.Margin = new Avalonia.Thickness(StartHorizontalPosition, StartVerticalPosition + paramNumber * VerticalSpaceBetweenParam, 0, 0);
            label.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            label.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            return label;
        }

        void ParamControlSetDefaultParams(Control control, int paramNumber)
        {
            control.Margin = new Avalonia.Thickness(StartHorizontalPosition + HorizontalSpaceBetweenCollums, StartVerticalPosition + paramNumber * VerticalSpaceBetweenParam, 0, 0);
            control.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            control.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            control.Width = 120;
        }

        IControl GetParamControl(ConnectionParam param, int paramNumber)
        {
            if (param is StringParam)
            {
                TextBox textBox = new TextBox();
                ParamControlSetDefaultParams(textBox, paramNumber);
                textBox.Text = ((StringParam)param).DefaultValue;
                textBox.KeyUp += (object? sender, Avalonia.Input.KeyEventArgs e) =>
                {
                    if (!connection.SetParam(paramNumber, textBox.Text))
                        textBox.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    else
                        textBox.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                };

                connection.SetParam(paramNumber, ((StringParam)param).DefaultValue);
                return textBox;
            }
            else if (param is EnumParam)
            {
                ComboBox comboBox = new ComboBox();
                ParamControlSetDefaultParams(comboBox, paramNumber);
                comboBox.Items = ((EnumParam)param).getValidValues();
                comboBox.SelectedIndex = ((EnumParam)param).DefaultIndex;
                comboBox.SelectionChanged += (object? sender, SelectionChangedEventArgs e) =>
                {
                    connection.SetParam(paramNumber, comboBox.SelectedItem);
                };

                comboBox.Tapped += (object? sender, Avalonia.Interactivity.RoutedEventArgs e) =>
                {
                    comboBox.Items = ((EnumParam)param).getValidValues();
                };

                connection.SetParam(paramNumber, comboBox.SelectedItem);
                return comboBox;
            }
            else
                throw new Exception("Unknown param type");

        }


        void AddParam(ConnectionParam param)
        {
            int paramNumber = param.ParamNumber;
            IControl label = GetParamLabel(param.ParamName, paramNumber);
            IControl paramControl = GetParamControl(param, paramNumber);

            this.Children.Add(label);
            this.Children.Add(paramControl);
        }

        public ChanelParamsControl(IConnection connection)
        {
            this.connection = connection;
            IEnumerable<ConnectionParam> connectionParams = connection.GetParams();

            foreach (var param in connectionParams)
            {
                AddParam(param);
            }
        }
    }
}
