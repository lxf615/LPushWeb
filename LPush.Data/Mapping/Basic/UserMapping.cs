
using LPush.Model.Basic;

namespace LPush.Data.Mapping
{
    public partial class UserMapping : NopEntityTypeConfiguration<Login>
    {
        public UserMapping()
        {
            this.ToTable("Login");
            this.HasKey(m => m.Id);
            this.Property(m => m.LoginName).IsRequired().HasMaxLength(64);
            this.Property(m => m.Password).IsRequired().HasMaxLength(32);
            this.Property(m => m.Email);
            this.Property(m => m.UserType).IsRequired();
            this.Property(m => m.EnterpriseId).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.CreateBy).IsRequired().HasMaxLength(64);
            this.Property(m => m.CreateDate).IsRequired();
            this.Property(m => m.ModifyBy);
            this.Property(m => m.ModifyDate);
        }
    }
}
