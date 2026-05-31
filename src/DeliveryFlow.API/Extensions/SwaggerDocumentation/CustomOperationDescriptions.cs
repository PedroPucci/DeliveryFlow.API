using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeliveryFlow.API.Extensions.SwaggerDocumentation
{
    public class CustomOperationDescriptions : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context?.ApiDescription?.HttpMethod is null || context.ApiDescription.RelativePath is null)
                return;

            var path = context.ApiDescription.RelativePath.ToLowerInvariant();

            var routeHandlers = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                { "users",               () => HandleUsersOperations(operation, context) },
                { "orders",               () => HandleOrdersOperations(operation, context) },
                { "auth",               () => HandleAuthOperations(operation, context) }

            };

            foreach (var kv in routeHandlers
                     .OrderByDescending(k => k.Key.Length))
            {
                if (path.Contains(kv.Key))
                {
                    kv.Value.Invoke();
                    return;
                }
            }
        }

        private void HandleUsersOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new User.";
                operation.Description = "This endpoint allows you to create a new User by providing the necessary details.";
                AddResponses(operation, "200", "The User was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing User.";
                operation.Description = "This endpoint allows you to update an existing User by providing the necessary details.";
                AddResponses(operation, "200", "The User was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing User.";
                operation.Description = "This endpoint allows you to delete an existing User by providing the ID.";
                AddResponses(operation, "200", "The User was successfully deleted.");
                AddResponses(operation, "404", "User not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                if (path.Contains("{id}"))
                {
                    operation.Summary = "Retrieve user by id.";
                    operation.Description = "This endpoint is responsible for retrieving an active user by id.";
                    AddResponses(operation, "200", "User retrieved successfully.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retrieve all users.";
                    operation.Description = "This endpoint is responsible for retrieving all users.";
                    AddResponses(operation, "200", "All users retrieved successfully.");
                }
            }
        }

        private void HandleOrdersOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new Order.";
                operation.Description = "This endpoint allows you to create a new order by providing the necessary details.";
                AddResponses(operation, "200", "The order was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing Order.";
                operation.Description = "This endpoint allows you to update an existing order by providing the necessary details.";
                AddResponses(operation, "200", "The order was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing Order.";
                operation.Description = "This endpoint allows you to delete an existing order by providing the ID.";
                AddResponses(operation, "200", "The order was successfully deleted.");
                AddResponses(operation, "404", "Order not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                if (path.Contains("{id}"))
                {
                    operation.Summary = "Retrieve order by id.";
                    operation.Description = "This endpoint is responsible for retrieving an order by id.";
                    AddResponses(operation, "200", "Order retrieved successfully.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retrieve all orders.";
                    operation.Description = "This endpoint is responsible for retrieving all orders.";
                    AddResponses(operation, "200", "All orders retrieved successfully.");
                }
            }
            else if (method == "PATCH" && path.Contains("delivery"))
            {
                operation.Summary = "Register order delivery.";
                operation.Description = "This endpoint allows you to register the delivery date and time of an order.";
                AddResponses(operation, "204", "Delivery registered successfully.");
                AddResponses(operation, "404", "Order not found.");
            }
        }

        private void HandleAuthOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST" && path.Contains("auth/login"))
            {
                operation.Summary = "Authenticate user.";
                operation.Description = "This endpoint allows the user to authenticate by providing login credentials.";
                AddResponses(operation, "200", "User authenticated successfully.");
                AddResponses(operation, "401", "Invalid email or password.");
            }
        }

        private void AddResponses(OpenApiOperation operation, string statusCode, string description)
        {
            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses.Add(statusCode, new OpenApiResponse { Description = description });
            }
        }
    }
}