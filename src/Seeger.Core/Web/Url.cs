using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web
{
    public class Url
    {
        public string Schema { get; private set; }
        public string Domain { get; private set; }
        public int Port { get; private set; }
        public string Path { get; private set; }
        public string Query { get; private set; }
        public string FileExtension { get; private set; }
        public SegmentCollection Segments { get; private set; }

        public Url(string url)
        {
            Require.NotNullOrEmpty(url, "url");

            Port = 80;
            Path = "/";
            Query = String.Empty;
            FileExtension = String.Empty;
            Segments = new SegmentCollection(this);

            Parse(url);

            if (Path.Length > 1)
            {
                int index = Path.LastIndexOf('.', Path.Length - 1);
                if (index > 0)
                {
                    FileExtension = Path.Substring(index).TrimEnd('/');
                }
            }
        }

        public override string ToString()
        {
            StringBuilder url = new StringBuilder();

            if (!String.IsNullOrEmpty(Schema))
            {
                url.Append(Schema).Append("://").Append(Domain);
                if (Port != 80)
                {
                    url.Append(":").Append(Port);
                }
            }
            url.Append(Path);
            url.Append(Query);

            return url.ToString();
        }

        #region Parser

        void Parse(string url)
        {
            if (url == "/")
            {
                Path = "/";
                return;
            }

            ParseSchema(url);
        }

        void ParseSchema(string url)
        {
            if (url.StartsWith("/"))
            {
                ParsePath(url);
                return;
            }

            int index = url.IndexOf("://");
            if (index == 0)
            {
                throw new InvalidOperationException("Invalid url.");
            }
            else if (index < 0)
            {
                throw new InvalidOperationException("Not support relative url.");
            }
            else
            {
                Schema = url.Substring(0, index);
                if (index + 3 == url.Length)
                {
                    throw new InvalidOperationException("Invalid url.");
                }
                ParseDomain(url.Substring(index + 3));
            }
        }

        void ParseDomain(string urlPart)
        {
            int index = urlPart.IndexOf(':');
            if (index < 0)
            {
                index = urlPart.IndexOf('/');
                if (index > 0)
                {
                    Domain = urlPart.Substring(0, index);
                    ParsePath(urlPart.Substring(index));
                }
                else
                {
                    Domain = urlPart;
                }
            }
            else
            {
                Domain = urlPart.Substring(0, index);
                ++index;

                string port = String.Empty;
                
                if (index == urlPart.Length)
                {
                    throw new InvalidOperationException("Invaid url.");
                }
                int indexOfSlash = urlPart.IndexOf('/', index);
                if (indexOfSlash > 0)
                {
                    port = urlPart.Substring(index, indexOfSlash - index);
                }
                else
                {
                    port = urlPart.Substring(index);
                }

                int intPort = 0;

                if (!Int32.TryParse(port, out intPort))
                {
                    throw new InvalidOperationException("Invalid url.");
                }

                this.Port = intPort;

                if (indexOfSlash > 0)
                {
                    ParsePath(urlPart.Substring(indexOfSlash));
                }
            }
        }

        void ParsePath(string urlPart)
        {
            int indexOfAsk = urlPart.IndexOf('?');
            if (indexOfAsk < 0)
            {
                Path = urlPart;
            }
            else if (indexOfAsk == 0)
            {
                Path = "/";
                Query = urlPart;
            }
            else
            {
                Path = urlPart.Substring(0, indexOfAsk);
                Query = urlPart.Substring(indexOfAsk);
            }
        }

        #endregion

        public class SegmentCollection : IEnumerable<string>
        {
            private Url _url;
            private List<string> _segments;

            public SegmentCollection(Url url)
            {
                Require.NotNull(url, "url");

                _url = url;
            }

            public int Count
            {
                get
                {
                    EnsureSegmentsParsed();
                    return _segments.Count;
                }
            }

            public string this[int index]
            {
                get
                {
                    EnsureSegmentsParsed();
                    return _segments[index];
                }
            }

            void EnsureSegmentsParsed()
            {
                if (_segments == null)
                {
                    _segments = _url.Path.Split(new String[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }

            public IEnumerator<string> GetEnumerator()
            {
                EnsureSegmentsParsed();

                return _segments.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
