using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

[ServiceContract(CallbackContract = typeof(IDataChannel))]
public interface IDataChannel
{
    [OperationContract(IsOneWay = true)]
    void SendData(string data);
}

public class WCFP2PTransport : IDataChannel
{
    private IDataChannel m_outChannel = null;
    private DuplexChannelFactory<IDataChannel> m_factory = null;
    private Task m_completion;
    private string m_name;

    public WCFP2PTransport(string service, string name)
    {
        m_name = name;
        try
        {
            var binding = new NetPeerTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            var endpoint = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IDataChannel)),
                binding,
                new EndpointAddress("net.p2p://" + service));

            m_factory = new DuplexChannelFactory<IDataChannel>(new InstanceContext(this), endpoint);

            m_outChannel = m_factory.CreateChannel();

            m_completion = Task.Factory.FromAsync(((ICommunicationObject)m_outChannel).BeginOpen, ((ICommunicationObject)m_outChannel).EndOpen, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var tsk = new TaskCompletionSource<bool>();
            tsk.SetException(ex);
            m_completion = tsk.Task;
        }

    }

    public Task ChannelOpened
    {
        get
        {
            return m_completion;
        }
    }

    public void SendToPeer(string data)
    {
        m_outChannel.SendData(data);
    }

    // IDataChannel method(s), handle incoming traffic
    public void SendData(string data)
    {
        Console.WriteLine("{0} Received data: {1}", m_name, data);
    }

    // cleanup code omitted for brevity
}

