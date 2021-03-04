using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace APIDaemonClient
{
    public interface IClientAppBuilderWrapper
    {
        IConfidentialClientApplication app { get; set; }

        Task<AuthenticationResult> GetAuthenticationResult();
    }
}