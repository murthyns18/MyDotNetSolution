using LMS.Models;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace LMS.Services
{
    public static class API
    {
        static string apiBaseURL = AppSettings.APIDetails.URL;

        public static string Post(string URL, string? token, dynamic myobject)
        {
            try
            {
                HttpClient client = new HttpClient();
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                JsonContent content = JsonContent.Create(myobject);
                var response = client.PostAsync(apiBaseURL + URL, content).GetAwaiter().GetResult();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return responseMessage;

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var responseMessage = "token expired";
                    return responseMessage;
                }
                else
                {
                    var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return responseMessage;
                }
            }
            catch (HttpRequestException httpEx) when (httpEx.InnerException is System.Net.Sockets.SocketException socketEx)
            {
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.NetworkDown:
                        return "The network is down. Please check your network connection.";
                    case SocketError.NetworkUnreachable:
                        return "Network unreachable. Please ensure you have a working internet connection.";
                    case SocketError.HostUnreachable:
                        return "The remote host is unreachable. Please try again later.";
                    case SocketError.ConnectionRefused:
                        return "The connection was refused by the server. Please check if the server is running and reachable.";
                    case SocketError.ConnectionReset:
                        return "The connection was reset by the server. Please try again.";
                    case SocketError.TimedOut:
                        return "The connection attempt timed out. Please try again later.";
                    default:
                        return "A network issue occurred. Please check your internet connection and try again.";
                }
            }

            catch (Exception ex)
            {
                return ex.Message;
            }


        }
        public static string Get(string URL, string? token, string? queryParameters = null)
        {
            try
            {
                HttpClient client = new HttpClient();
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                URL = URL + (!string.IsNullOrEmpty(queryParameters) ? "?" + queryParameters : string.Empty);
                var response = client.GetAsync(apiBaseURL + URL).GetAwaiter().GetResult();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return responseMessage;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var responseMessage = "token expired";
                    return responseMessage;
                }
                else
                {
                    var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return responseMessage;
                }

            }
            catch (HttpRequestException httpEx) when (httpEx.InnerException is System.Net.Sockets.SocketException socketEx)
            {
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.NetworkDown:
                        return "The network is down. Please check your network connection.";
                    case SocketError.NetworkUnreachable:
                        return "Network unreachable. Please ensure you have a working internet connection.";
                    case SocketError.HostUnreachable:
                        return "The remote host is unreachable. Please try again later.";
                    case SocketError.ConnectionRefused:
                        return "The connection was refused by the server. Please check if the server is running and reachable.";
                    case SocketError.ConnectionReset:
                        return "The connection was reset by the server. Please try again.";
                    case SocketError.TimedOut:
                        return "The connection attempt timed out. Please try again later.";
                    default:
                        return "A network issue occurred. Please check your internet connection and try again.";
                }
            }

            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        public static string Delete(string URL, string? token)
        {
            try
            {
                HttpClient client = new HttpClient();
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.DeleteAsync(apiBaseURL + URL).GetAwaiter().GetResult();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return responseMessage;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "token expired";
                }
                else
                {
                    var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return responseMessage;
                }
            }
            catch (HttpRequestException httpEx) when (httpEx.InnerException is SocketException socketEx)
            {
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.NetworkDown:
                        return "The network is down. Please check your network connection.";
                    case SocketError.NetworkUnreachable:
                        return "Network unreachable. Please ensure you have a working internet connection.";
                    case SocketError.HostUnreachable:
                        return "The remote host is unreachable. Please try again later.";
                    case SocketError.ConnectionRefused:
                        return "The connection was refused by the server. Please check if the server is running and reachable.";
                    case SocketError.ConnectionReset:
                        return "The connection was reset by the server. Please try again.";
                    case SocketError.TimedOut:
                        return "The connection attempt timed out. Please try again later.";
                    default:
                        return "A network issue occurred. Please check your internet connection and try again.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
