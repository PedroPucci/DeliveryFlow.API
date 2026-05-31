using System.ComponentModel;

namespace DeliveryFlow.Application.Contracts.DomainErrors
{
    public enum OrderErrors
    {
        [Description("'OrderNumber' must be greater than zero!")]
        Order_Error_InvalidOrderNumber,

        [Description("'Description' can not be null or empty!")]
        Order_Error_DescriptionCanNotBeNullOrEmpty,

        [Description("'Description' must be at least 5 characters long!")]
        Order_Error_DescriptionLengthLessFive,

        [Description("'Value' must be greater than zero!")]
        Order_Error_InvalidValue,

        [Description("'ZipCode' can not be null or empty!")]
        Order_Error_InvalidZipCode,

        [Description("'Number' can not be null or empty!")]
        Order_Error_InvalidAddressNumber
    }
}