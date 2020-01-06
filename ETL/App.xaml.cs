// ***********************************************************************
// Assembly         : ETL
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 12-30-2019
// ***********************************************************************
// <copyright file="App.xaml.cs" company="">
//     Copyright ©  2019 Mariusz Owczarski
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ETL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
