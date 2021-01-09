using System.Net.Http;
using System.Threading.Tasks;

namespace APIDaemonClient
{
    public interface IDaemonHttpClient
    {
        HttpClient HttpClient { get; }
        void ConfigureRequestHeaders(string accessToken);
        Task<bool> HttpGetAsync(string URL);
        Task<bool> HttpPostStringAsync(string URL, string postContent);
    }
}