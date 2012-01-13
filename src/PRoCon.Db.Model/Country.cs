using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Domain
{
    using NHibernate.Mapping.Attributes;

    [Class(NameType = typeof(Country), Table = "country")]
    public class Country
    {
        [Id(Name ="Id", TypeType = typeof(long), Column = "id")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        [Property(Column = "name")]
        public virtual string Name { get; set; }
    }
}
