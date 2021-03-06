﻿using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Arch.Domain.Core
{
    public abstract class Entity: ICloneable
    {
        public Guid Id { get; protected set; }
      
        
        public Guid GetId(Guid? id) => id ?? Guid.NewGuid();
        public DateTime CreatedDate { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
