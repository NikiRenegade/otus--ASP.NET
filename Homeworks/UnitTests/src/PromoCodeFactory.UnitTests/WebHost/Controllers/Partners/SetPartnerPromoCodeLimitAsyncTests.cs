using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;
        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }
        public Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = true,
                NumberIssuedPromoCodes = 42,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                        CreateDate = new DateTime(2020, 07, 9),
                        EndDate = new DateTime(2020, 10, 9),
                        Limit = 100,
                        PartnerId = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8")
                    }
                }
            };

            return partner;
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = null;
            _partnersRepositoryMock.Setup(r => r.GetByIdAsync(partnerId)).ReturnsAsync(partner);
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                Limit = 5,
                EndDate = DateTime.Now.AddDays(10)
            };
            
            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = CreateBasePartner();
            partner.IsActive = false;
            _partnersRepositoryMock.Setup(r => r.GetByIdAsync(partnerId)).ReturnsAsync(partner);
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                Limit = 5,
                EndDate = DateTime.Now.AddDays(10)
            };
            
            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PreviousActiveLimitNotEnded_ResetsNumberIssuedPromoCodes()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = CreateBasePartner();
            
            partner.PartnerLimits = new List<PartnerPromoCodeLimit>();

            var activeLimit = new PartnerPromoCodeLimit()
            {
                Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                CreateDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(+5), // лимит ещё не закончился
                Limit = 50,
                PartnerId = partner.Id,
                Partner = partner
            };

            partner.PartnerLimits.Add(activeLimit);

            _partnersRepositoryMock.Setup(r => r.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest()
            {
                Limit = 5,
                EndDate = DateTime.Now.AddDays(10)
            };
            
            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);
            
            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            
            partner.NumberIssuedPromoCodes.Should().Be(0);
            
            partner.PartnerLimits
                .Any(l => l.Limit == request.Limit && l.EndDate == request.EndDate)
                .Should().BeTrue();
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PreviousActiveLimitEnded_NotResetsNumberIssuedPromoCodes()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = CreateBasePartner();
            
            partner.PartnerLimits = new List<PartnerPromoCodeLimit>();

            var activeLimit = new PartnerPromoCodeLimit()
            {
                Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                CreateDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(-3),
                Limit = 50,
                PartnerId = partner.Id,
                Partner = partner
            };

            partner.PartnerLimits.Add(activeLimit);

            _partnersRepositoryMock.Setup(r => r.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest()
            {
                Limit = 5,
                EndDate = DateTime.Now.AddDays(10)
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            
            partner.NumberIssuedPromoCodes.Should().Be(42);
            
            partner.PartnerLimits
                .Any(l => l.Limit == request.Limit && l.EndDate == request.EndDate)
                .Should().BeTrue();
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_NewLimitIsSet_PreviousActiveLimitIsCancelled()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = CreateBasePartner();

            partner.PartnerLimits = new List<PartnerPromoCodeLimit>();

            var activeLimit = new PartnerPromoCodeLimit()
            {
                Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                CreateDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(50),
                Limit = 50,
                PartnerId = partner.Id,
                Partner = partner
            };

            partner.PartnerLimits.Add(activeLimit);

            _partnersRepositoryMock.Setup(r => r.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest()
            {
                Limit = 5,
                EndDate = DateTime.Now.AddDays(10)
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            
            activeLimit.CancelDate.Should().NotBeNull();
            
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_InvalidLimit_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = CreateBasePartner();

            _partnersRepositoryMock.Setup(r => r.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest()
            {
                Limit = 0,
                EndDate = DateTime.Now.AddDays(10)
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);
            
            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            
            
        }
    }
}