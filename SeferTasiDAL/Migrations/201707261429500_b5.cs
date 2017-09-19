namespace SeferTasiDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Siparisler", "SiparisinUlastigiTarih", c => c.DateTime(storeType: "smalldatetime"));
            DropColumn("dbo.Siparisler", "SiparisVarabilecegiTarih");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Siparisler", "SiparisVarabilecegiTarih", c => c.DateTime(storeType: "smalldatetime"));
            DropColumn("dbo.Siparisler", "SiparisinUlastigiTarih");
        }
    }
}
