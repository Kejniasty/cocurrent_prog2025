// Created by Lech Czochra
//using System.Globalization;
//using System.Windows.Controls;

//public class PositiveIntegerValidationRule : ValidationRule
//{
//    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
//    {
//        if (int.TryParse(value as string, out int result) && result > 0)
//        {
//            return ValidationResult.ValidResult;
//        }
//        return new ValidationResult(false, "Please enter a positive integer.");
//    }
//}
