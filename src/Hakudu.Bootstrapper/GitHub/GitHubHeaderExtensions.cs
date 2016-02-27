/*
 * Parts of the code were taken from the Octokit.NET project
 * licensed under the MIT License (MIT).
 *
 * Original source:
 * https://github.com/octokit/octokit.net/blob/80d8ab846304c1ca3abbc293d89bf105ddb0859b/Octokit/Http/ApiInfoParser.cs
 *
 * Copyright (c) 2012 GitHub, Inc.
 * See NOTICE file in the root of the repository for license details.
 */

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Hakudu.Bootstrapper.GitHub
{
    public static class GitHubHeaderExtensions
    {
        static readonly RegexOptions _regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        static readonly Regex _linkRelRegex = new Regex("rel=\"(next|prev|first|last)\"", _regexOptions);
        static readonly Regex _linkUriRegex = new Regex("<(.+)>", _regexOptions);

        public static IDictionary<string, Uri> GetLinks(this HttpResponseHeaders headers)
        {
            var httpLinks = new Dictionary<string, Uri>();

            IEnumerable<string> linkHeaderValues;
            if (headers.TryGetValues("Link", out linkHeaderValues))
            {
                foreach (var headerValue in linkHeaderValues)
                {
                    var links = headerValue.Split(',');

                    foreach (var link in links)
                    {
                        var relMatch = _linkRelRegex.Match(link);
                        if (!relMatch.Success || relMatch.Groups.Count != 2) break;

                        var uriMatch = _linkUriRegex.Match(link);
                        if (!uriMatch.Success || uriMatch.Groups.Count != 2) break;

                        httpLinks.Add(relMatch.Groups[1].Value, new Uri(uriMatch.Groups[1].Value));
                    }
                }
            }

            return httpLinks;
        }
    }
}
