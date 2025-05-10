using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibely_App.Data.Models;

namespace Vibely_App.API
{
    public interface IPlaylistBusiness : IBusiness<Playlist>
    {
        public Playlist? FindByName(string name);
    }
}
