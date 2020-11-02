using Battleship.API.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleship.Api.Validators
{
    public class BattleshipOptionsValidator : AbstractValidator<BattleshipOptions>
    {
        public BattleshipOptionsValidator()
        {
            RuleFor(b => b.Column).NotNull().GreaterThan(0);
            RuleFor(b => b.Row).NotNull().GreaterThan(0);
            RuleFor(b => b.PlayerId).NotNull().GreaterThan(0);
            RuleFor(b => b.OpponentId).NotNull().GreaterThan(0);
        }
    }
}
