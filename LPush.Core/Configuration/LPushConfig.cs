using System;
using System.Configuration;
using System.Xml;

using LPush.Core.Data;
namespace LPush.Core.Configuration
{
    /// <summary>
    /// Represents a LPushConfig
    /// </summary>
    public partial class LPushConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new LPushConfig();

            //redis
            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            config.RedisCachingEnabled = GetBool(redisCachingNode, "Enabled");
            if (config.RedisCachingEnabled)
            {
                config.RedisCachingConnectionString = GetString(redisCachingNode, "ConnectionString");
            }

            //Database
            DataSettings settings = config.DataSettings = new DataSettings();
            settings.DataConnectionString = new System.Collections.Generic.List<string>();
            var dataProviderNode = section.SelectSingleNode("DataProvider");
            settings.DataProvider = GetString(dataProviderNode, "Type");
            var databaseNodes = section.SelectNodes("Database");
            for (int i = 0; i < databaseNodes.Count; i++)
            {
                settings.DataConnectionString.Add(GetString(databaseNodes[i], "ConnectionString"));
            }

            return config;
        }

        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        /// <summary>
        /// Indicates whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }
        /// <summary>
        /// Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }


        /// <summary>
        /// The type of dataprovider of database
        /// </summary>
        public string DataProvider { get; private set; }
        /// <summary>
        /// Database connection string.
        /// </summary>
        public string DataConnectionString { get; private set; }


        public DataSettings DataSettings { get; private set; }
    }
}
