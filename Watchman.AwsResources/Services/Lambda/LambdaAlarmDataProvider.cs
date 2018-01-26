using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.CloudWatch.Model;
using Amazon.Lambda.Model;
using Watchman.Configuration.Generic;

namespace Watchman.AwsResources.Services.Lambda
{
    public class LambdaAlarmDataProvider : IAlarmDimensionProvider<FunctionConfiguration, ResourceConfig>,
        IResourceAttributesProvider<FunctionConfiguration, ResourceConfig>
    {
        public List<Dimension> GetDimensions(FunctionConfiguration resource, ResourceConfig config, IList<string> dimensionNames)
        {
            return dimensionNames
                .Select(x => GetDimension(resource, x))
                .ToList();
        }

        private Dimension GetDimension(FunctionConfiguration resource, string dimensionName)
        {
            var dim = new Dimension
                {
                    Name = dimensionName
                };

            switch (dimensionName)
            {
                case "FunctionName":
                    dim.Value = resource.FunctionName;
                    break;

                default:
                    throw new Exception("Unsupported dimension " + dimensionName);
            }

            return dim;
        }

        public decimal GetValue(FunctionConfiguration resource, ResourceConfig config, string property)
        {
            switch (property)
            {
                case "Timeout":
                    // alarm needs timeout in milliseconds
                    return resource.Timeout * 1000;
            }

            throw new Exception("Unsupported Lambda property name");
        }
    }
}
