﻿namespace BookStore.Domain.Common.Models;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract class ValueObject
{
    private const BindingFlags PrivateBindingFlags
        = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

    public override bool Equals(object? other)
    {
        if (other == null)
        {
            return false;
        }

        var type = this.GetType();
        var otherType = other.GetType();

        if (type != otherType)
        {
            return false;
        }

        var fields = type.GetFields(PrivateBindingFlags);

        foreach (var field in fields)
        {
            var firstValue = field.GetValue(other);
            var secondValue = field.GetValue(this);

            if (firstValue == null)
            {
                if (secondValue != null)
                {
                    return false;
                }
            }
            else if (!firstValue.Equals(secondValue))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var fields = this.GetFields();

        const int startValue = 17;
        const int multiplier = 59;

        return fields
            .Select(field => field.GetValue(this))
            .Where(value => value != null)
            .Aggregate(startValue, (current, value) => current * multiplier + value!.GetHashCode());
    }

    public static bool operator ==(ValueObject first, ValueObject second) => first.Equals(second);

    public static bool operator !=(ValueObject first, ValueObject second) => !(first == second);

    private IEnumerable<FieldInfo> GetFields()
    {
        var type = this.GetType();

        var fields = new List<FieldInfo>();

        while (type != typeof(object) && type != null)
        {
            fields.AddRange(type.GetFields(PrivateBindingFlags));

            type = type.BaseType!;
        }

        return fields;
    }
}