using Microsoft.EntityFrameworkCore;

namespace TripleTriad.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder UseSnakeCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            // We loop through the entity types twice.
            // First to change the table and column names,
            // then to change the names of keys and indexes.
            // This is because the table and column names are
            // used in key- and index names and we need to make
            // sure they have been transformed first.
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Relational().ColumnName.ToSnakeCase();
                }
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }
                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }
                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
                }
            }
            return modelBuilder;
        }
    }
}