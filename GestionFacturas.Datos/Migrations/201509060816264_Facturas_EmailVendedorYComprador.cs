namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas_EmailVendedorYComprador : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "VendedorEmail", c => c.String(maxLength: 50));
            AddColumn("dbo.Facturas", "CompradorEmail", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facturas", "CompradorEmail");
            DropColumn("dbo.Facturas", "VendedorEmail");
        }
    }
}
