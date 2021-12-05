using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ChristmasPresents
{
    public class EmailService
    {
        private SmtpClient client;
        private MailAddress fromAddress;
        private MailAddress toAddress;
        private string subject;

        public EmailService()
        {
            fromAddress = new MailAddress("sdv.adm.navidad@gmail.com", "From Name");
            toAddress = new MailAddress("sdv.adm.navidad@gmail.com", "To Name");
            const string fromPassword = "ASDqwe123!@#";
            subject = "Formaulario completado";

            client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
        }

        public void SendEmail(string presentGiverName, string presentGiverPhone, 
                              string presentGiverEmail, string presentGiverMethod,
                              string kidName, string kidArea, string presentGiverLetter, 
                              string presentCost, string presentShopName)
        {
            
            string body = $"Nombre y apellido: {presentGiverName} \nNumero de telefono: {presentGiverPhone} \nCorreo electronico: {presentGiverEmail} \nMetodo de pago: {presentGiverMethod} \nAhijado: {kidName} ({kidArea}) \nMensaje para mi ahijado/a: {presentGiverLetter} \nValor del regalo: ${presentCost} \nNegocio: {presentShopName} \n";


            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                client.Send(message);
            }
        }
    }
}
