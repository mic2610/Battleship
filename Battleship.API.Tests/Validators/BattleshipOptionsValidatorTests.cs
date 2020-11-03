using Battleship.Api.Validators;
using Battleship.API.Models;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Api.Tests.Validators
{
    public class BattleshipOptionsValidatorTests
    {
        private static BattleshipOptionsValidator validator;

        [TestClass]
        public class ShouldHaveError
        {
            [ClassInitialize]
            public static void Initialise(TestContext context)
            {
                validator = new BattleshipOptionsValidator();
            }

            [TestMethod]
            public void WhenPlayerIdIsNull()
            {
                var model = new BattleshipOptions { PlayerId = null };
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(bs => bs.PlayerId);
            }

            [TestMethod]
            public void WhenPropertyValuesAreInvalid()
            {
                var model = new BattleshipOptions { PlayerId = 0, Column = 0, Row = 0, OpponentId = 0 };
                var result = validator.TestValidate(model);
                result.ShouldHaveAnyValidationError();
            }
        }

        [TestClass]
        public class ShouldNotHaveError
        {
            [ClassInitialize]
            public static void Initialise(TestContext context)
            {
                validator = new BattleshipOptionsValidator();
            }

            [TestMethod]
            public void WhenPropertiesAreSet()
            {
                var model = new BattleshipOptions { PlayerId = 1, Column = 1, Row = 1, OpponentId = 2 };
                var result = validator.TestValidate(model);
                result.ShouldNotHaveAnyValidationErrors();
            }
        }
    }
}