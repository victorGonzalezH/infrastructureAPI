using System;

namespace Infrastructure.Application.SMS.UsesCases.SendSMS
{
    public class PhoneNumberException: Exception
    {
        public PhoneNumberException()
        {
            
        }

        public PhoneNumberException(string message): base(message)
        {
            
        }
        
    }

}