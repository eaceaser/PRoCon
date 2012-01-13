using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db
{
    using Domain;

    public interface IRankInfo
    {
        Player Player { get; }
        long Rank { get; }
    }
}
