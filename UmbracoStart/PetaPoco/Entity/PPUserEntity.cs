using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace UmbracoStart.PetaPoco.Entity
{
    [TableName("PetaPocoUser")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class PPUserEntity
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("emailAddress")]
        public string EmailAddress { get; set; }
    }
}