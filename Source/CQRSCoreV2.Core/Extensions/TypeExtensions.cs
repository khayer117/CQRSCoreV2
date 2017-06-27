namespace CQRSCoreV2.Core
{
    using System;
    using System.Text;

    public static class TypeExtensions
    {
        public static bool Is<TType>(this Type instance)
        {
            return typeof(TType).IsAssignableFrom(instance);
        }

        public static string ReadableName(this Type instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var builder = new StringBuilder();

            if (instance.Namespace != null)
            {
                builder.Append(instance.Namespace);
                builder.Append(".");
            }

            var typeName = instance.Name;
            var genericMarkerIndex = typeName.IndexOf(
                "`",
                StringComparison.Ordinal);

            if (genericMarkerIndex > 0)
            {
                typeName = typeName.Substring(0, genericMarkerIndex);
            }

            builder.Append(typeName);

            var arguments = instance.GetGenericArguments();

            if (arguments.Length == 0)
            {
                return builder.ToString();
            }

            builder.Append("<");

            for (var i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(ReadableName(arguments[i]));
            }

            builder.Append(">");

            return builder.ToString();
        }

        public static Type RealType(this Type instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var underlyingType = Nullable.GetUnderlyingType(instance);

            return underlyingType ?? instance;
        }
    }
}