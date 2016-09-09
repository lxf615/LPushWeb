using System;
using LPush.Core;
using LPush.Core.Data;

namespace LPush.Data
{
    public partial class DataProviderManager : BaseDataProviderManager
    {
        public DataProviderManager(DataSettings settings) : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {

            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new Exception("Data Settings doesn't contain a providerName");

            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
                case "mysql":
                    return new MySqlServerDataProvider();
                default:
                    throw new Exception(string.Format("Not supported dataprovider name: {0}", providerName));
            }
        }

    }
}
