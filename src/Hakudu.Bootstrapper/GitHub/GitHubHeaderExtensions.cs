/*
 * Parts of the code were taken from the Octokit.NET project (https://github.com/octokit/octokit.net)
 * licensed under the MIT License (MIT).
 *
 * Copyright (c) 2012 GitHub, Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
