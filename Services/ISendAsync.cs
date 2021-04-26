using System.Threading.Tasks;

namespace OnlineElection.Services
{
    public interface ISendAsync
    {
       
        Task<bool> SendAsync();
    }
}
