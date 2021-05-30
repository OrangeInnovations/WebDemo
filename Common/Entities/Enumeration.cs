using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Entities
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(c => c.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var enumeration = obj as Enumeration;

            if (enumeration == null)
                return false;

            var typeMatch = GetType().Equals(obj.GetType());
            var valueMatch = Id.Equals(enumeration.Id);

            return typeMatch && valueMatch;
        }

        public override int GetHashCode() => Id.GetHashCode();


        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
