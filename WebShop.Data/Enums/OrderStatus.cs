using System.ComponentModel;

namespace WebShop.Data.Enums
{
    public enum OrderStatus
    {
        Created, // Заказ создан
        Accepted, // Принят на обработку
        OnDelivery, // Находится на доставке
        Done // Товар доставлен по указанному адресу
    }
}
