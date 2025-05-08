using System.Net;
using System.Net.Http.Headers;
using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class FilesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IFilesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<string, ErrorResponse>> GetFileData(int fileId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Files/{fileId}/GetFileData");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error",
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<string, ErrorResponse>
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error",
                        Exception = ex
                    }
                };
            }
        }

        public async Task<ExternalResponse<UploadFileResponse, ErrorResponse>> UploadFile(
            string? identityCenterToken, string? identityCompany, IFormFile? file)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                if (file == null && file!.Length == 0)
                {
                    return new ExternalResponse<UploadFileResponse, ErrorResponse>
                    {
                        StatusCode = (int) HttpStatusCode.BadRequest,
                        Error = new ErrorResponse{
                            Error = "unknown_error",
                            ErrorDescription = "exception_error"
                        }
                    };
                }

                var fileContent = new StreamContent(file!.OpenReadStream())
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue(file.ContentType),
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = file.FileName
                        }
                    }
                };

                using var formData = new MultipartFormDataContent
                {
                    fileContent
                };

                var response = await _httpClient.PostAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Files", formData);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<UploadFileResponse, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<UploadFileResponse, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error",
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<UploadFileResponse, ErrorResponse>
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error",
                        Exception = ex
                    }
                };
            }
        }
    }
}