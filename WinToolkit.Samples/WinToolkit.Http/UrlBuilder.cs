using System;
using System.Collections.Generic;
using System.Linq;

namespace WinToolkit.Http
{
    public sealed class UrlBuilder
    {
        private readonly string _host;
        private readonly string _version;
        private readonly string _scheme;
        private readonly Dictionary<string, string> _params = new Dictionary<string, string>();
        private string _path = string.Empty;

        public UrlBuilder(string host, string version): this("https", host, version)
        {

        }
        public UrlBuilder(string scheme, string host, string version)
        {
            _scheme = scheme;
            _host = host;
            _version = version;
        }

        public void WithPath(string path) => _path = path;

        public void WithParam(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(nameof(value));

            _params.Add(name, value);
        }

        public string Build()
        {
            string queryParams = string.Join("&", _params.Select(p => $"{p.Key}={p.Value}"));
            return $"{_scheme}://{_host}/{_version}/{_path}?{queryParams}";
        }
    }
}
