using Newtonsoft.Json;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FunctionAppPerfTest.Models
{
    public class CreateCardAccountResponse : BaseResponse<CardAccountCreated>
    {
    }

    [ExcludeFromCodeCoverage]
    public class CardAccountCreated
    {
        public string CardAccountId { get; set; }
        public dynamic OtherData { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CardAccount
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("accountRefNo")]
        public string AccountRefNo { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AccountCreditData
    {
        [JsonProperty("creditCheckDefinitionId")]
        public string? CreditCheckDefinitionId { get; set; }

        [JsonProperty("creditLimit")]
        public string? CreditLimit { get; set; }

        [JsonProperty("currencyCode")]
        public string? CurrencyCode { get; set; }

        [JsonProperty("lastCreditLimitChangeDate")]
        public string? LastCreditLimitChangeDate { get; set; }

        [JsonProperty("institutionId")]
        public string? InstitutionId { get; set; }

        [JsonProperty("overlimitFlag")]
        public string? OverlimitFlag { get; set; }

        [JsonProperty("wholeAccountCreditRule")]
        public string? WholeAccountCreditRule { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CardApplication
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("alternativePAN")]
        public string? AlternativePAN { get; set; }

        [JsonProperty("pan")]
        public string? Pan { get; set; }

        [JsonProperty("plasticIssueNo")]
        public string? PlasticIssueNo { get; set; }

        [JsonProperty("cardVerification2")]
        public string? CardVerification2 { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Device
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("endDate")]
        public string? EndDate { get; set; }
        [JsonProperty("applicationData")]
        public List<CardApplication> ApplicationData { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Agreement
    {
        [JsonProperty("agreementReference")]
        public string? AgreementReference { get; set; }
        [JsonProperty("institutionId")]
        public string? InstitutionId { get; set; }
    }
}