using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace CookieEdit2.Windows
{
    /// <summary>
    /// Interaction logic for SerialIOEditWindow.xaml
    /// </summary>
    public partial class SerialIOEditWindow : Window
    {
        public SerialIoSettings settingsObject
        {
            get
            {

                if (!Enum.TryParse(parity_cb.Text, out Parity p))
                    p = Parity.Even;

                if (!Enum.TryParse(stopbits_cb.Text, out StopBits sb))
                    sb = StopBits.One;

                if (!Enum.TryParse(handshake_cb.Text, out Handshake hs))
                    sb = StopBits.One;

                if (!int.TryParse(baud_cb.Text, out int bd))
                    bd = 2400;

                if (!int.TryParse(databits_cb.Text, out int db))
                    db = 7;

                return new SerialIoSettings(name_tb.Text, port_cb.Text, bd, p, db, sb, hs);
            }

            set
            {
                name_tb.Text = value.name;
                port_cb.Text = value.port;
                parity_cb.Text = value.parity.ToString();
                baud_cb.Text = value.baud.ToString();
                databits_cb.Text = value.dataBits.ToString();
                stopbits_cb.Text = value.stopBits.ToString();
                handshake_cb.Text = value.handshake.ToString();
            }
        }


        public SerialIOEditWindow()
        {
            InitializeComponent();

            port_cb.ItemsSource = SerialPort.GetPortNames();
            stopbits_cb.ItemsSource = Enum.GetNames(typeof(StopBits));
            parity_cb.ItemsSource = Enum.GetNames(typeof(Parity));
            handshake_cb.ItemsSource = Enum.GetNames(typeof(Handshake));

            for (int i = 1; i <= 8; i++)
                databits_cb.Items.Add(i.ToString());

            baud_cb.Items.Add(300);
            baud_cb.Items.Add(600);
            baud_cb.Items.Add(1200);
            baud_cb.Items.Add(2400);
            baud_cb.Items.Add(9600);
            baud_cb.Items.Add(14400);
            baud_cb.Items.Add(19200);
            baud_cb.Items.Add(38400);
            baud_cb.Items.Add(57600);
            baud_cb.Items.Add(115200);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            DialogResult = false;
            Hide();
        }

        private void port_tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
