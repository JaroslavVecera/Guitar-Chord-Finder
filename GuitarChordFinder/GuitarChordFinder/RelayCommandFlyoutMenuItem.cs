using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    public class RelayCommandFlyoutMenuItem
    {
        public RelayCommandFlyoutMenuItem()
        {
            TargetType = typeof(RelayCommandFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}