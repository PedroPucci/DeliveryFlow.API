using DeliveryFlow.Domain.Entities;

namespace DeliveryFlow.Shared.Logging
{
    public static class LogMessages
    {
        #region User Authentication

        public static string LoginUserSuccess(UserEntity userEntity) =>
            $"User logged in successfully. User name: {userEntity.Name}";

        public static string TokenGenerateSuccess() =>
            "Token generated successfully.";

        public static string InvalidLoginInputs() =>
            "User login failed. Invalid email or password.";

        public static string MissingLoginCredentials() =>
            "Email and password are required.";

        #endregion

        #region User Validation

        public static string InvalidUserInputs() =>
            "Invalid user data.";

        #endregion

        #region User Not Found

        public static string CannotPerformActionOnUser(string action, string userId) =>
            $"Cannot {action} user. User with id {userId} was not found.";

        #endregion

        #region User CRUD

        public static string AddUserError(Exception ex) =>
            $"Error adding user. Details: {ex.Message}";

        public static string AddUserSuccess(UserEntity userEntity) =>
            $"User name: {userEntity.Name} - id: {userEntity.Id} added successfully.";

        public static string UpdateUserError(Exception ex) =>
            $"Error updating user. Details: {ex.Message}";

        public static string UpdateUserSuccess(UserEntity userEntity) =>
            $"User name: {userEntity.Name} - id: {userEntity.Id} updated successfully.";

        public static string DeleteUserError(Exception ex) =>
            $"Error deleting user. Details: {ex.Message}";

        public static string DeleteUserSuccess(UserEntity userEntity) =>
            $"User name: {userEntity.Name} - id: {userEntity.Id} deleted successfully.";

        public static string GetAllUsersError(Exception ex) =>
            $"Error retrieving users list. Details: {ex.Message}";

        public static string GetAllUsersSuccess() =>
            "Users retrieved successfully.";

        public static string GetUserByIdError(Exception ex) =>
            $"Error retrieving user by id. Details: {ex.Message}";

        public static string GetUserByIdSuccess(UserEntity userEntity) =>
            $"User name: {userEntity.Name} - id: {userEntity.Id} retrieved successfully.";

        #endregion

        #region Password

        public static string InvalidPassword() =>
            "Incorrect current password.";

        public static string UpdatePasswordSuccess() =>
            "Password updated successfully.";

        #endregion

        #region Order CRUD

        public static string AddOrderError(Exception ex) =>
            $"Error adding order. Details: {ex.Message}";

        public static string AddOrderSuccess(OrderEntity orderEntity) =>
            $"Order number: {orderEntity.OrderNumber} - id: {orderEntity.Id} added successfully.";

        public static string UpdateOrderError(Exception ex) =>
            $"Error updating order. Details: {ex.Message}";

        public static string UpdateOrderSuccess(OrderEntity orderEntity) =>
            $"Order number: {orderEntity.OrderNumber} - id: {orderEntity.Id} updated successfully.";

        public static string DeleteOrderError(Exception ex) =>
            $"Error deleting order. Details: {ex.Message}";

        public static string DeleteOrderSuccess(OrderEntity orderEntity) =>
            $"Order number: {orderEntity.OrderNumber} - id: {orderEntity.Id} deleted successfully.";

        public static string GetAllOrdersError(Exception ex) =>
            $"Error retrieving orders list. Details: {ex.Message}";

        public static string GetAllOrdersSuccess() =>
            "Orders retrieved successfully.";

        public static string GetOrderByIdError(Exception ex) =>
            $"Error retrieving order by id. Details: {ex.Message}";

        public static string GetOrderByIdSuccess(OrderEntity orderEntity) =>
            $"Order number: {orderEntity.OrderNumber} - id: {orderEntity.Id} retrieved successfully.";

        public static string GetOrderByNumberError(Exception ex) =>
            $"Error retrieving order by number. Details: {ex.Message}";

        public static string GetOrderByNumberSuccess(OrderEntity orderEntity) =>
            $"Order number: {orderEntity.OrderNumber} retrieved successfully.";

        public static string RegisterDeliveryError(Exception ex) =>
            $"Error registering order delivery. Details: {ex.Message}";

        public static string RegisterDeliverySuccess(OrderEntity orderEntity) =>
            $"Delivery registered successfully for order number: {orderEntity.OrderNumber} - id: {orderEntity.Id}.";

        public static string OrderAlreadyExists(int orderNumber) =>
            $"Order number: {orderNumber} already exists.";

        #endregion
    }
}