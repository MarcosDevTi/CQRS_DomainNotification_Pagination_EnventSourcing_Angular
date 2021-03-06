using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapperDirect.Configuration;
using AutoMapperDirect.Mappers.Internal;

namespace AutoMapperDirect.Mappers
{
    public class DictionaryMapper : IObjectMapper
    {
        public bool IsMatch(TypePair context) => context.SourceType.IsDictionaryType() && context.DestinationType.IsDictionaryType();

        public Expression MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap,
            IMemberMap memberMap, Expression sourceExpression, Expression destExpression, Expression contextExpression)
            => CollectionMapperExpressionFactory.MapCollectionExpression(configurationProvider, profileMap, memberMap, sourceExpression, destExpression, contextExpression, typeof(Dictionary<,>), CollectionMapperExpressionFactory.MapKeyPairValueExpr);
    }
}