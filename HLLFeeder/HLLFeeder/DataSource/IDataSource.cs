using System.Collections.Generic;

namespace HLLFeeder.DataSource
{
    interface IDataSource: IEnumerable<string>
    {
        double ItemsCount { get; }
        double UniqueItemsCount { get; }
    }
}
