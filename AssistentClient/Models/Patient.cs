using AssistentClient.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace AssistentClient.Models
{
    public class Patient
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string taj { get; set; }
        public string phone { get; set; }

        public Patient(string name)
        {
            this.name = name;
        }
        public Patient(int id,string name, string address, string taj,string phone)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.taj = taj;
            this.phone = phone;
        }
        public Patient()
        {
        }

        public override string ToString()
        {
            return name.ToString();
        }


        public void validateName()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidInputException("The value of " + nameof(name) + " can not be empty!");
                
            }
        }

        public void validateTAJ()
        {
            if (string.IsNullOrWhiteSpace(taj))
            {
                throw new InvalidInputException("The value of " + nameof(taj) + " can not be empty!");
            }
            else if (!new Regex("^\\d\\d\\d \\d\\d\\d \\d\\d\\d$").IsMatch(taj))
            {
                throw new InvalidInputException("The format of the " + nameof(taj) + " is not valid!");
            }
        }

        public void validateAddress()
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new InvalidInputException("The value of " + nameof(address) + " can not be empty!");
            }
        }

        public void validatePhone()
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new InvalidInputException("The value of " + nameof(phone) + " can not be empty!");
            }
        }
        public void validateEmpty()
        {
            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(taj))
            {
                throw new InvalidInputException("All fields are required!");
            }
        }
    }
}
