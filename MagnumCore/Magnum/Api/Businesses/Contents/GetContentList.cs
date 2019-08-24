using System;
using System.Collections.Generic;
using Magnum.Api.Commons.Business;
using Magnum.Api.Commons.Table;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Contents
{
    public class GetContentList : BusinessOperationBase, IBusinessOperationQuery<MContent>
    {
        public IEnumerable<MContent> Apply(MContent dat, CTable param)
        {
            //Parameter "dat" can be use for create the filter in the future
            var ctx = GetNoSqlContext();
            var path = "contents";
            IEnumerable<MContent> contents = ctx.GetObjectList<MContent>(path);

            return contents;
        }
    }
}
