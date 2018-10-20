using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Converters
{
    internal class EntityConverter
    {
        //public Guid GetEntityTemplateId(Type type)
        //{
        //    return EntityContext.Current.TypesMap.GetTemplateId(type);
        //}

        //public IEntity CreateEntityInstance(Guid templateId, object value)
        //{
        //    var type = EntityContext.Current.TypesMap.GetEntityType(templateId);
        //    if (!type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEntity)))
        //        throw new InvalidEntityTypeException($"Type {type.GetType().FullName} does not implements interface {typeof(IEntity).Name}");

        //    var serializedObject = JsonConvert.SerializeObject(value);
        //    return (IEntity)JsonConvert.DeserializeObject(serializedObject, type);
        //}
    }
}
