using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class MusicStoreDbInitializer : System.Data.Entity.DropCreateDatabaseAlways<MusicStoreDb>
    {
        protected override void Seed(MusicStoreDb context)
        {
            context.Artists.Add(new Artist { Name = "Okka Magadu" });
            context.Genres.Add(new Genre { Name = "Rock" });
            context.Albums.Add(new Album
            {
                Artist = new Artist { Name = "DSP" },
                Genre = new Genre { Name = "Rock" },
                Price = 9.99m,
                Title = "Caravan"
            });
            base.Seed(context);
        }
    }

}