using System.Net.Http.Headers;
using Entities;
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
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Files/{fileId}/GetFileData");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);

            ExternalResponse<string, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<string, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<UploadFileResponse, ErrorResponse>> UploadFile(
            string? identityCenterToken, IFormFile? file)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            ExternalResponse<UploadFileResponse, ErrorResponse> result;

            if (file == null && file!.Length == 0)
            {
                result = new ExternalResponse<UploadFileResponse, ErrorResponse>
                {
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

            var response = await _httpClient.PostAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Messages/SendMessageWithEntitiesAttached", formData);

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<UploadFileResponse, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<UploadFileResponse, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<UploadFileResponse, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }
    }
}