using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapperDirect.Execution;
using AutoMapperDirect.Internal;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace AutoMapperDirect.Mappers
{
    public class ToDynamicMapper : IObjectMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, ResolutionContext context, ProfileMap profileMap)
        {
            var sourceTypeDetails = profileMap.CreateTypeDetails(typeof(TSource));
            foreach (var member in sourceTypeDetails.PublicReadAccessors)
            {
                object sourceMemberValue;
                try
                {
                    sourceMemberValue = member.GetMemberValue(source);
                }
                catch (RuntimeBinderException)
                {
                    continue;
                }
                var destinationMemberValue = context.MapMember(member, sourceMemberValue);
                SetDynamically(member.Name, destination, destinationMemberValue);
            }
            return destination;
        }

        private static void SetDynamically(string memberName, object target, object value)
        {
            var binder = Binder.SetMember(CSharpBinderFlags.None, memberName, null,
                new[]{
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                });
            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);
            callsite.Target(callsite, target, value);
        }

        private static readonly MethodInfo MapMethodInfo = typeof(ToDynamicMapper).GetDeclaredMethod(nameof(Map));

        public bool IsMatch(TypePair context) => context.DestinationType.IsDynamic() && !context.SourceType.IsDynamic();

        public Expression MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap,
            IMemberMap memberMap, Expression sourceExpression, Expression destExpression,
            Expression contextExpression) =>
            Expression.Call(null,
                MapMethodInfo.MakeGenericMethod(sourceExpression.Type, destExpression.Type),
                sourceExpression,
                ExpressionFactory.ToType(
                    Expression.Coalesce(ExpressionFactory.ToObject(destExpression),
                        DelegateFactory.GenerateConstructorExpression(destExpression.Type)), destExpression.Type),
                contextExpression,
                Expression.Constant(profileMap));
    }
}