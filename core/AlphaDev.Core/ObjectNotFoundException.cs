using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AlphaDev.Core
{
    public class ObjectNotFoundException<T> : ObjectNotFoundException
    {
        public ObjectNotFoundException([NotNull] params (string propertyName, object criteria)[] values) : base(
            typeof(T), values)
        {
        }
    }

    public class ObjectNotFoundException : Exception
    {
        public Type Type { get; }

        public IDictionary<string, object[]> Criteria { get; }

        public ObjectNotFoundException(Type type, [NotNull] params (string propertyName, object criteria)[] values)
        {
            Type = type;
            Criteria = values.GroupBy(tuple => tuple.propertyName)
                           .ToDictionary(tuple => tuple.Key,
                               tuples => tuples.Select(tuple => tuple.criteria).ToArray());
            var criteria = Criteria.SelectMany(pair => pair.Value.Select(o => $"{Environment.NewLine}{pair.Key}:{o}"));
            Message = $"{Type.FullName} was not found based on the criteria.{string.Join(string.Empty, criteria)}";
        }

        public override string Message { get; }
    }
}