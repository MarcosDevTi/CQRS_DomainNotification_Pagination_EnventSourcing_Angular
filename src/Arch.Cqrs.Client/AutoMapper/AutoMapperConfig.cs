﻿
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AutoMapper;


namespace Arch.Cqrs.Client.AutoMapper
{
    public class AutoMapperConfig
    {

        private readonly IMapper _mapper;

        public AutoMapperConfig(IMapper mapper)
        {
            _mapper = mapper;
        }

        public static void Register<T>(Func<AssemblyName, bool> filter = null)
        {
            var target = typeof(T).Assembly;

            bool AllFilters(AssemblyName x) => true;

            var assembliesLoad = target.GetReferencedAssemblies()
                .Where(filter ?? AllFilters)
                .Select(Assembly.Load)
                .ToList();
            assembliesLoad.Add(target);

            var types = assembliesLoad.SelectMany(a => a.GetExportedTypes()).ToArray();

            Mapper.Initialize(config =>
            {
                CustomMappings(config, types);
                LoadIMapToMappings(config, types);
                LoadIMapFromMappings(config, types);
            });
        }
        private static void CustomMappings(IMapperConfigurationExpression config, Type[] types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(ICustomMapper).IsAssignableFrom(t) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select (ICustomMapper)Activator.CreateInstance(t)).ToList();

            foreach (var m in maps)
            {
                m.Map(config);
            }
            //maps.ForEach(m => m.Map(config));
        }

        private static void LoadIMapToMappings(IMapperConfigurationExpression cfg, Type[] types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Src = t,
                            Dest = i.GetGenericArguments()[0]
                        }).ToList();


            maps.ForEach(m => cfg.CreateMap(m.Src, m.Dest));
        }

        private static void LoadIMapFromMappings(IMapperConfigurationExpression cfg, Type[] types)
        {
            var maps = (from t in types

                        from i in t.GetInterfaces()

                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Src = i.GetGenericArguments()[0],
                            Dest = t
                        }).ToList();

            maps.ForEach(m => cfg.CreateMap(m.Src, m.Dest));
        }
    }
}
