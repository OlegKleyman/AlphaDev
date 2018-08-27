using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace AlphaDev.Core.Data.Sql.Tests.Unit
{
    public class InformationContextFactoryTests
    {
        [NotNull]
        private static InformationContextFactory GetInformationContextFactory(IConfigurationRoot config)
        {
            return new InformationContextFactory(config);
        }


    }
}
