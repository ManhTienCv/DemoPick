using System;
using System.Reflection;

namespace DemoPick.Services
{
    internal static class SmokeUiReflectionHelper
    {
        internal static T GetPrivateField<T>(object target, string fieldName) where T : class
        {
            if (target == null || string.IsNullOrWhiteSpace(fieldName))
                return null;

            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                return null;

            return field.GetValue(target) as T;
        }

        internal static void InvokePrivateMethod(object target, string methodName, params object[] args)
        {
            if (target == null)
                throw new InvalidOperationException("Cannot invoke private method on null target");

            var method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
                throw new InvalidOperationException("Method not found: " + target.GetType().FullName + "." + methodName);

            method.Invoke(target, args);
        }
    }
}
