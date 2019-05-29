using System.Globalization;
using System.Windows.Controls;

namespace FinancialAnalysis.Logic.Rules
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
            {
                return new ValidationResult(false, error);
            }

            return ValidationResult.ValidResult;
        }
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
            {
                errorMessage = string.Format($"You cannot leave Field {fieldName} empty");
            }

            if (fieldValue == null || string.IsNullOrEmpty(fieldValue?.ToString()))
            {
                errorMessage = string.Format($"You cannot leave Field {fieldName} empty");
            }

            return errorMessage;
        }
    }
}
