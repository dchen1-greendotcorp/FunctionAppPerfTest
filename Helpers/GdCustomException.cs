using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Helpers
{
    [Serializable]
    public class GdCustomException : Exception, ISerializable
    {
        public string StatusCode { get; set; }
        public string StatusSubCode { get; set; }

        public GdCustomException(string statusCode, string statusSubCode, string descriptionMessage = "")
            : base(descriptionMessage)
        {
            StatusCode = statusCode;
            StatusSubCode = statusSubCode;
        }

        public GdCustomException(string statusCode, string statusSubCode, string descriptionMessage, Exception inner)
            : base(descriptionMessage, inner)
        {
            this.StatusCode = statusCode;
            StatusSubCode = statusSubCode;
        }

        internal GdCustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(StatusCode), StatusCode);
            info.AddValue(nameof(StatusSubCode), StatusSubCode);
            base.GetObjectData(info, context);
        }
    }
}
