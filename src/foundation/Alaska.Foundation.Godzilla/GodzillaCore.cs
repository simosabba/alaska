using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla
{
    internal static class GodzillaCore
    {
        public static class Areas
        {
            public const string CollectionDefinition = "/system/collections";
            public const string RelationshipDefinition = "/system/relationships";
        }

        public static class Templates
        {
            #region Container
            public const string ContainerBase = "18c3dc4a-c7c4-49b3-82b9-d8b73133acdc";
            public const string Folder = "3204c449-7a95-4ffa-bf10-5d64f4a519e8";
            public const string Area = "f94fa74b-fbb5-4fa5-932d-22bcc29099a6";
            public const string CollectionArea = "84ff9b56-e31c-4f69-801b-db18d71deca4";
            #endregion

            #region Template
            public const string Template = "f3b2c8e2-232c-49d2-be39-68e31081a709";
            #endregion

            #region CollectionDefinition
            public const string CollectionDefinition = "6568047e-d456-409b-9721-eec7674f4de3";
            #endregion

            #region RelationshipDefinition
            public const string RelationshipDefinition = "fc6a68df-e61a-4ac6-9fbf-f0b1bd6c09ed";
            #endregion
        }

        public static class Relationships
        {
            public const string DerivedFrom = "a42233fa-52c0-459d-8138-cbe341aff30f";
        }
    }
}
