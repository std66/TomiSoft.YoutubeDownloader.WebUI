using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TomiSoft.YouTubeDownloader.WebUI.Core;

namespace TomiSoft.YouTubeDownloader.WebUI.Models {
    public class ErrorResponse {
        public string Message { get; }
        public int ErrorCode { get; }

        private readonly HttpStatusCode statusCode;

        public ErrorResponse(ErrorCodes ErrorCode) 
            : this(ErrorCode, HttpStatusCode.InternalServerError) {

        }

        public ErrorResponse(ErrorCodes ErrorCode, HttpStatusCode StatusCode) {
            var type = typeof(ErrorCodes);
            var memInfo = type.GetMember(ErrorCode.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            
            this.Message = ((DisplayAttribute)attributes[0]).Name;
            this.ErrorCode = (int)ErrorCode;
            this.statusCode = StatusCode;
        }

        public JsonResult AsJsonResult() {
            return new JsonResult(this) {
                StatusCode = (int)this.statusCode
            };
        }
    }
}
