using DHNet.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace DHNet.Data.Logging
{
    public class LoggableEntity
    {
        public String Name { get; private set; }
        public String Action { get; private set; }
        private static String IdName { get; set; }
        public Func<Int32> Id { get; private set; }
        public IEnumerable<LoggableProperty> Properties { get; private set; }

        static LoggableEntity()
        {
            IdName = typeof(BaseModel).GetProperties().Single(property => property.IsDefined(typeof(KeyAttribute), false)).Name;
        }

        public LoggableEntity(DbEntityEntry<BaseModel> entry)
        {
            DbPropertyValues values =
                entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                    ? entry.GetDatabaseValues()
                    : entry.CurrentValues;

            Type entityType = entry.Entity.GetType();
            if (entityType.Namespace == "System.Data.Entity.DynamicProxies") entityType = entityType.BaseType;
            Properties = values.PropertyNames.Where(name => name != IdName).Select(name => new LoggableProperty(entry.Property(name), values[name]));
            Properties = entry.State == EntityState.Modified ? Properties.Where(property => property.IsModified) : Properties;
            Properties = Properties.ToArray();
            Action = entry.State.ToString();
            Id = () => entry.Entity.Id;
            Name = entityType.Name;
        }

        public override String ToString()
        {
            StringBuilder changes = new StringBuilder();
            foreach (LoggableProperty property in Properties)
                changes.Append(property + Environment.NewLine);

            return changes.ToString();
        }
    }
}
