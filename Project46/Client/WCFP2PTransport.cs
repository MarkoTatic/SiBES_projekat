using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

[ServiceContract(CallbackContract = typeof(IDataChannel))]
public interface IDataChannel
{
    [OperationContract(IsOneWay = true)]
    void SendData(string data, string name);
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



            var sourceContext = new InstanceContext(this);

            m_factory = new DuplexChannelFactory<IDataChannel>(sourceContext, endpoint);

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

    public void SendToPeer(string data, string name)
    {
        m_outChannel.SendData(data, name);
    }

    // IDataChannel method(s), handle incoming traffic
    public void SendData(string data, string name)
    {
        if(m_name != name)
            Console.WriteLine("{0} Received data: {1}", m_name, data);
    }

    public void CloseChannel()//da se ugasi konekcija nakon sto klijent pritisne 'x', jer inace bi on bio na gostojucem kanalu sve dok se opet ne redirektuje na neki drugi
    {
        m_factory.Close();
    }

    // cleanup code omitted for brevity
}

