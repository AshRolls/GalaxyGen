using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GalaxyGen.Model
{
    internal class GalaxyInitialiser : DropCreateDatabaseIfModelChanges<GalaxyContext>
    {
        protected override void Seed(GalaxyContext context)
        {
            // add any db seeding here

            base.Seed(context);
        }

    }
}