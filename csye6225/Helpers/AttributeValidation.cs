using System;
using System.ComponentModel.DataAnnotations;
using csye6225.Common.Enums;

namespace csye6225.Helpers
{
    public class PaymentStatusEnumValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object aValue)
        {
            PaymentStatusEnum val;
            return Enum.TryParse(aValue.ToString(), true, out val);
        }
    }
}