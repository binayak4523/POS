using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static String RegistrationType { get; set; }
        public static String UserName {  get; set; }
        public static String UserRole { get; set; }
        public static String LanguageISOCode { get; set; }
        public static String CountryISOCode { get; set; }
        public static String CurrencyCode { get; set; }
    }
}
