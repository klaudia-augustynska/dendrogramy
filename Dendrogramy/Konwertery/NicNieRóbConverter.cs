﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Dendrogramy.Konwertery
{
    /// <summary>
    /// XAML przy użyciu multibindingu wymaga konwertera nawet wtedy, kiedy nie chcę przekształcać danych.
    /// </summary>
    public class NicNieRóbConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
