using ECommerce.Modals;

namespace ECommerce.Repositories
{
  public interface ICustomerRepo
  {
    Customer getById(long customerId);
  }
}