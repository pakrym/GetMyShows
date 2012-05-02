using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace MyShows.Core.Models
{
    public class DataContext
    {
        private IEmbeddedObjectContainer container;

        public DataContext()
        {
            container = OpenFile();
        }

        public IEnumerable<Profile> GetProfiles()
        {
            
                return container.AsQueryable<Profile>().ToArray();
            
        }

        private static IEmbeddedObjectContainer OpenFile()
        {
            return Db4oEmbedded.OpenFile("database.db4o");
        }

        public void StoreProfiles(IEnumerable<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                StoreProfile(profile);
            }
            
        }

        public void StoreProfile(Profile profile)
        {
            
                container.Store(profile);
                container.Commit();
            
        }

        public void RemoveProfiles(IEnumerable<Profile> profiles)
        {
                foreach (var profile in profiles)
                {
                    container.Delete(profile);
                }
                container.Commit();
            
        }
    }
}