using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KonsolideRapor.Web.App_Code
{
    public class KendoPagingParameter
    {
        public string IdField = "ObjId";

        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<KendoPagingFilter> Filters { get; set; }
        public List<KendoPagingSort> SortOrders { get; set; }

        public KendoPagingParameter()
        {
            Filters = new List<KendoPagingFilter>();
            SortOrders = new List<KendoPagingSort>();
        }

        public void Fill()
        {
            NameValueCollection form = System.Web.HttpContext.Current.Request.Form;

            Take = int.Parse(form["take"]);
            Skip = int.Parse(form["skip"]); 
            Page = int.Parse(form["page"]); 
            PageSize = int.Parse(form["pageSize"]); 

            Filters.Clear();
            for (int i = 0; i < 100; i++)
            {
                string key = "filter[filters][" + i + "]";
                if (form[key + "[field]"] == null) break;

                KendoPagingFilter filter = new KendoPagingFilter();
                filter.Field = form[key + "[field]"];
                filter.Operator = form[key + "[operator]"];
                filter.Value = form[key + "[value]"];

                Filters.Add(filter);
            }

            SortOrders.Clear();
            for (int i = 0; i < 100; i++)
            {
                string key = "sort[" + i + "]";
                if (form[key + "[field]"] == null) break;

                KendoPagingSort sort = new KendoPagingSort();
                sort.Field = form[key + "[field]"];
                sort.Direction = form[key + "[dir]"];

                SortOrders.Add(sort);
            }
        }

        public string BuildSql(string sql, List<object> paramValues)
        {
            string whereString = "";
            foreach (var item in Filters)
            {
                if (!string.IsNullOrEmpty(whereString)) whereString += " and ";

                whereString += item.Field;
                switch (item.Operator)
                {
                    case "neq":
                        whereString += " <> ";
                        break;
                    case "endswith":
                        whereString += " like ";
                        if (!item.Value.StartsWith("%")) item.Value = "%" + item.Value;
                        break;
                    case "doesnotcontain":
                        whereString += " not like ";
                        if (!item.Value.StartsWith("%")) item.Value = "%" + item.Value;
                        if (!item.Value.EndsWith("%")) item.Value += "%";
                        break;
                    case "contains":
                        whereString += " like ";
                        if (!item.Value.StartsWith("%")) item.Value = "%" + item.Value;
                        if (!item.Value.EndsWith("%")) item.Value += "%";
                        break;
                    case "startswith":
                        whereString += " like ";
                        if (!item.Value.EndsWith("%")) item.Value += "%";
                        break;
                    default: whereString += " = "; break;
                }
                whereString += "@prm" + paramValues.Count;
                paramValues.Add(item.Value);
            }
            if (!string.IsNullOrEmpty(whereString)) whereString = "where " + whereString;

            string orderString = "";
            foreach (var item in this.SortOrders)
            {
                if (!string.IsNullOrEmpty(orderString)) orderString += ", ";
                orderString += item.Field + " " + item.Direction;
            }
            if (!string.IsNullOrEmpty(orderString)) orderString = "order by " + orderString;

            string pagingOrder = orderString;
            if (string.IsNullOrEmpty(pagingOrder)) pagingOrder = "order by " + this.IdField;

            string pagingParam0 = "@prm" + paramValues.Count;
            paramValues.Add(this.Skip + 1);
            string pagingParam1 = "@prm" + paramValues.Count;
            paramValues.Add(this.Skip + this.Take + 1); // +1'in sebebi snraki sayfa var mi tespit edebilmek

            string result = string.Format(
                @"select * 
                  from (
                    select top 500 Row_Number() over ({3}) PagingRowNumber, *
                    from (
                        {0}
                    ) systemAlias1
                    {1}
                    {2}
                  ) systemAlias2
                  where PagingRowNumber between {4} and {5} 
                 ", sql, whereString, orderString, pagingOrder, pagingParam0, pagingParam1);

            return result;
        }

        public JsonResult JsonResult(IList list)
        {
            int numberOfRecords = this.Take + this.Skip;
            if (list != null && list.Count > this.Take)
            {
                numberOfRecords++;
            }
            while (list != null && list.Count > this.Take)
            {
                list.RemoveAt(list.Count - 1);
            }

            return new JsonResult(){ Data = new { total = numberOfRecords, data = list }};
        }
    }

    public class KendoPagingFilter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    public class KendoPagingSort
    {
        public string Field { get; set; }
        public string Direction { get; set; }
    }

    public class EFaturaHelper
    {
        public static JsonResult Success(object data)
        {
            return new JsonResult() { Data = new { success = true, message = "", data = data } };
        }

        public static JsonResult Error(string message, object data)
        {
            return new JsonResult() { Data = new { success = false, message = message, data = data } };
        }
    }
}