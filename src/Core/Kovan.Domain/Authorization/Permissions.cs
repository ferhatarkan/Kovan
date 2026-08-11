namespace Kovan.Domain.Authorization;

/// <summary>
/// Uygulama genelindeki tüm izinleri merkezi bir yerden yönetmek için kullanılır.
/// </summary>
public static class Permissions
{
    public static class Invoices
    {
        public const string View = "Permissions.Invoices.View";
        public const string Create = "Permissions.Invoices.Create";
        public const string Edit = "Permissions.Invoices.Edit";
        public const string Delete = "Permissions.Invoices.Delete";
    }

    public static class Customers
    {
        public const string View = "Permissions.Customers.View";
        public const string Create = "Permissions.Customers.Create";
        public const string Edit = "Permissions.Customers.Edit";
        public const string Delete = "Permissions.Customers.Delete";
    }

    public static class Products
    {
        public const string View = "Permissions.Products.View";
        public const string Create = "Permissions.Products.Create";
        public const string Edit = "Permissions.Products.Edit";
        public const string Delete = "Permissions.Products.Delete";
    }

    public static class PurchaseOrders
    {
        public const string View = "Permissions.PurchaseOrders.View";
        public const string Create = "Permissions.PurchaseOrders.Create";
        public const string Edit = "Permissions.PurchaseOrders.Edit";
        public const string Delete = "Permissions.PurchaseOrders.Delete";
    }

    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create"; // Örneğin, kullanıcı davet etme
        public const string Edit = "Permissions.Users.Edit";   // Örneğin, rol değiştirme
        public const string Delete = "Permissions.Users.Delete";
    }
}