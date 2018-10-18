//using Alaska.Foundation.Godzilla.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Mongo.Helpers
//{
//    public static class MongoFiltersHelper<T>
//            where T : IDatabaseCollectionElement
//    {
//        public static FilterDefinition<T> Build(Filter<T> filter)
//        {
//            try
//            {
//                return BuildFilter(filter);
//            }
//            catch (Exception e)
//            {
//                throw new MongoFilterCompositionException("Mongo filter composition error", e);
//            }
//        }

//        private static FilterDefinition<T> BuildFilter(Filter<T> filter)
//        {
//            if (filter is RegexFilter<T>)
//                return BuildRegexFilter((RegexFilter<T>)filter);
//            if (filter is ExpressionFilter<T>)
//                return BuildExpressionFilter((ExpressionFilter<T>)filter);
//            if (filter is ComposedFilter<T>)
//                return BuildComposedFilter((ComposedFilter<T>)filter);

//            throw new NotImplementedException($"Filter type {filter.GetType().FullName} not implemented");
//        }

//        private static FilterDefinition<T> BuildRegexFilter(RegexFilter<T> filter)
//        {
//            return Builders<T>.Filter.Regex(filter.Field, new MongoDB.Bson.BsonRegularExpression(filter.Pattern));
//        }

//        private static FilterDefinition<T> BuildExpressionFilter(ExpressionFilter<T> filter)
//        {
//            return Builders<T>.Filter.Where(filter.Expression);
//        }

//        private static FilterDefinition<T> BuildNegativeFilter(NegativeFilter<T> filter)
//        {
//            var innerFilter = BuildFilter(filter);
//            return Builders<T>.Filter.Not(innerFilter);
//        }

//        private static FilterDefinition<T> BuildComposedFilter(ComposedFilter<T> filter)
//        {
//            var innerFilters = filter.InnerFilters.Select(x => BuildFilter(x));
//            switch (filter.Operator)
//            {
//                case Operator.And:
//                    return Builders<T>.Filter.And(innerFilters);
//                case Operator.Or:
//                    return Builders<T>.Filter.Or(innerFilters);
//                default:
//                    throw new NotImplementedException($"Filter operator {filter.Operator} not implemented");
//            }
//        }
//    }
//}
