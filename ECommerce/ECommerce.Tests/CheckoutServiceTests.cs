using AutoFixture;
using ECommerce.Modals;
using ECommerce.Repositories;
using ECommerce.Services;
using Moq;
using NUnit.Framework;
using System;
using FluentAssertions;

namespace ECommerce.Tests
{
  public class CheckoutServiceTests
  {
    private readonly DateTime today = DateTime.Now;

    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
      _fixture = new Fixture();
    }

    // Doğum günü bugün olan müşterilerimize %20 indirim uygulanır.
    [Test]
    public void it_should_apply_20_percent_discount_when_customers_birthday()
    {
      // Arrange
      var customerId = _fixture.Create<long>();

      const decimal basketTotal = 100M;
      const decimal expectedPaymentTotal = 90M;

      var customerRepo = new Mock<ICustomerRepo>();

      var todayBornCustomer = _fixture.Build<Customer>()
                                      .With(x => x.Birthdate, today)
                                      .With(x => x.IsVip, false)
                                      .Create();

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(todayBornCustomer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      var payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      payedAmount.Should().Be(expectedPaymentTotal);
    }

    [Test]
    public void it_should_not_apply_20_percent_discount_when_it_is_not_birthday()
    {
      // Arrange
      var customerId = _fixture.Create<long>();

      const decimal basketTotal = 100M;
      const decimal expectedPaymentTotal = 110M;

      var customerRepo = new Mock<ICustomerRepo>();

      var customer = _fixture.Build<Customer>()
        .With(x => x.Birthdate, today.AddDays(-2))
        .With(x => x.IsVip, false)
        .Create();

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(customer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      decimal payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      payedAmount.Should().Be(expectedPaymentTotal);
    }

    [Test]
    public void it_should_charge_10_lira_shipment_fee()
    {
      // Arrange
      var customerId = _fixture.Create<long>();

      const decimal basketTotal = 100M;
      const decimal expectedPaymentTotal = 110M;

      var customerRepo = new Mock<ICustomerRepo>();

      var customer = _fixture.Build<Customer>()
        .With(x => x.Birthdate, today.AddDays(-2))
        .With(x => x.IsVip, false)
        .Create();

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(customer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      var payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      payedAmount.Should().Be(expectedPaymentTotal);
    }

    [Test]
    public void it_should_not_charge_shipment_fee_when_customer_is_vip()
    {
      // Arrange
      var customerId = _fixture.Create<long>();

      const decimal basketTotal = 100M;
      const decimal expectedPaymentTotal = 100M;

      var customerRepo = new Mock<ICustomerRepo>();

      var customer = _fixture.Build<Customer>()
        .With(x => x.Birthdate, today.AddDays(-2))
        .With(x => x.IsVip, true)
        .Create();

      customerRepo
        .Setup(cr => cr.getById(customerId))
        .Returns(customer);

      var checkoutService = new CheckoutService(customerRepo.Object);

      // Act
      var payedAmount = checkoutService.Checkout(customerId, basketTotal);

      // Assert
      payedAmount.Should().Be(expectedPaymentTotal);
    }
  }
}