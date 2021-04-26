using System.Threading.Tasks;
using OnlineElection.Models;

namespace OnlineElection.Services
{
    public interface ITokenGenerator
    {
        Task<string> Token(Person person, params object[] t);
    }
}
