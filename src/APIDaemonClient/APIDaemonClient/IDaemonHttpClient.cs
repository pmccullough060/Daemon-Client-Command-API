using APIDaemonClient.Attributes;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIDaemonClient
{
    public interface IDaemonHttpClient
    {
        HttpClient HttpClient { get; }
        void ConfigureRequestHeaders(string accessToken);

        [CLIMethod("HttpGetAsync", "Get all of the command objects")]
        Task<bool> HttpGetAsync();

        [CLIMethod("HttpGetAsync", "Get all of the command objects", ":IndexPosition")]
        Task<bool> HttpGetAsync(int index);


        Task<bool> HttpPostStringAsync(string URL, string postContent);
    }
}