namespace Infrastructure.Application.SMS.UsesCases.SendSMS
{

    public class SendSMSCommand
    {
            public string PhoneNumber {get; set;}

            public string Message {get; set;}

            public string SenderId {get; set;}

            public SmsTypes SmsTpe {get; set;}

    }

}