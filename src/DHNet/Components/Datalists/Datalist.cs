using Datalist;
using DHNet.Data.Core;
using DHNet.Objects;
using DHNet.Resources;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DHNet.Components.Datalists
{
    public class Datalist<TModel, TView> : GenericDatalist<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public Datalist(UrlHelper url) : base(url)
        {
            String view = typeof(TView).Name.Replace("View", "");
            DialogTitle = ResourceProvider.GetDatalistTitle(view);
            DatalistUrl = url.Action(view, Prefix, new { area = "" });
        }
        public Datalist(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override String GetColumnHeader(PropertyInfo property)
        {
            DatalistColumnAttribute column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column != null && column.Relation != null)
                return GetColumnHeader(property.PropertyType.GetProperty(column.Relation));

            return ResourceProvider.GetPropertyTitle(typeof(TView), property.Name) ?? "";
        }
        protected override String GetColumnCssClass(PropertyInfo property)
        {
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum) return "text-left";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "text-right";
                case TypeCode.DateTime:
                    return "text-center";
                default:
                    return "text-left";
            }
        }

        protected override IQueryable<TView> GetModels()
        {
            return UnitOfWork.Select<TModel>().To<TView>();
        }

        protected override IQueryable<TView> FilterById(IQueryable<TView> models)
        {
            Int32 id;
            if (!Int32.TryParse(CurrentFilter.Id, out id))
                return Enumerable.Empty<TView>().AsQueryable();

            return UnitOfWork.Select<TModel>().Where(model => model.Id == id).To<TView>();
        }
    }
}
