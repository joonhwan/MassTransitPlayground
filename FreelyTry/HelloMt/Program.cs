using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace HelloMt
{
    // - message in MassTransit is interface !
    // - The properties on the interface are defined with getters only
    //   --> immutable!
    public interface SubmitOrder : CorrelatedBy<Guid>
    {
        DateTime SubmitDate { get; }
        string CustomerNumber { get; }
        string OrderNumber { get; }
    }

    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        public Task Consume(ConsumeContext<SubmitOrder> context)
        {
            var message = context.Message;
            var order = string.Format("{0}/{1}/{2}",
                message.SubmitDate,
                message.CustomerNumber,
                message.OrderNumber
                );
            return Task.Run(() =>
            {
                Console.WriteLine("Processing: {0}", order);
                Thread.Sleep(1000);
                Console.WriteLine("Done.");
            });
        }
    }

    public class OrderService
    {
        
    }

    class Program
    {
        private static SubmitOrderConsumer _sharedConsumer;
        private static SubmitOrderObserver _shareConsumerObserver;

        static void Main(string[] args)
        {
            _sharedConsumer = new SubmitOrderConsumer();
            _shareConsumerObserver = new SubmitOrderObserver();
            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri("rabbitmq://localhost"), hostConfigurator =>
                {
                    hostConfigurator.Username("admin");
                    hostConfigurator.Password("admin");
                });
                configurator.UseRetry(Retry.Immediate(5));

                // endpoint 추가방법..(동일이름의 Queue로는 오직 1개만 추가된다)

                // 1번 방법) 매번 consumer가 생성된 다음, 처리하고 파기되는 모델.
                configurator.ReceiveEndpoint(host, "input_queue", endpointConfigurator =>
                {
                    endpointConfigurator.Consumer<SubmitOrderConsumer>();
                });

                // 2번 방법) 위 1번방법과 유사하지만, inline으로 handler를 구성하는 경우.
                //   (아래 예는 SubmitOrderConsumer를 재사용하였으나, 실제로는 SubmitOrderConsumer.Consume()이 구현한 코드만 넣어도 됨)
                //configurator.ReceiveEndpoint(host, "input_queue", endpointConfigurator =>
                //{
                //    endpointConfigurator.Handler<SubmitOrder>(context =>
                //    {
                //        var consumer = new SubmitOrderConsumer();
                //        return consumer.Consume(context);
                //    });
                //});

                // 3번 방법) 공유되는 consumer를 queue와 연결하는법
                //configurator.ReceiveEndpoint(host, "input_queue", endpointConfigurator =>
                //{
                //    endpointConfigurator.Instance(_sharedConsumer);
                //});

                // 4번 방법) Observer를 등록하는경우.
                //configurator.ReceiveEndpoint(host, "input_queue", endpointConfigurator =>
                //{
                //    endpointConfigurator.Observer(_shareConsumerObserver);
                //});
            });
            
            //bus.ConnectConsumer(() => new SubmitOrderConsumer());

            var busHandle = bus.Start();
            do
            {
                Console.WriteLine("Enter customer (or quit to exit)");
                var customer = Console.ReadLine();

                if("quit".Equals(customer, StringComparison.OrdinalIgnoreCase))
                    break;

                bus.Publish<SubmitOrder>(new
                {
                    SubmitDate = DateTime.Now,
                    CustomerNumber = customer,
                    OrderNumber = "Order_321",
                });
            }
            while (true);

            bus.Stop();
        }
    }

    internal class SubmitOrderObserver : IObserver<ConsumeContext<SubmitOrder>>
    {
        public static SubmitOrderObserver Instance = new SubmitOrderObserver();

        public void OnNext(ConsumeContext<SubmitOrder> value)
        {
            Console.WriteLine("Submit Order is observed : {0}", value.Message);
        }

        public void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            //throw new NotImplementedException();
        }
    }
}
