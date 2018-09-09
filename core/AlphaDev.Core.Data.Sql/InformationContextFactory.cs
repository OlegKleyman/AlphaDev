﻿using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;

namespace AlphaDev.Core.Data.Sql
{
    public class InformationContextFactory : AlphaContextFactory<InformationContext>
    {
        public InformationContextFactory() : this(null)
        {
        }

        public InformationContextFactory(Configurer configurer) : base(configurer)
        {
        }
    }
}