using System;

namespace Contracts
{
    public static class Conventions
    {
        public static bool IsEvent(Type type)
        {
            return type.Namespace.StartsWith("Contracts.Events");
        }

        public static bool IsCommand(Type type)
        {
            return type.Namespace.StartsWith("Contracts.Commands");
        }
    }
}
