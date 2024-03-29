﻿using Microsoft.AspNetCore.WebUtilities;
using WebApiDB.Interfaces;
using WebApiDB.Pagination;

namespace WebApiDB.Servics
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
