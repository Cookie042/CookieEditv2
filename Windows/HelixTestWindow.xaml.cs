﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookieEdit2.Windows
{
    /// <summary>
    /// Interaction logic for HelixTestWindow.xaml
    /// </summary>
    public partial class HelixTestWindow : Window
    {
        public HelixTestWindow(DxViewModel conext)
        {
            InitializeComponent();
            Viewport3Dx.DataContext = conext;

        }
    }
}
