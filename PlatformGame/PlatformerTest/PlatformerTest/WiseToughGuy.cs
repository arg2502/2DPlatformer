using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerTest
{
    class WiseToughGuy:WiseGuy
    {
        public WiseToughGuy(Block[] bls, List<Hero> list)
            :base(bls,list)
        {
            isJumpable = false;
            damage = 2;
        }
    }
}
