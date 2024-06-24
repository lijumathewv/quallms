using Microsoft.AspNetCore.Http;
using System.Text;
using System.Xml.Linq;

namespace QualvationLibrary
{
    public interface ISessionService
    {
        string GetSessionValue(string key);
        void SetSessionValue(string key, string value);
    }

    public class SessionService(IHttpContextAccessor httpContextAccessor, CustomLogger logger) : ISessionService
    {
        public string GetSessionValue(string name)
        {
            var session = httpContextAccessor.HttpContext.Session;
            try
            {
                if (session == null)
                {
                    throw new Exception( "Error: Session is null in SessionService");
                }
                else
                {
                    if (!session.IsAvailable)
                    {
                        throw new Exception("!session.IsAvailable");
                    }

                    byte[] value;
                    // Check if the session param exists.
                    if (!session.TryGetValue(name, out value))
                    {
                        throw new Exception("session.TryGetValue ::" + name);
                    }

                    return Encoding.UTF8.GetString(value);
                }
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                throw;
            }
        }

        public void SetSessionValue(string key, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            httpContextAccessor.HttpContext.Session.Set(key, bytes);
        }

        public T ParseRole<T>()
        {
            var role = GetSessionValue("LoginRole");
            if (!string.IsNullOrEmpty(GetSessionValue("LoginRole")))
            {
                return (T)Enum.Parse(typeof(T), GetSessionValue("LoginRole"), true);
            }
            else
            {
                return (T)Enum.Parse(typeof(T), "None", true);
            }
            
        }
    }
}
