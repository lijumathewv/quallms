using Microsoft.AspNetCore.Mvc;
using QualvationLibrary;
using System.Text;

namespace QualLMS.WebAppMvc.Controllers
{
    public class BaseController : Controller
    {
        public string GetSessionValue(string name)
        {
            var session = HttpContext.Session;
            try
            {
                if (session == null)
                {
                    throw new Exception("Error: Session is null in Data.SessionClass");
                }
                else
                {
                    if (!session.IsAvailable)
                    {
                        throw new Exception("!IsAvailable");
                    }

                    byte[] value;
                    // Check if the session param exists.
                    if (!session.TryGetValue(name, out value))
                    {
                        throw new Exception("TryGetValue ::" + name);
                    }

                    return Encoding.UTF8.GetString(value);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SetSessionValue(string key, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            HttpContext.Session.Set(key, bytes);
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
