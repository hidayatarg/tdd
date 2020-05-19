using ECommerce.Repositories;
using System;

namespace ECommerce.Services
{
  public class CheckoutService
  {
    private readonly ICustomerRepo customerRepo;

    public CheckoutService(ICustomerRepo customerRepo)
    {
      this.customerRepo = customerRepo;
    }

    public decimal Checkout(long customerId, decimal basketTotal)
    {
      var customer = customerRepo.getById(customerId);

      var finalAmount = basketTotal;

      if (customer.Birthdate.Month == DateTime.Now.Month && customer.Birthdate.Day == DateTime.Now.Day)
      {
        finalAmount = basketTotal * 80 / 100;
      }

      if (!customer.IsVip)
      {
        finalAmount += 10;
      }

      return finalAmount;
    }
  }
}