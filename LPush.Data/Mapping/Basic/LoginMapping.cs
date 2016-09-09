
using LPush.Model;

namespace LPush.Data.Mapping
{
    public partial class LoginMapping : NopEntityTypeConfiguration<Login>
    {
        public LoginMapping()
        {
            this.ToTable("basic.Login");
            this.HasKey(m => m.Id);
            this.Property(m => m.LoginName);
            this.Property(m => m.Password);
            this.Property(m => m.Email);
            this.Property(m => m.UserType);
            this.Property(m => m.EnterpriseId);
            this.Property(m => m.IsDeleted);
            this.Property(m => m.CreateBy);
            this.Property(m => m.CreateDate);
            this.Property(m => m.ModifyBy);
            this.Property(m => m.ModifyDate);
        }
    }
}
