using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    class ModFinder
    {
        Dictionary<List<TermType>, Tuple<List<int>, bool>> _pairs = new Dictionary<List<TermType>, Tuple<List<int>, bool>>();

        public void Add(List<TermType> mods, Tuple<List<int>, bool> res)
        {
            _pairs.Add(mods, res);
        }

        public Tuple<List<int>, bool> this[List<TermType> mods] { get { return Find(mods); } }

        Tuple<List<int>, bool> Find(List<TermType> mods)
        {
            var key = _pairs.Keys.FirstOrDefault(mods1 => mods.SequenceEqual(mods1));
            if (key == null)
                return null;
            return _pairs[key];
        }

        public static ModFinder Seventh()
        {
            var res = new ModFinder();
            res.Add(new List<TermType>(), new Tuple<List<int>, bool>(new List<int>(), true));
            res.Add(new List<TermType>() { TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0, 0, 0, 1 }, true));
            res.Add(new List<TermType>() { TermType.min }, new Tuple<List<int>, bool>(new List<int>() { 0, -1 }, false));
            res.Add(new List<TermType>() { TermType.min, TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0, -1, 0, 1 }, false));
            res.Add(new List<TermType>() { TermType.aug }, new Tuple<List<int>, bool>(new List<int>() { 0,0,-1 }, false));
            res.Add(new List<TermType>() { TermType.dim }, new Tuple<List<int>, bool>(new List<int>() { 0,-1,-1,-1 }, false));
            res.Add(new List<TermType>() { TermType.dim, TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0,-1,-1, 1 }, false));
            res.Add(new List<TermType>() { TermType.aug, TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0,0,1, 1 }, false));
            return res;
        }

        public static ModFinder Sixth()
        {
            var res = new ModFinder();
            res.Add(new List<TermType>(), new Tuple<List<int>, bool>(new List<int>(), true));
            res.Add(new List<TermType>() { TermType.maj }, new Tuple<List<int>, bool>(new List<int>(), true));
            res.Add(new List<TermType>() { TermType.min }, new Tuple<List<int>, bool>(new List<int>() { 0, -1 }, false));
            res.Add(new List<TermType>() { TermType.aug }, new Tuple<List<int>, bool>(new List<int>() { 0, 0, 1 }, false));
            res.Add(new List<TermType>() { TermType.dim }, new Tuple<List<int>, bool>(new List<int>() { 0, -1, -1 }, false));
            res.Add(new List<TermType>() { TermType.min, TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0, -1 }, false));
            res.Add(new List<TermType>() { TermType.dim, TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0, -1, -1 }, false));
            res.Add(new List<TermType>() { TermType.aug, TermType.maj }, new Tuple<List<int>, bool>(new List<int>() { 0, 0, 1 }, false));
            return res;
        }

        public static ModFinder Simple()
        {
            var res = new ModFinder();
            res.Add(new List<TermType>(), new Tuple<List<int>, bool>(new List<int>(), true));
            res.Add(new List<TermType>() { TermType.maj }, new Tuple<List<int>, bool>(new List<int>(), true));
            res.Add(new List<TermType>() { TermType.min }, new Tuple<List<int>, bool>(new List<int>() { 0, -1 }, false));
            res.Add(new List<TermType>() { TermType.aug }, new Tuple<List<int>, bool>(new List<int>() { 0, 0, 1 }, false));
            res.Add(new List<TermType>() { TermType.dim }, new Tuple<List<int>, bool>(new List<int>() { 0, -1, -1 }, false));
            return res;
        }
    }
}
