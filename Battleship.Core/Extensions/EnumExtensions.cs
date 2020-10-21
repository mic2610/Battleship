using MicroserviceTemplate.Core.Extensions;
using System;

namespace MicroserviceTemplate.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var member = type.GetMember(name)[0];
            return member.Name;
        }
    }
}
