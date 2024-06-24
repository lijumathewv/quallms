using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace QualvationLibrary
{
    public class Client (IConfiguration config, CustomLogger logger)
    {
        public async Task<byte[]> GetImageAsBase64Async(string imageUrl)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };

            using (HttpClient client = new HttpClient(handler))
            {
                var imageBytes = await client.GetByteArrayAsync(imageUrl);
                return imageBytes;
            }
        }

        public T ExecutePostAPI<T>(string url, StringContent param)
        {
            var returnModel = PostAPI(url, param).Result;

            object model;

            logger.IsError = returnModel.Error;
            if (!returnModel.Error)
            {
                if (!string.IsNullOrEmpty(returnModel.ReturnModel))
                {
                    model = JsonSerializer.Deserialize<T>(returnModel.ReturnModel);
                }
                else
                {
                    model = returnModel;
                }

                logger.IsSuccess = true;
                logger.SuccessMessage = returnModel.Message;
            }
            else
            {
                logger.ErrorMessage = returnModel.Message;
                logger.ErrorMessage += "<br/>" + returnModel.ApiError!.Title;

                if (returnModel.ApiError.Errors != null)
                {
                    foreach (var err in returnModel.ApiError.Errors)
                    {
                        if (err.Value != null)
                        {
                            foreach (var val in err.Value)
                            {
                                logger.ErrorMessage += "<br/>" + val;
                            }
                        }
                    }
                }

                model = returnModel;

            }

            return (T)model!;
        }

        public T ExecutePostAPI<T>(string url, string param = "")
        {
            var returnModel = PostAPI(url, param).Result;

            object model;

            if (returnModel.Message.Contains("Invalid"))
            {
                logger.IsError = true;
            }
            else
            {
                logger.IsError = returnModel.Error;
            }

            if (!logger.IsError)
            {
                if (!string.IsNullOrEmpty(returnModel.ReturnModel))
                {
                    model = JsonSerializer.Deserialize<T>(returnModel.ReturnModel!);
                }
                else
                {
                    model = returnModel;
                }

                logger.IsSuccess = true;
                logger.SuccessMessage = returnModel.Message;
            }
            else
            {
                logger.ErrorMessage = returnModel.Message;
                if (returnModel.ApiError != null)
                {
                    logger.ErrorMessage += "<br/>" + returnModel.ApiError!.Title;
                    if (returnModel.ApiError.Errors != null)
                    {
                        foreach (var err in returnModel.ApiError.Errors)
                        {
                            if (err.Value != null)
                            {
                                foreach (var val in err.Value)
                                {
                                    logger.ErrorMessage += "<br/>" + val;
                                }
                            }
                        }
                    }
                }
                model = returnModel;

            }

            return (T)model!;
        }

        public async Task<ResultCommon> PostSignInUpAPI(string url, string param)
        {
            ResultCommon resultLogin = new ResultCommon();

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
            using (HttpClient client = new HttpClient(handler))
            {
                if (string.IsNullOrEmpty(logger.BaseURL))
                {
                    logger.BaseURL = config.GetSection("AppSettings:API").Value;
                }

                client.BaseAddress = new Uri(logger.BaseURL!);

                HttpResponseMessage response;

                if (string.IsNullOrEmpty(param))
                {
                    response = await client.PostAsync(url, null);
                }
                else
                {
                    HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(url, content);
                }

                var result = response.Content.ReadAsStringAsync();

                string APIResult = result.Result.ToString();

                if (response.IsSuccessStatusCode)
                {
                    ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(APIResult)!;
                    resultLogin.Error = !res.Error;

                    if (string.IsNullOrEmpty(res.Message))
                    {
                        ResultLogin res1 = JsonSerializer.Deserialize<ResultLogin>(APIResult)!;
                        resultLogin.Error = res1.Error;
                        resultLogin.ReturnModel = JsonSerializer.Serialize(res1.Value!);
                    }
                    else
                    {
                        resultLogin.ApiError = new APIError();
                        if (resultLogin.Error)
                        {
                            resultLogin.Message = res.Message;
                        }
                        else
                        {
                            resultLogin.ReturnModel = APIResult;
                        }
                    }

                }
                else
                {
                    resultLogin.Error = true;
                    resultLogin.Message = APIResult;
                    logger.ErrorMessage = "";

                    if (!APIResult.ToLower().Contains("mysql"))
                    {
                        resultLogin.ApiError = JsonSerializer.Deserialize<APIError>(APIResult);

                        if (resultLogin.ApiError != null)
                        {
                            if (resultLogin.ApiError.Errors != null)
                            {
                                foreach (var err in resultLogin.ApiError.Errors)
                                {
                                    if (err.Value != null)
                                    {
                                        foreach (var val in err.Value)
                                        {
                                            logger.ErrorMessage += "<br/>" + val;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return resultLogin;
        }

        public async Task<ResultCommon> PostAPI(string url, StringContent param)
        {
            ResultCommon resultLogin = new ResultCommon();
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

                using (HttpClient client = new HttpClient(handler))
                {

                    if (string.IsNullOrEmpty(logger.BaseURL))
                    {
                        logger.BaseURL = config.GetSection("AppSettings:API").Value;
                    }

                    client.BaseAddress = new Uri(logger.BaseURL!);

                    Task<HttpResponseMessage> response;

                    response = client.PostAsync(url, param);

                    var result = response.Result.Content.ReadAsStringAsync();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(result.Result.ToString())!;

                        resultLogin.Error = res.Error;
                        if (res.Message.ToLower().Contains("error"))
                        {
                            resultLogin.Error = true;
                        }

                        if (!resultLogin.Error)
                        {
                            resultLogin.ReturnModel = res.ReturnModel;
                            resultLogin.Message = res.Message;
                        }
                        else
                        {
                            resultLogin.ApiError = new APIError();
                            if (resultLogin.Error)
                            {
                                resultLogin.Message = res.Message;
                            }
                            else
                            {
                                resultLogin.ReturnModel = result.Result.ToString();
                            }
                        }

                    }
                    else
                    {
                        resultLogin.Error = true;
                        resultLogin.ApiError = JsonSerializer.Deserialize<APIError>(result.Result.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                resultLogin.Error = true;
                resultLogin.ApiError = JsonSerializer.Deserialize<APIError>(ex.Message.ToString());
            }

            return resultLogin;
        }

        public async Task<ResultCommon> PostAPI(string url, string param)
        {
            ResultCommon resultLogin = new ResultCommon();
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

                using (HttpClient client = new HttpClient(handler))
                {

                    if (string.IsNullOrEmpty(logger.BaseURL))
                    {
                        logger.BaseURL = config.GetSection("AppSettings:API").Value;
                    }

                    client.BaseAddress = new Uri(logger.BaseURL!);

                    Task<HttpResponseMessage> response;

                    if (string.IsNullOrEmpty(param))
                    {
                        response = client.PostAsync(url, null);
                    }
                    else
                    {
                        HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
                        response = client.PostAsync(url, content);
                    }

                    var result = response.Result.Content.ReadAsStringAsync();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(result.Result.ToString())!;

                        resultLogin.Error = res.Error;
                        if (res.Message.ToLower().Contains("error"))
                        {
                            resultLogin.Error = true;
                        }

                        if (!resultLogin.Error)
                        {
                            resultLogin.ReturnModel = res.ReturnModel;
                            resultLogin.Message = res.Message;
                        }
                        else
                        {
                            resultLogin.ApiError = new APIError();
                            if (resultLogin.Error)
                            {
                                resultLogin.Message = res.Message;
                            }
                            else
                            {
                                resultLogin.ReturnModel = result.Result.ToString();
                            }
                        }

                    }
                    else
                    {
                        resultLogin.Error = true;
                        resultLogin.Message = result.Result.ToString();
                        if (!resultLogin.Message.Contains("InvalidOperationException"))
                        {
                            resultLogin.ApiError = JsonSerializer.Deserialize<APIError>(result.Result.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultLogin.Error = true;
                resultLogin.ApiError = JsonSerializer.Deserialize<APIError>(ex.Message.ToString());
            }

            return resultLogin;
        }

        public T ParseResult<T>(string response)
        {
            return (T)JsonSerializer.Deserialize<T>(response!)!;
        }

    }

    public class ResultLogin
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("value")]
        public LoginProperties? Value { get; set; }
    }
}
