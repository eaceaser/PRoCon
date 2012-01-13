namespace PRoCon.Db.Domain
{
    using Net.Commons.Lang.Builder;
    using NHibernate.Mapping.Attributes;

    [Class(NameType = typeof (Ban), Table = "player_ban")]
    public class Ban
    {
        [Id(Name = "Id", Column = "id", TypeType = typeof (long))]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        [ManyToOne(ClassType = typeof (Player), Column = "player_id")]
        public virtual Player Player { get; set; }

        [Property(Column = "reason")]
        public virtual string Reason { get; set; }

        [Property(Column = "duration")]
        public virtual string Duration { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as Ban;
            if (entity != null)
            {
                return new EqualsBuilder()
                    .Append(this.Player, entity.Player)
                    .Append(this.Reason, entity.Reason)
                    .Append(this.Duration, entity.Duration)
                    .IsEqual;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(this.Player)
                .Append(this.Reason)
                .Append(this.Duration)
                .ToHashCode();
        }
    }
}