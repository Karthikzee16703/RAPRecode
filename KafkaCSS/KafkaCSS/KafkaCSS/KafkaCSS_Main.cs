using System;
using System.Collections.Generic;
using KafkaNet;
using KafkaNet.Model;

namespace KafkaCSS
{
    public class KafkaCSS_Main

    {
        private Producer Client; 
        public  KafkaCSS_Main()
            {

            }

       public void Init(string url)
        {
            Uri uri = new Uri(url);
            var options = new KafkaOptions(uri);
            var router = new BrokerRouter(options);
             this.Client = new Producer(router);
        } 

        public void SendMessage(string CSSmessage, string topicName)
        {
            //Console.WriteLine(":::  : :In KafkaCSS SendMessage");
            KafkaNet.Protocol.Message msg = new KafkaNet.Protocol.Message(CSSmessage);
            this.Client.SendMessageAsync(topicName, new List<KafkaNet.Protocol.Message> { msg });
            //Console.WriteLine(":::  : :Completed KafkaCSS SendMessage");
        }

        public void Stop()
        {
            this.Client.Stop();
        }

    }
}

