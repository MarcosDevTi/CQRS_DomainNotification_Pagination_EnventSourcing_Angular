﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arch.Infra.Shared.Grid
{
    public class GridOutils
    {
        public static (string memberName, bool editable, string displayName)[] GetHeadGenericGrid(Type type)
        {
            var tipoGen = typeof(GridFluent<>).MakeGenericType(type);

            var target = type.Assembly;
            var assemblies = target.GetReferencedAssemblies()
                .Select(Assembly.Load).ToList();
            assemblies.Add(target);

            var map = assemblies.SelectMany(_ => _.GetExportedTypes())
                .FirstOrDefault(_ => tipoGen.IsAssignableFrom(_));
            var entityType = (dynamic)Activator.CreateInstance(map);
            entityType.Configuration(entityType);

            var membersName = ((List<(string memberName, bool editable, string displayName)>)entityType.MembersDisplayName)
                .Select(_ => (_.memberName, _.editable, _.displayName));

            return membersName.ToArray();
        }
    }
}
