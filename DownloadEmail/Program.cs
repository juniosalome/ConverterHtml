
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using SendGrid;


/*
using System;
using System.Collections;
using System.Text;
using MailKit.Net.Imap;
using MailBee;
using MailBee.ImapMail;
using MailBee.Mime;
*/
namespace DownloadEmail
{



    class Program
    {

        public class Constant {
            public const string ImapHost = "192.168.240.11";
            public const int ImapPort = 7143;
            public const string OAuth = "";
            public const string UserName = "junio@plannernet.com.br";
            public const string Password = "gYbZs53!";

        }

        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            var p = new Program();
            p.teste03();





        }

        public void teste00()
        {
            using (var client = new ImapClient())
            {
                client.Connect(Constant.ImapHost, Constant.ImapPort, SecureSocketOptions.None);

                try
                {
                    client.Authenticate(Constant.UserName, Constant.Password);
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine($"Erro ao autenticar usuario usuario");

                }

                client.Inbox.Open(FolderAccess.ReadWrite);
                IList<UniqueId> uids = client.Inbox.Search(SearchQuery.All);

                foreach (UniqueId uid in uids)
                {
                    MimeMessage message = client.Inbox.GetMessage(uid);
                    VerificarMensagem(message);

                }
            }
            return;
        }//Fim teste00


        public static void VerificarMensagem(MimeMessage message){
            foreach (MimeEntity attachment in message.Attachments)
            {
                var fileName = ($"{ DateTime.Now:yyyyMMddHHmmssss}-" + attachment.ContentDisposition?.FileName).Replace(".dat", ".eml") ?? ($"{ DateTime.Now:yyyyMMddHHmmssss}-" + attachment.ContentType.Name).Replace(".dat", ".eml");
                // Here you get the message class with attachments, visuals
                using (var stream = File.Create(fileName))
                {
                    if (attachment is MessagePart)
                    {
                        var rfc822 = (MessagePart)attachment;
                        rfc822.Message.WriteTo(stream);
                    }
                    else if (attachment is TnefPart)
                    {
                        MimeMessage msg = ((TnefPart)attachment).ConvertToMessage();
                        VerificarMensagem(msg);
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        part.Content.DecodeTo(stream);

                    }
                }
            }
        }

       
        public void teste02(string name) {

            var conversationId = Guid.NewGuid().ToString(); // TO DO: get the real conversation ID from dtaabase

            var testConversationEmailsToSend = 7;

            for (var i = 1; i <= testConversationEmailsToSend; i++)
            {
                var emailNumberForConversation = GetConversationEmailCount(i);
                var messageId = string.Format("{0}-{1}@yourdomain.com", conversationId, emailNumberForConversation);
                var previousMessageId = GetPreviousMessaageId(conversationId, emailNumberForConversation);

                var msg = new SendGridMessage();

                msg.Headers.Add("Message-ID", messageId);
                msg.Headers.Add("In-Reply-To", string.Format("<{0}>", previousMessageId));
                SetReferences(msg, conversationId, emailNumberForConversation);

                msg.AddTo("to@example.com");
                msg.From = new MailAddress("from@example.com");

                msg.Subject = "continuing the conversation";
                msg.Text = string.Format("{0} messaage #{1}", msg.Subject, i);

                var web = new Web(new NetworkCredential("sendgridusername", "sendgridpassword"));
                var task = web.DeliverAsync(msg);

                task.Wait(new TimeSpan(0, 0, 15));

                // sleep 1 minute before sending next email... for testing sample code
                Thread.Sleep(new TimeSpan(0, 0, 15));
            }

        }//Fim teste02
        public void teste03() {

            // To start a thread using a static thread procedure, use the
            // class name and method name when you create the ThreadStart
            // delegate. Beginning in version 2.0 of the .NET Framework,
            // it is not necessary to create a delegate explicitly. 
            // Specify the name of the method in the Thread constructor, 
            // and the compiler selects the correct delegate. For example:
            //
            // Thread newThread = new Thread(Work.DoWork);
            //
            ThreadStart threadDelegate = new ThreadStart(DoWork);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();

            // To start a thread using an instance method for the thread 
            // procedure, use the instance variable and method name when 
            // you create the ThreadStart delegate. Beginning in version
            // 2.0 of the .NET Framework, the explicit delegate is not
            // required.
            //
            
            Data = 42;
            threadDelegate = new ThreadStart(DoMoreWork);
            newThread = new Thread(threadDelegate);
            newThread.Start();


        }//Fim teste04
        public void teste05() { }//Fim teste05

        private static object GetPreviousMessaageId(string conversationId, int emailNumberForConversation)
        {
            var previousMessageCount = Math.Max(emailNumberForConversation - 1, 1);

            return string.Format("{0}-{1}@yourdomain.com", conversationId, previousMessageCount);
        }

        private static int GetConversationEmailCount(int dumbyParameterForSampleCode)
        {
            // TO DO: look in database to see how many of these messaages our system has sent.
            // if this is the first email for the conversation we'll return 1;

            return dumbyParameterForSampleCode; // hard setting value only for example code purposes
        }

        private static void SetReferences(SendGridMessage msg, string conversationId, int emailCount)
        {
            var referencesValue = "";

            for (var i = emailCount - 1; i > 0; i--)
            {
                referencesValue += string.Format("<{0}-{1}@yourdomain.com>{2}", conversationId, i, Environment.NewLine);
            }

            msg.Headers.Add("References", referencesValue);
        }
        public static void DoWork()
        {
            Console.WriteLine("Static thread procedure.");
        }
        public int Data;
        public void DoMoreWork()
        {
            Console.WriteLine("Instance thread procedure. Data={0}", Data);
        }


    }
}
