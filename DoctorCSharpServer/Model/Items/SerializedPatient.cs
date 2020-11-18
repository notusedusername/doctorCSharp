
using DoctorCSharpServer.Controllers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Model.Items
{
    public class SerializedPatient
    {
        public string name { get; set; }

        public string TAJ_nr { get; set; }

        public string address { get; set; }
        public string phone { get; set; }

        public SerializedPatient()
        {
        }

        public void validate()
        {
            validateName();
            validateTAJ();
            validateAddress();
            validatePhone();
        }
        public void validate_taj()
        {
            validateTAJ();
        }

        private void validateName()
        {
            if(string.IsNullOrWhiteSpace(name)){
                throw new InvalidInputException("The value of " + nameof(name) + " can not be empty!");
            }
        }

        private void validateTAJ()
        {
            if (string.IsNullOrWhiteSpace(TAJ_nr))
            {
                throw new InvalidInputException("The value of " + nameof(TAJ_nr) + " can not be empty!");
            }
            else if (!new Regex("^\\d\\d\\d \\d\\d\\d \\d\\d\\d$").IsMatch(TAJ_nr))
            {
                throw new InvalidInputException("The format of the " + nameof(TAJ_nr) + " is not valid!");
            }
        }

        private void validateAddress()
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new InvalidInputException("The value of " + nameof(address) + " can not be empty!");
            }
        }

        private void validatePhone()
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new InvalidInputException("The value of " + nameof(phone) + " can not be empty!");
            }
        }
    }
}
