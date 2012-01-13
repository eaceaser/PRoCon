using System;

namespace PRoCon.Db
{
    using Internal.Dao;

    public interface IPRoConDatabaseAccess
    {
        PlayerDao GetPlayerAccess();
    }
}