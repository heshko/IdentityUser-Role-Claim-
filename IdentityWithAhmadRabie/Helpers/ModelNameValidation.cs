using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Helpers
{
    public class ModelNameValidation :ValidationAttribute
    {
        private string _modelName;

        public ModelNameValidation(string modelName)
        {
            _modelName = modelName;
        }
        public override bool IsValid(object value)
        {
            string[] vls = value.ToString().Split('@');
         return   vls[1].ToUpper() == _modelName.ToUpper();
        }
    }
}
