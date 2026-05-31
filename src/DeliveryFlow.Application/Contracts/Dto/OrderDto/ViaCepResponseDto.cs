using System.Text.Json.Serialization;

namespace DeliveryFlow.Application.Contracts.Dto.OrderDto
{
    public class ViaCepResponseDto
    {
        [JsonPropertyName("cep")]
        public string? ZipCode { get; set; }

        [JsonPropertyName("logradouro")]
        public string Street { get; set; } = string.Empty;

        [JsonPropertyName("bairro")]
        public string District { get; set; } = string.Empty;

        [JsonPropertyName("localidade")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("uf")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("erro")]
        public bool Error { get; set; }
    }
}