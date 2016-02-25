using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hakudu.Bootstrapper.GitHub
{
    public class GitHubApiClient
    {
        readonly Uri _githubEndpoint = new Uri("https://api.github.com");
        readonly string _githubRepoUri;

        readonly HttpClient _apiClient;
        readonly HttpClient _downloadClient;

        public GitHubApiClient(string user, string repo, string userAgent)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(repo))
                throw new ArgumentNullException(nameof(repo));

            _githubRepoUri = $"/repos/{user}/{repo}";

            _apiClient = new HttpClient { BaseAddress = _githubEndpoint };
            _apiClient.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github.v3.text+json");
            _apiClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            _downloadClient = new HttpClient();
            _downloadClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        }

        public async Task<GitHubReleaseList> GetReleases(GitHubReleaseList prev = null)
        {
            Uri uri;

            if (prev != null)
            {
                if (!prev.HasNext)
                    throw new InvalidOperationException("The list of releases has reached end.");

                uri = prev.NextUri;
            }
            else
                uri = new Uri($"{_githubRepoUri}/releases", UriKind.Relative);

            using (var response = await _apiClient.GetAsync(uri))
            {
                response.EnsureSuccessStatusCode();

                var releases = await response.Content.ReadAsAsync<List<GitHubRelease>>();
                var links = response.Headers.GetLinks();

                Uri nextUri;
                links.TryGetValue("next", out nextUri);

                return new GitHubReleaseList(releases, nextUri);
            }
        }

        public async Task<GitHubRelease> GetRelease(int releaseId)
        {
            using (var response = await _apiClient.GetAsync($"{_githubRepoUri}/releases/{releaseId}"))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<GitHubRelease>();
            }
        }

        public async Task<GitHubRelease> GetLatestRelease()
        {
            using (var response = await _apiClient.GetAsync($"{_githubRepoUri}/releases/latest"))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<GitHubRelease>();
            }
        }

        public async Task<string> DownloadAsset(GitHubReleaseAsset asset)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            using (var httpStream = await _downloadClient.GetStreamAsync(asset.DownloadUrl))
            {
                var fileName = Path.GetTempFileName();

                try
                {
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await httpStream.CopyToAsync(fileStream);
                        return fileName;
                    }
                }
                catch (Exception)
                {
                    File.Delete(fileName);
                    throw;
                }
            }
        }
    }
}
