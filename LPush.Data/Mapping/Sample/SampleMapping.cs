using System.ComponentModel.DataAnnotations.Schema;

using LPush.Model.Sample;

namespace LPush.Data.Mapping
{
    public partial class ExampleMapping : NopEntityTypeConfiguration<Example>
    {
        public ExampleMapping()
        {
            this.ToTable("example");
			this.HasKey(m => m.Id);
            this.Property(m => m.FirstName).IsRequired().HasMaxLength(10);
            this.Property(m => m.LastName).IsRequired().HasMaxLength(10);
            this.Property(m => m.CreateDt).IsRequired();
            this.Property(m => m.CreateBy).HasMaxLength(20);//用户登录名.
            this.Property(m => m.Deleted).IsRequired();//用户登录名.
        }
    }
}
