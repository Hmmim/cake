﻿using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is static.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Whether or not the specified type is static</returns>
        public static bool IsStatic(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// Gets the full name of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNamespace">if set to <c>true</c> then namespace is included.</param>
        /// <returns>The full name of a type</returns>
        public static string GetFullName(this Type type, bool includeNamespace = true)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.IsGenericParameter)
            {
                return type.Name;
            }
            return type.IsGenericType
                ? GetGenericTypeName(type, includeNamespace)
                : includeNamespace ? type.FullName : type.Name;
        }

        private static string GetGenericTypeName(this Type type, bool includeNamespace)
        {
            var builder = new StringBuilder();
            if (includeNamespace)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }
            builder.Append(type.Name.Substring(0, type.Name.IndexOf('`')));
            builder.Append("<");
            builder.Append(GetGenericTypeArguments(type, includeNamespace));
            builder.Append(">");
            return builder.ToString();
        }

        private static string GetGenericTypeArguments(this Type type, bool includeNamespace)
        {
            var genericArguments = new List<string>();
            foreach (var argument in type.GetGenericArguments())
            {
                genericArguments.Add(GetFullName(argument, includeNamespace));
            }
            return string.Join(", ", genericArguments);
        }
    }
}