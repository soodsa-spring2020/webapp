using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using csye6225.Common.Enums;
using Microsoft.AspNetCore.Http;

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

    public class UniqueCategoryValidationAttribute : ValidationAttribute  
    {  
        protected override ValidationResult IsValid(object aValue, ValidationContext validationContext)  
        {  
            List<string> collection = aValue as List<string>;
            if(collection.Select(s => s.Trim()).Distinct().Count() != collection.Select(s => s.Trim()).Count()){
                return new ValidationResult("All category must be unique for the bill.");
            }
            return ValidationResult.Success; 
        }  
  
    }

    public class BillDueDateValidationAttribute : ValidationAttribute  
    {  
        public string BillDate { get; set; }

        protected override ValidationResult IsValid(object aValue, ValidationContext validationContext)  
        {  
            var billDate = validationContext.ObjectType.GetProperty(BillDate);
            var billDateVal = (DateTime)billDate.GetValue(validationContext.ObjectInstance, null);
            var dueDateVal = (DateTime)aValue;

            if(dueDateVal < billDateVal){
                return new ValidationResult("Due date has to be greater than the bill date.");
            }
            return ValidationResult.Success; 
        }  
  
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FilePathExtensionsAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public FilePathExtensionsAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;
            if (file != null)
            {
                var fileName = file.FileName;
                return AllowedExtensions.Any(y => fileName.EndsWith(y));
            }
            return true;
        }
    }
}