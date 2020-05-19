using System;
using ECommerce.Modals;
using ECommerce.Repositories;
using ECommerce.Services;
using NUnit.Framework;
using Moq;

namespace ECommerce.Tests
{
  public class CheckoutServiceTests
  {
    private readonly DateTime today = DateTime.Now;

    // Doğum günü bugün olan müşterilerimize %20 indirim uygulanır.
    [Test]
    public void it_should_apply_20_percent_discount_when_customers_birthday()
    {
      // Arrange
      long customerId = 90;

      var customerRepo = new Mock<ICustomerRepo>();

      var todayBornCustomer = new Customer
      {
        Birthdate = today
      };

      customerRepo.Setup(cr => cr.getById(customerId)).Returns(todayBornCustomer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      const decimal basketTotal = 100;

      decimal payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      Assert.AreEqual(90, payedAmount);
    }
    
    [Test]
    public void it_should_not_apply_20_percent_discount_when_it_is_not_birthday()
    {
      // Arrange
      var customerRepo = new Mock<ICustomerRepo>();
      long customerId = 90;
      
      var customer = new Customer
      {
        Birthdate = today.AddDays(-2)
      };

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(customer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      const decimal basketTotal = 100;

      decimal payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      Assert.AreEqual(payedAmount, 110);
    }

    [Test]
    public void it_should_charge_10_lira_shipment_fee()
    {
      // Arrange
      var customerRepo = new Mock<ICustomerRepo>();
      long customerId = 90;

      var customer = new Customer
      {
        Birthdate = today.AddDays(-2)
      };

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(customer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      const decimal basketTotal = 100;

      decimal payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      Assert.AreEqual(payedAmount, 110);
    }

    [Test]
    public void it_should_not_charge_shipment_fee_when_customer_is_vip()
    {
      // Arrange
      var customerRepo = new Mock<ICustomerRepo>();
      long customerId = 90;

      var customer = new Customer
      {
        Birthdate = today.AddDays(-2),
        IsVip = true
      };

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(customer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      const decimal basketTotal = 100;

      decimal payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      Assert.AreEqual(payedAmount, 100);
    }
  }
}